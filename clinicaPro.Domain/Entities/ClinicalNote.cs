using System.Dynamic;

namespace ClinicaPro.Domain.Entities;

public class ClinicalNote

{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
    public DateTime NoteDateUtc { get; set; }

    public string Reason { get; set; } = default!; // consulta y motivos para el pacientes
    public string? Symptoms { get; set; }          // Sintomas del pacientes
    public string? Diagnosis { get; set; }          // Diagnóstico para el paciente
    public string? Treatment { get; set; }          // Tratamiento para el pacientes
    public string? Observations { get; set; }       // Observaciones adicionales del profesional de salud (opcional)

    public string? CreatedBy { get; set; }     // Nombre o identificador del médico/profesional que creó la nota
    public DateTime CreatedAtUtc { get; set; }      // Fecha y hora en UTC en la que la nota fue registrada en el sistema              






}