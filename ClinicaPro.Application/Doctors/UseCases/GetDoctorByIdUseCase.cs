using ClinicaPro.Application.Doctors.DTOs;
using ClinicaPro.Application.Doctors.Ports;

namespace ClinicaPro.Application.Doctors.UseCases;

public class GetDoctorByIdUseCase
{
    private readonly IDoctorRepository _repo;
    public GetDoctorByIdUseCase(IDoctorRepository repo) => _repo = repo;

    public async Task<DoctorResponse?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var d = await _repo.GetByIdAsync(id, ct);
        if (d is null) return null;

        return new DoctorResponse
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
        };
    }
}
