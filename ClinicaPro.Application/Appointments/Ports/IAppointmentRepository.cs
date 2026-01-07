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

    // Existencias
    Task<bool> PatientExistsAsync(Guid patientId, CancellationToken ct = default);
    Task<bool> DoctorExistsAsync(Guid doctorId, CancellationToken ct = default); // NUEVO

    // Choque por paciente
    Task<bool> HasOverlapAsync(Guid patientId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);
    Task<bool> HasOverlapExcludingAsync(Guid appointmentId, Guid patientId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);

    // Choque por doctor
    Task<bool> HasDoctorOverlapAsync(Guid doctorId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default); // NUEVO
    Task<bool> HasDoctorOverlapExcludingAsync(Guid appointmentId, Guid doctorId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);

    // Agenda por fecha (día)
    Task<List<Appointment>> GetByDateAsync(DateOnly date, CancellationToken ct = default);
}
