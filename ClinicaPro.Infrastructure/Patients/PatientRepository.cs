// ClinicaPro.Infrastructure/Patients/PatientRepository.cs

using ClinicaPro.Application.Patients.Ports;
using ClinicaPro.Domain.Entities;
using ClinicaPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Infrastructure.Patients;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _db;

    public PatientRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Patient>> GetAllAsync(CancellationToken ct = default)
        => await _db.Patients.AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Patients.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(Patient patient, CancellationToken ct = default)
        => await _db.Patients.AddAsync(patient, ct);

    public void Remove(Patient patient)
        => _db.Patients.Remove(patient);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
