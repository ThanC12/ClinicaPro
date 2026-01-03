using ClinicaPro.Application.Patients.DTOs;
using ClinicaPro.Application.Patients.Ports;

namespace ClinicaPro.Application.Patients.UseCases;

public class GetAllPatientsUseCase
{
    private readonly IPatientRepository _repo;
    public GetAllPatientsUseCase(IPatientRepository repo) => _repo = repo;

    public async Task<List<PatientResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);

        return list.Select(p => new PatientResponse
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
        }).ToList();
    }
}
