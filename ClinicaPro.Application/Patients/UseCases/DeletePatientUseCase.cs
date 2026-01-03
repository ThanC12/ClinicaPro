using ClinicaPro.Application.Patients.Ports;

namespace ClinicaPro.Application.Patients.UseCases;

public class DeletePatientUseCase
{
    private readonly IPatientRepository _repo;
    public DeletePatientUseCase(IPatientRepository repo) => _repo = repo;

    public async Task<bool> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return false;

        _repo.Remove(p);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
