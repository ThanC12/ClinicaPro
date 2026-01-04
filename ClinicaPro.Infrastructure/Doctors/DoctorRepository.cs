using ClinicaPro.Application.Doctors.Ports;
using ClinicaPro.Domain.Entities;
using ClinicaPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Infrastructure.Doctors;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _db;
    public DoctorRepository(AppDbContext db) => _db = db;

    public Task<List<Doctor>> GetAllAsync(CancellationToken ct = default)
        => _db.Doctors.AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(ct);

    public Task<Doctor?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Doctors.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Doctor?> GetByEmailAsync(string email, CancellationToken ct = default)
        => _db.Doctors.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, ct);

    public Task AddAsync(Doctor doctor, CancellationToken ct = default)
        => _db.Doctors.AddAsync(doctor, ct).AsTask();

    public Task<bool> UpdateAsync(Doctor doctor, CancellationToken ct = default)
    {
        _db.Doctors.Update(doctor);
        return Task.FromResult(true);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var d = await _db.Doctors.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (d is null) return false;

        _db.Doctors.Remove(d);
        return true;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
