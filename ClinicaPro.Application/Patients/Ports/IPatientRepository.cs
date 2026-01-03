using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.Patients.Ports;

public interface IPatientRepository
{
    Task<List<Patient>> GetAllAsync(CancellationToken ct = default);
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Patient patient, CancellationToken ct = default);

    void Remove(Patient patient);

    Task SaveChangesAsync(CancellationToken ct = default);
}
