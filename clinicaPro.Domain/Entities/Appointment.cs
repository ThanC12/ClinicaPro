namespace ClinicaPro.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PatientId { get; set; }

    public DateTime ScheduledAtUtc { get; set; }  // fecha/hora cita (UTC)

    public int DurationMinutes { get; set; } = 30;

    public string? Reason { get; set; }          // motivo
    public string Status { get; set; } = "Scheduled"; // Scheduled | Cancelled | Completed

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
