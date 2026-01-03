namespace ClinicaPro.Application.Appointments.DTOs;

public class UpdateAppointmentRequest
{
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Reason { get; set; }
    public string Status { get; set; } = "Scheduled";
}
