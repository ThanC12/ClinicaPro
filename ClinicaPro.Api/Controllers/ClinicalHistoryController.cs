using ClinicaPro.Application.ClinicalHistory.DTOs;
using ClinicaPro.Application.ClinicalHistory.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClinicalHistoryController : ControllerBase
{
    private readonly CreateClinicalNoteUseCase _create;
    private readonly GetClinicalNoteByIdUseCase _getById;
    private readonly GetPatientNotesUseCase _getPatientNotes;

    public ClinicalHistoryController(
        CreateClinicalNoteUseCase create,
        GetClinicalNoteByIdUseCase getById,
        GetPatientNotesUseCase getPatientNotes)
    {
        _create = create;
        _getById = getById;
        _getPatientNotes = getPatientNotes;
    }

    [HttpPost("notes")]
    public async Task<IActionResult> CreateNote([FromBody] CreateClinicalNoteRequest req, CancellationToken ct)
    {
        var result = await _create.ExecuteAsync(req, ct);
        return CreatedAtAction(nameof(GetNoteById), new { id = result.Id }, result);
    }

    [HttpGet("notes/{id:guid}")]
    public async Task<IActionResult> GetNoteById(Guid id, CancellationToken ct)
    {
        var note = await _getById.ExecuteAsync(id, ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpGet("patients/{patientId:guid}/notes")]
    public async Task<IActionResult> GetPatientNotes(Guid patientId, CancellationToken ct)
    {
        var notes = await _getPatientNotes.ExecuteAsync(patientId, ct);
        return Ok(notes);
    }
}
