using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.Ports;

namespace ClinicaPro.Application.Appointments.UseCases;

public class GetAllAppointmentsUseCase
{
    private readonly IAppointmentRepository _repo;
    public GetAllAppointmentsUseCase(IAppointmentRepository repo) => _repo = repo;

    public async Task<List<AppointmentResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(a => new AppointmentResponse
        {
            Id = a.Id,
            PatientId = a.PatientId,
            ScheduledAtUtc = a.ScheduledAtUtc,
            DurationMinutes = a.DurationMinutes,
            Reason = a.Reason,
            Status = a.Status,
            CreatedAtUtc = a.CreatedAtUtc
        }).ToList();
    }
}
