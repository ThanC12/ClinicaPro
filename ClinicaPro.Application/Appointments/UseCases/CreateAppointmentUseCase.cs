using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.Ports;
using ClinicaPro.Application.Common;

using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.Appointments.UseCases;

public class CreateAppointmentUseCase
{
    private readonly IAppointmentRepository _repo;

    public CreateAppointmentUseCase(IAppointmentRepository repo) => _repo = repo;

    public async Task<AppointmentResponse> ExecuteAsync(CreateAppointmentRequest req, CancellationToken ct = default)
    {
        if (req is null) throw new ArgumentNullException(nameof(req));

        if (req.PatientId == Guid.Empty)
            throw new ArgumentException("PatientId es obligatorio.");

        if (req.DurationMinutes <= 0)
            throw new ArgumentException("DurationMinutes debe ser mayor a 0.");

        // Validación: paciente existe
        var exists = await _repo.PatientExistsAsync(req.PatientId, ct);
        if (!exists)
            throw new NotFoundException("El paciente no existe.");


        // Validación: choque (rango)
        var start = req.ScheduledAtUtc;
        var end = req.ScheduledAtUtc.AddMinutes(req.DurationMinutes);

        if (await _repo.HasOverlapAsync(req.PatientId, start, end, ct))
            throw new InvalidOperationException("El paciente ya tiene una cita en ese horario.");
        var appt = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = req.PatientId,
            ScheduledAtUtc = req.ScheduledAtUtc,
            DurationMinutes = req.DurationMinutes,
            Reason = req.Reason,
            Status = "Scheduled",
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(appt, ct);


        return new AppointmentResponse
        {
            Id = appt.Id,
            PatientId = appt.PatientId,
            ScheduledAtUtc = appt.ScheduledAtUtc,
            DurationMinutes = appt.DurationMinutes,
            Reason = appt.Reason,
            Status = appt.Status,
            CreatedAtUtc = appt.CreatedAtUtc
        };
    }
}