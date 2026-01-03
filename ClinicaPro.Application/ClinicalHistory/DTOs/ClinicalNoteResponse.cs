namespace ClinicaPro.Application.ClinicalHistory.DTOs;

public class ClinicalNoteResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public DateTime NoteDateUtc { get; set; }

    public string Reason { get; set; } = default!;
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Observations { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
