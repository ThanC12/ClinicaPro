using ClinicaPro.Application.Appointments.DTOs;
using ClinicaPro.Application.Appointments.UseCases;
using Microsoft.AspNetCore.Mvc;
using ClinicaPro.Application.Common;

namespace ClinicaPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly CreateAppointmentUseCase _create;
    private readonly GetAllAppointmentsUseCase _getAll;
    private readonly GetAppointmentByIdUseCase _getById;
    private readonly UpdateAppointmentUseCase _update;
    private readonly DeleteAppointmentUseCase _delete;
    private readonly GetAgendaByDateUseCase _agenda;


    public AppointmentsController(
        CreateAppointmentUseCase create,
        GetAllAppointmentsUseCase getAll,
        GetAppointmentByIdUseCase getById,
        UpdateAppointmentUseCase update,
        DeleteAppointmentUseCase delete,
        GetAgendaByDateUseCase agenda)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _delete = delete;
        _agenda = agenda;

    }
    [HttpGet("agenda")]
    public async Task<IActionResult> GetAgenda([FromQuery] DateOnly date, CancellationToken ct)
        => Ok(await _agenda.ExecuteAsync(date, ct));


    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _getAll.ExecuteAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var appt = await _getById.ExecuteAsync(id, ct);
        return appt is null ? NotFound() : Ok(appt);
    }
    

      [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAppointmentRequest req,
        CancellationToken ct)
    {
        try
        {
            var created = await _create.ExecuteAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            // Choque de agenda / reglas de negocio
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            // Validaciones de entrada
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentRequest req, CancellationToken ct)
    {
        var ok = await _update.ExecuteAsync(id, req, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _delete.ExecuteAsync(id, ct);
        return ok ? NoContent() : NotFound();
        
    }
}
