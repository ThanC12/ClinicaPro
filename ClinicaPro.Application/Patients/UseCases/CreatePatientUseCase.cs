using ClinicaPro.Application.Patients.DTOs;
using ClinicaPro.Application.Patients.Ports;
using ClinicaPro.Domain.Entities;

namespace ClinicaPro.Application.Patients.UseCases;

public class CreatePatientUseCase
{
    private readonly IPatientRepository _repo;

    public CreatePatientUseCase(IPatientRepository repo) => _repo = repo;

    public async Task<PatientResponse> ExecuteAsync(CreatePatientRequest request, CancellationToken ct = default)
    {
        // validación simple
        if (string.IsNullOrWhiteSpace(request.Identification))
            throw new ArgumentException("Identification es obligatorio");

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("FullName es obligatorio");

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            Identification = request.Identification.Trim(),
            FullName = request.FullName.Trim(),
            BirthDate = request.BirthDate,
            Phone = request.Phone,
            Address = request.Address,
            Allergies = request.Allergies,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(patient, ct);
        await _repo.SaveChangesAsync(ct);

        return new PatientResponse
        {
            Id = patient.Id,
            Identification = patient.Identification,
            FullName = patient.FullName,
            BirthDate = patient.BirthDate,
            Phone = patient.Phone,
            Address = patient.Address,
            Allergies = patient.Allergies,
            IsActive = patient.IsActive,
            CreatedAtUtc = patient.CreatedAtUtc
        };
    }
}
