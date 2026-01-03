namespace ClinicaPro.Application.Appointments.DTOs;

public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Reason { get; set; }
}
