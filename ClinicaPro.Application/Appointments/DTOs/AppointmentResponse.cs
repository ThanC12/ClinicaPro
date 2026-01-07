using ClinicaPro.Domain.Enums;

namespace ClinicaPro.Application.Appointments.DTOs;

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }          //  NUEVO
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string? Reason { get; set; }
    public AppointmentStatus Status { get; set; }   //  enum
    public DateTime CreatedAtUtc { get; set; }
}
