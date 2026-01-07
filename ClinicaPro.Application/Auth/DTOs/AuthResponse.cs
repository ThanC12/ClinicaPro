namespace ClinicaPro.Application.Auth.DTOs;

public class AuthResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }
}
