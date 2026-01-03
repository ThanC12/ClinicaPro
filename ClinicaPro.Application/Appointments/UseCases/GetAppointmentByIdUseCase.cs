using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.Ports;

namespace ClinicaPro.Application.Appointments.UseCases;

public class GetAppointmentByIdUseCase
{
    private readonly IAppointmentRepository _repo;
    public GetAppointmentByIdUseCase(IAppointmentRepository repo) => _repo = repo;

    public async Task<AppointmentResponse?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var a = await _repo.GetByIdAsync(id, ct);
        if (a is null) return null;

        return new AppointmentResponse
        {
            Id = a.Id,
            PatientId = a.PatientId,
            ScheduledAtUtc = a.ScheduledAtUtc,
            DurationMinutes = a.DurationMinutes,
            Reason = a.Reason,
            Status = a.Status,
            CreatedAtUtc = a.CreatedAtUtc
        };
    }
}
