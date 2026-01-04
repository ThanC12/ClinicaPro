namespace ClinicaPro.Application.Doctors.DTOs;

public class UpdateDoctorRequest
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string Specialty { get; set; } = default!;
    public string Role { get; set; } = "Doctor";
    public bool IsActive { get; set; } = true;
}
