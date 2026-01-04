public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }   //  NUEVO
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string? Reason { get; set; }
}
