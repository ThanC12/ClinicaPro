using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.Ports;

namespace ClinicaPro.Application.Appointments.UseCases;

public class GetAgendaByDateUseCase
{
    private readonly IAppointmentRepository _repo;

    public GetAgendaByDateUseCase(IAppointmentRepository repo) => _repo = repo;

    public async Task<List<AppointmentResponse>> ExecuteAsync(DateOnly date, CancellationToken ct = default)
    {
        var list = await _repo.GetByDateAsync(date, ct);

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
