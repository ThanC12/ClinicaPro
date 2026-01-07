using ClinicaPro.Application.Auth.DTOs;
using ClinicaPro.Application.Auth.Ports;
using ClinicaPro.Domain.Entities;
using ClinicaPro.Domain.Enums;

public class RegisterUserUseCase
{
    private readonly IUserRepository _repo;

    public RegisterUserUseCase(IUserRepository repo) => _repo = repo;

    public async Task<(Guid userId, string email, string role)> ExecuteAsync(
        RegisterRequest req,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.Email))
            throw new ArgumentException("Email requerido");

        if (string.IsNullOrWhiteSpace(req.Password))
            throw new ArgumentException("Password requerido");

        var existing = await _repo.GetByEmailAsync(req.Email.Trim().ToLower(), ct);
        if (existing is not null)
            throw new InvalidOperationException("Ya existe un usuario con ese email");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email.Trim().ToLower(),
            PasswordHash = HashPassword(req.Password),
            Role = req.Role,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(user, ct);
        await _repo.SaveChangesAsync(ct);

        return (user.Id, user.Email, user.Role.ToString());
    }

    private static string HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
