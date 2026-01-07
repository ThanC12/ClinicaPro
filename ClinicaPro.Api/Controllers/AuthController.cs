using ClinicaPro.Api.Security;
using ClinicaPro.Application.Auth.DTOs;
using ClinicaPro.Application.Auth.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _register;
    private readonly LoginUseCase _login;
    private readonly JwtTokenService _jwt;

    public AuthController(
        RegisterUserUseCase register,
        LoginUseCase login,
        JwtTokenService jwt)
    {
        _register = register;
        _login = login;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterRequest req,
        CancellationToken ct)
    {
        var (userId, email, role) = await _register.ExecuteAsync(req, ct);
        var (token, expires) = _jwt.CreateToken(userId, email, role);

        return Ok(new AuthResponse
        {
            UserId = userId,
            Email = email,
            Role = role,
            Token = token,
            ExpiresAtUtc = expires
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody] LoginRequest req,
        CancellationToken ct)
    {
        var (userId, email, role) = await _login.ExecuteAsync(req, ct);
        var (token, expires) = _jwt.CreateToken(userId, email, role);

        return Ok(new AuthResponse
        {
            UserId = userId,
            Email = email,
            Role = role,
            Token = token,
            ExpiresAtUtc = expires
        });
    }
}
