using ClinicaPro.Application.Doctors.DTOs;
using ClinicaPro.Application.Doctors.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly CreateDoctorUseCase _create;
    private readonly GetAllDoctorsUseCase _getAll;
    private readonly GetDoctorByIdUseCase _getById;
    private readonly UpdateDoctorUseCase _update;
    private readonly DeleteDoctorUseCase _delete;

    public DoctorsController(
        CreateDoctorUseCase create,
        GetAllDoctorsUseCase getAll,
        GetDoctorByIdUseCase getById,
        UpdateDoctorUseCase update,
        DeleteDoctorUseCase delete)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _delete = delete;
    }

    //  SOLO Admin puede crear doctor
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDoctorRequest req, CancellationToken ct)
    {
        var result = await _create.ExecuteAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    //  Admin o Doctor puede ver lista (ajusta si quieres que solo Admin vea todo)
    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _getAll.ExecuteAsync(ct));

    //  Admin o Doctor puede ver por Id
    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var d = await _getById.ExecuteAsync(id, ct);
        return d is null ? NotFound() : Ok(d);
    }

    //  SOLO Admin puede actualizar doctor
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDoctorRequest req, CancellationToken ct)
    {
        var ok = await _update.ExecuteAsync(id, req, ct);
        return ok ? NoContent() : NotFound();
    }

    // SOLO Admin puede eliminar doctor
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _delete.ExecuteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
