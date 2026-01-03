using ClinicaPro.Application.ClinicalHistory.DTOs;
using ClinicaPro.Application.ClinicalHistory.Ports;

namespace ClinicaPro.Application.ClinicalHistory.UseCases;

public class GetClinicalNoteByIdUseCase
{
    private readonly IClinicalHistoryRepository _repo;

    public GetClinicalNoteByIdUseCase(IClinicalHistoryRepository repo) => _repo = repo;

    public async Task<ClinicalNoteResponse?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var note = await _repo.GetByIdAsync(id, ct);
        if (note is null) return null;

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
