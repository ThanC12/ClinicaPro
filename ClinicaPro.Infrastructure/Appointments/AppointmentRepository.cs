using ClinicaPro.Application.Appointments.Ports;
using ClinicaPro.Domain.Entities;
using ClinicaPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Infrastructure.Appointments;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db) => _db = db;

    public async Task<List<Appointment>> GetAllAsync(CancellationToken ct = default)
        => await _db.Appointments
            .AsNoTracking()
            .OrderByDescending(x => x.ScheduledAtUtc)
            .ToListAsync(ct);

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(Appointment appointment, CancellationToken ct = default)
        => await _db.Appointments.AddAsync(appointment, ct);

    public Task<bool> UpdateAsync(Appointment appointment, CancellationToken ct = default)
    {
        _db.Appointments.Update(appointment);
        return Task.FromResult(true);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var appt = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (appt is null) return false;

        _db.Appointments.Remove(appt);
        return true;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public Task<bool> PatientExistsAsync(Guid patientId, CancellationToken ct = default)
        => _db.Patients.AnyAsync(p => p.Id == patientId, ct);

    public Task<List<Appointment>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
    {
        // Agenda por día (UTC): [start, nextDay)
        var start = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end = date.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

        return _db.Appointments
            .AsNoTracking()
            .Where(a => a.ScheduledAtUtc >= start && a.ScheduledAtUtc < end)
            .OrderBy(a => a.ScheduledAtUtc)
            .ToListAsync(ct);
    }

    // ✅ Choque para CREATE (sin excluir)
    public Task<bool> HasOverlapAsync(Guid patientId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default)
    {
        return _db.Appointments
            .AsNoTracking()
            .AnyAsync(a =>
                a.PatientId == patientId &&
                a.Status == "Scheduled" &&
                a.ScheduledAtUtc < endUtc &&
                a.ScheduledAtUtc.AddMinutes(a.DurationMinutes) > startUtc,
                ct
            );
    }

    // ✅ Choque para UPDATE (excluye la misma cita)
    public Task<bool> HasOverlapExcludingAsync(
        Guid appointmentId,
        Guid patientId,
        DateTime startUtc,
        DateTime endUtc,
        CancellationToken ct = default)
    {
        return _db.Appointments
            .AsNoTracking()
            .AnyAsync(a =>
                a.Id != appointmentId &&
                a.PatientId == patientId &&
                a.Status == "Scheduled" &&
                a.ScheduledAtUtc < endUtc &&
                a.ScheduledAtUtc.AddMinutes(a.DurationMinutes) > startUtc,
                ct
            );
    }
}
