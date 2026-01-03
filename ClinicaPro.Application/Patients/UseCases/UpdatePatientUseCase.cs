using ClinicaPro.Application.Patients.DTOs;
using ClinicaPro.Application.Patients.Ports;

namespace ClinicaPro.Application.Patients.UseCases;

public class UpdatePatientUseCase
{
    private readonly IPatientRepository _repo;
    public UpdatePatientUseCase(IPatientRepository repo) => _repo = repo;

    public async Task<bool> ExecuteAsync(Guid id, UpdatePatientRequest req, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return false;

        if (string.IsNullOrWhiteSpace(req.Identification)) throw new ArgumentException("Identification es obligatorio");
        if (string.IsNullOrWhiteSpace(req.FullName)) throw new ArgumentException("FullName es obligatorio");

        p.Identification = req.Identification.Trim();
        p.FullName = req.FullName.Trim();
        p.BirthDate = req.BirthDate;
        p.Phone = req.Phone;
        p.Address = req.Address;
        p.Allergies = req.Allergies;
        p.IsActive = req.IsActive;

        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
