using ClinicaPro.Application.ClinicalHistory.Ports;
using ClinicaPro.Domain.Entities;
using ClinicaPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Infrastructure.ClinicalHistory;

public class ClinicalHistoryRepository : IClinicalHistoryRepository
{
    private readonly AppDbContext _db;
    public ClinicalHistoryRepository(AppDbContext db) => _db = db;

    public Task<bool> PatientExistsAsync(Guid patientId, CancellationToken ct = default)
        => _db.Patients.AnyAsync(p => p.Id == patientId, ct);

    public Task AddAsync(ClinicalNote note, CancellationToken ct = default)
        => _db.ClinicalNotes.AddAsync(note, ct).AsTask();

    public Task<ClinicalNote?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.ClinicalNotes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<ClinicalNote>> GetByPatientAsync(Guid patientId, CancellationToken ct = default)
        => _db.ClinicalNotes.AsNoTracking()
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(x => x.NoteDateUtc)
            .ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
