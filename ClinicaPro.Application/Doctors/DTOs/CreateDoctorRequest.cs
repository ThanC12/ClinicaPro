namespace ClinicaPro.Application.Doctors.DTOs;

public class CreateDoctorRequest
{
    public string Identification { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string Specialty { get; set; } = default!;
    public string Role { get; set; } = "Doctor";
}
