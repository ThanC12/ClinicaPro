namespace ClinicaPro.Domain.Entities;

public class Patient
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Identification { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public DateTime? BirthDate { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Allergies { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
