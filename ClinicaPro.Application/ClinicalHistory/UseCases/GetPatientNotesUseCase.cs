using ClinicaPro.Application.ClinicalHistory.DTOs;
using ClinicaPro.Application.ClinicalHistory.Ports;

namespace ClinicaPro.Application.ClinicalHistory.UseCases;

public class GetPatientNotesUseCase
{
    private readonly IClinicalHistoryRepository _repo;

    public GetPatientNotesUseCase(IClinicalHistoryRepository repo) => _repo = repo;

    public async Task<List<ClinicalNoteResponse>> ExecuteAsync(Guid patientId, CancellationToken ct = default)
    {
        var notes = await _repo.GetByPatientAsync(patientId, ct);

        return notes.Select(n => new ClinicalNoteResponse
        {
            Id = n.Id,
            PatientId = n.PatientId,
            NoteDateUtc = n.NoteDateUtc,
            Reason = n.Reason,
            Symptoms = n.Symptoms,
            Diagnosis = n.Diagnosis,
            Treatment = n.Treatment,
            Observations = n.Observations,
            CreatedBy = n.CreatedBy,
            CreatedAtUtc = n.CreatedAtUtc
        }).ToList();
    }
}
