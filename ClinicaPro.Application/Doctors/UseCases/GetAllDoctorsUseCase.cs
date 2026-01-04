using ClinicaPro.Application.Doctors.DTOs;
using ClinicaPro.Application.Doctors.Ports;

namespace ClinicaPro.Application.Doctors.UseCases;

public class GetAllDoctorsUseCase
{
    private readonly IDoctorRepository _repo;
    public GetAllDoctorsUseCase(IDoctorRepository repo) => _repo = repo;

    public async Task<List<DoctorResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(d => new DoctorResponse
        {
            Id = d.Id,
            Identification = d.Identification,
            FullName = d.FullName,
            Email = d.Email,
            Phone = d.Phone,
            Specialty = d.Specialty,
            Role = d.Role,
            IsActive = d.IsActive,
            CreatedAtUtc = d.CreatedAtUtc
        }).ToList();
    }
}
