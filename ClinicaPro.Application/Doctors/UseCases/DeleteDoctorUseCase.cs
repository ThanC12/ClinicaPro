using ClinicaPro.Application.Doctors.Ports;

namespace ClinicaPro.Application.Doctors.UseCases;

public class DeleteDoctorUseCase
{
    private readonly IDoctorRepository _repo;
    public DeleteDoctorUseCase(IDoctorRepository repo) => _repo = repo;

    public async Task<bool> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var ok = await _repo.DeleteAsync(id, ct);
        if (!ok) return false;

        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
