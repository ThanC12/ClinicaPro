public class UpdateAppointmentRequest
{
    public Guid DoctorId { get; set; }   //  NUEVO
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "Scheduled";
}
