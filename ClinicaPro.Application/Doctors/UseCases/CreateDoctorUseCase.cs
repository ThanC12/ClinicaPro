using ClinicaPro.Application.Common;
using ClinicaPro.Application.Doctors.DTOs;
using ClinicaPro.Application.Doctors.Ports;
using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.Doctors.UseCases;

public class CreateDoctorUseCase
{
    private readonly IDoctorRepository _repo;
    public CreateDoctorUseCase(IDoctorRepository repo) => _repo = repo;

    public async Task<DoctorResponse> ExecuteAsync(CreateDoctorRequest req, CancellationToken ct = default)
    {
        // regla simple (luego mejoramos)
        var existing = await _repo.GetByEmailAsync(req.Email, ct);
        if (existing is not null)
            throw new ConflictException("Ya existe un doctor con ese email.");

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            Identification = req.Identification.Trim(),
            FullName = req.FullName.Trim(),
            Email = req.Email.Trim().ToLowerInvariant(),
            Phone = req.Phone?.Trim(),
            Specialty = req.Specialty.Trim(),
            Role = string.IsNullOrWhiteSpace(req.Role) ? "Doctor" : req.Role.Trim(),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(doctor, ct);
        await _repo.SaveChangesAsync(ct);

        return new DoctorResponse
        {
            Id = doctor.Id,
            Identification = doctor.Identification,
            FullName = doctor.FullName,
            Email = doctor.Email,
            Phone = doctor.Phone,
            Specialty = doctor.Specialty,
            Role = doctor.Role,
            IsActive = doctor.IsActive,
            CreatedAtUtc = doctor.CreatedAtUtc
        };
    }
}
