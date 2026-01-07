using ClinicaPro.Domain.Enums;

namespace ClinicaPro.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }

    public Guid DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; }

    public string? Reason { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public DateTime CreatedAtUtc { get; set; }
}
