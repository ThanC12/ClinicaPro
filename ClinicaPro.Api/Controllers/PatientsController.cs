using ClinicaPro.Domain.Entities;
using ClinicaPro.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PatientsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Patients.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).ToListAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var patient = await _db.Patients.FindAsync(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient patient)
    {
        patient.Id = Guid.NewGuid();
        patient.CreatedAtUtc = DateTime.UtcNow;

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Patient input)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient is null) return NotFound();

        patient.Identification = input.Identification;
        patient.FullName = input.FullName;
        patient.BirthDate = input.BirthDate;
        patient.Phone = input.Phone;
        patient.Address = input.Address;
        patient.Allergies = input.Allergies;
        patient.IsActive = input.IsActive;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient is null) return NotFound();

        _db.Patients.Remove(patient);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
