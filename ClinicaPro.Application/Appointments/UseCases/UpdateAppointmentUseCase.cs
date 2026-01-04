using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.Ports;
using ClinicaPro.Application.Common;

namespace ClinicaPro.Application.Appointments.UseCases;

public class UpdateAppointmentUseCase
{
    private readonly IAppointmentRepository _repo;

    public UpdateAppointmentUseCase(IAppointmentRepository repo) => _repo = repo;

    public async Task<bool> ExecuteAsync(Guid id, UpdateAppointmentRequest req, CancellationToken ct = default)
    {
        if (req is null) throw new ArgumentNullException(nameof(req));

        // 1) Traer cita
        var a = await _repo.GetByIdAsync(id, ct);
        if (a is null) return false;

        // 2) Validaciones básicas
        if (req.ScheduledAtUtc == default)
            throw new ArgumentException("ScheduledAtUtc es requerido.");

        if (req.DurationMinutes <= 0)
            throw new ArgumentException("DurationMinutes debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(req.Status))
            throw new ArgumentException("Status es requerido.");

        var start = req.ScheduledAtUtc;
        var end = start.AddMinutes(req.DurationMinutes);

        // 3) Choque por PACIENTE excluyendo la misma cita
        var patientOverlap = await _repo.HasOverlapExcludingAsync(
            appointmentId: id,
            patientId: a.PatientId,
            startUtc: start,
            endUtc: end,
            ct: ct
        );

        if (patientOverlap)
            throw new ConflictException("El paciente ya tiene una cita en ese horario.");

        // 4) Choque por DOCTOR (cuando ya exista DoctorId)
        //    Si todavía no agregas DoctorId a Appointment/DTO/Repo, comenta este bloque por ahora.
        if (a.DoctorId != Guid.Empty)
        {
            var doctorOverlap = await _repo.HasDoctorOverlapExcludingAsync(
                appointmentId: id,
                doctorId: a.DoctorId,
                startUtc: start,
                endUtc: end,
                ct: ct
            );

            if (doctorOverlap)
                throw new ConflictException("El doctor ya tiene una cita en ese horario.");
        }

        // 5) Aplicar cambios
        a.ScheduledAtUtc = req.ScheduledAtUtc;
        a.DurationMinutes = req.DurationMinutes;
        a.Reason = req.Reason ?? a.Reason; // evita pisar con null si tu DTO lo permite
        a.Status = req.Status;

        // 6) Guardar
        var updated = await _repo.UpdateAsync(a, ct);
        if (!updated) return false;

        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
