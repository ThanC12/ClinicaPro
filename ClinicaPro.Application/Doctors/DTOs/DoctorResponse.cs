namespace ClinicaPro.Application.Doctors.DTOs;

public class DoctorResponse
{
    
    public Guid Id { get; set; }
    public string Identification { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string Specialty { get; set; } = default!;
    public string Role { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
