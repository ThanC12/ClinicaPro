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

        var a = await _repo.GetByIdAsync(id, ct);
        if (a is null) return false;

        if (req.DurationMinutes <= 0)
            throw new ArgumentException("DurationMinutes debe ser mayor a 0.");

        var start = req.ScheduledAtUtc;
        var end = start.AddMinutes(req.DurationMinutes);

        // Choque excluyendo la misma cita (id)
        var hasOverlap = await _repo.HasOverlapExcludingAsync(
            appointmentId: id,
            patientId: a.PatientId,
            startUtc: start,
            endUtc: end,
            ct: ct
        );

        if (hasOverlap)
            throw new ConflictException("El paciente ya tiene una cita en ese horario.");

        a.ScheduledAtUtc = req.ScheduledAtUtc;
        a.DurationMinutes = req.DurationMinutes;
        a.Reason = req.Reason;
        a.Status = req.Status;

        var updated = await _repo.UpdateAsync(a, ct);
        if (!updated) return false;

        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
