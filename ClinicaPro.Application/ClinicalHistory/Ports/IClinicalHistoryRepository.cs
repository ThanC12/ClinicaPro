using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.ClinicalHistory.Ports;

public interface IClinicalHistoryRepository
{
    Task<bool> PatientExistsAsync(Guid patientId, CancellationToken ct = default);

    Task AddAsync(ClinicalNote note, CancellationToken ct = default);
    Task<ClinicalNote?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<ClinicalNote>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
