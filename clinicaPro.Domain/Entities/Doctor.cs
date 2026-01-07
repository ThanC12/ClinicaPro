namespace ClinicaPro.Domain.Entities;

public class Doctor
{
    public Guid Id { get; set; }

    public string Identification { get; set; } = default!;   // cédula / DNI
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }

    public string Specialty { get; set; } = default!;         // Medicina general, Pediatría...
    public string Role { get; set; } = "Doctor";              // Admin | Doctor | Nurse (por ahora string)

    public bool IsActive { get; set; } = true;

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
