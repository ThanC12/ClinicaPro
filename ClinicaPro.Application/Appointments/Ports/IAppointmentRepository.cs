using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.Appointments.Ports;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetAllAsync(CancellationToken ct = default);
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Appointment appointment, CancellationToken ct = default);
    Task<bool> UpdateAsync(Appointment appointment, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);

    Task<bool> PatientExistsAsync(Guid patientId, CancellationToken ct = default);

    // Choque
    Task<bool> HasOverlapAsync(Guid patientId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);

    // Agenda por fecha (día)
    Task<List<Appointment>> GetByDateAsync(DateOnly date, CancellationToken ct = default);
    Task<bool> HasOverlapExcludingAsync(
    Guid appointmentId,
    Guid patientId,
    DateTime startUtc,
    DateTime endUtc,
    CancellationToken ct = default
);

}
