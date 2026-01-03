using ClinicaPro.Application.Patients.DTOs;
using ClinicaPro.Application.Patients.Ports;

namespace ClinicaPro.Application.Patients.UseCases;

public class GetPatientByIdUseCase
{
    private readonly IPatientRepository _repo;
    public GetPatientByIdUseCase(IPatientRepository repo) => _repo = repo;

    public async Task<PatientResponse?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return null;

        return new PatientResponse
        {
            Id = p.Id,
            Identification = p.Identification,
            FullName = p.FullName,
            BirthDate = p.BirthDate,
            Phone = p.Phone,
            Address = p.Address,
            Allergies = p.Allergies,
            IsActive = p.IsActive,
            CreatedAtUtc = p.CreatedAtUtc
        };
    }
}
