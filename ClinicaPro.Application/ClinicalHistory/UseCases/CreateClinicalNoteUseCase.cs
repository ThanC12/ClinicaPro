using ClinicaPro.Application.ClinicalHistory.DTOs;
using ClinicaPro.Application.ClinicalHistory.Ports;
using ClinicaPro.Application.Common;
using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.ClinicalHistory.UseCases;

public class CreateClinicalNoteUseCase
{
    private readonly IClinicalHistoryRepository _repo;

    public CreateClinicalNoteUseCase(IClinicalHistoryRepository repo) => _repo = repo;

    public async Task<ClinicalNoteResponse> ExecuteAsync(CreateClinicalNoteRequest req, CancellationToken ct = default)
    {
        if (req is null) throw new ArgumentNullException(nameof(req));
        if (req.PatientId == Guid.Empty) throw new ArgumentException("PatientId es obligatorio.");
        if (string.IsNullOrWhiteSpace(req.Reason)) throw new ArgumentException("Reason es obligatorio.");

        var exists = await _repo.PatientExistsAsync(req.PatientId, ct);
        if (!exists) throw new NotFoundException("El paciente no existe.");

        var note = new ClinicalNote
        {
            Id = Guid.NewGuid(),
            PatientId = req.PatientId,
            NoteDateUtc = req.NoteDateUtc == default ? DateTime.UtcNow : req.NoteDateUtc,
            Reason = req.Reason.Trim(),
            Symptoms = req.Symptoms,
            Diagnosis = req.Diagnosis,
            Treatment = req.Treatment,
            Observations = req.Observations,
            CreatedBy = req.CreatedBy,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(note, ct);
        await _repo.SaveChangesAsync(ct);

        return new ClinicalNoteResponse
        {
            Id = note.Id,
            PatientId = note.PatientId,
            NoteDateUtc = note.NoteDateUtc,
            Reason = note.Reason,
            Symptoms = note.Symptoms,
            Diagnosis = note.Diagnosis,
            Treatment = note.Treatment,
            Observations = note.Observations,
            CreatedBy = note.CreatedBy,
            CreatedAtUtc = note.CreatedAtUtc
        };
    }
}
