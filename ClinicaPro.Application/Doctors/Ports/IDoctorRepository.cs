using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.Doctors.Ports;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync(CancellationToken ct = default);
    Task<Doctor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Doctor?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task AddAsync(Doctor doctor, CancellationToken ct = default);
    Task<bool> UpdateAsync(Doctor doctor, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
