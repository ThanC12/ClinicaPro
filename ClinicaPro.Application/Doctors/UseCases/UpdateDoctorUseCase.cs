using ClinicaPro.Application.Common;
using ClinicaPro.Application.Doctors.DTOs;
using ClinicaPro.Application.Doctors.Ports;

namespace ClinicaPro.Application.Doctors.UseCases;

public class UpdateDoctorUseCase
{
    private readonly IDoctorRepository _repo;
    public UpdateDoctorUseCase(IDoctorRepository repo) => _repo = repo;

    public async Task<bool> ExecuteAsync(Guid id, UpdateDoctorRequest req, CancellationToken ct = default)
    {
        var d = await _repo.GetByIdAsync(id, ct);
        if (d is null) return false;

        // OJO: GetByIdAsync está AsNoTracking, así que creamos objeto “nuevo” con mismos Id.
        var updated = new ClinicaPro.Domain.Entities.Doctor
        {
            Id = d.Id,
            Identification = d.Identification,
            FullName = req.FullName.Trim(),
            Email = req.Email.Trim().ToLowerInvariant(),
            Phone = req.Phone?.Trim(),
            Specialty = req.Specialty.Trim(),
            Role = req.Role.Trim(),
            IsActive = req.IsActive,
            CreatedAtUtc = d.CreatedAtUtc
        };

        // Validación email único
        var other = await _repo.GetByEmailAsync(updated.Email, ct);
        if (other is not null && other.Id != id)
            throw new ConflictException("Ya existe otro doctor con ese email.");

        await _repo.UpdateAsync(updated, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
