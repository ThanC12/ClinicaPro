using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.Ports;
using ClinicaPro.Application.Common;
using ClinicaPro.Domain.Entities;
using ClinicaPro.Domain.Enums;

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

        if (req.DoctorId == Guid.Empty)
            throw new ArgumentException("DoctorId es obligatorio.");

        if (req.ScheduledAtUtc == default)
            throw new ArgumentException("ScheduledAtUtc es obligatorio.");

        if (req.DurationMinutes <= 0)
            throw new ArgumentException("DurationMinutes debe ser mayor a 0.");

        // Paciente existe
        if (!await _repo.PatientExistsAsync(req.PatientId, ct))
            throw new NotFoundException("El paciente no existe.");

        // Doctor existe
        if (!await _repo.DoctorExistsAsync(req.DoctorId, ct))
            throw new NotFoundException("El doctor no existe.");

        var start = req.ScheduledAtUtc;
        var end = start.AddMinutes(req.DurationMinutes);

        // Choque por paciente
        if (await _repo.HasOverlapAsync(req.PatientId, start, end, ct))
            throw new ConflictException("El paciente ya tiene una cita en ese horario.");

        // Choque por doctor
        if (await _repo.HasDoctorOverlapAsync(req.DoctorId, start, end, ct))
            throw new ConflictException("El doctor ya tiene una cita en ese horario.");

        var appt = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = req.PatientId,
            DoctorId = req.DoctorId,
            ScheduledAtUtc = req.ScheduledAtUtc,
            DurationMinutes = req.DurationMinutes,
            Reason = req.Reason?.Trim(),
            Status = AppointmentStatus.Scheduled,   //  enum correcto
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(appt, ct);
        await _repo.SaveChangesAsync(ct);

        return new AppointmentResponse
        {
            Id = appt.Id,
            PatientId = appt.PatientId,
            DoctorId = appt.DoctorId,
            ScheduledAtUtc = appt.ScheduledAtUtc,
            DurationMinutes = appt.DurationMinutes,
            Reason = appt.Reason,
            Status = appt.Status,                  //  enum
            CreatedAtUtc = appt.CreatedAtUtc
        };
    }
}
