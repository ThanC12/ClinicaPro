using ClinicaPro.Application.Auth.DTOs;
using ClinicaPro.Application.Auth.Ports;
using System.Security.Cryptography;
using System.Text;

namespace ClinicaPro.Application.Auth.UseCases;

public class LoginUseCase
{
    private readonly IUserRepository _repo;

    public LoginUseCase(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<(Guid userId, string email, string role)> ExecuteAsync(
        LoginRequest req,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var email = req.Email.Trim().ToLowerInvariant();
        var user = await _repo.GetByEmailAsync(email, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Usuario inactivo");

        var hash = HashPassword(req.Password);
        if (user.PasswordHash != hash)
            throw new UnauthorizedAccessException("Credenciales inválidas");

          return (user.Id, user.Email, user.Role.ToString());
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
