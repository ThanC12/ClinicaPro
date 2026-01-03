namespace ClinicaPro.Application.Patients.DTOs;

public class PatientResponse
{
    public Guid Id { get; set; }
    public string Identification { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateTime? BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Allergies { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
