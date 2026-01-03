using ClinicaPro.Application.Appointments.Ports;

namespace ClinicaPro.Application.Appointments.UseCases;

public class DeleteAppointmentUseCase
{
    private readonly IAppointmentRepository _repo;

    public DeleteAppointmentUseCase(IAppointmentRepository repo) => _repo = repo;

    public async Task<bool> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var ok = await _repo.DeleteAsync(id, ct);
        if (!ok) return false;

        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
