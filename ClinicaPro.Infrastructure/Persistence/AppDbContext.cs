using ClinicaPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets (Tablas)
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<ClinicalNote> ClinicalNotes => Set<ClinicalNote>();
    public DbSet<Doctor> Doctors => Set<Doctor>();


    // Configuración del modelo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // PATIENTS
        // =========================
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("patients");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Identification)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(e => e.Identification).IsUnique();

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp with time zone");

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.Address)
                .HasColumnType("text");

            entity.Property(e => e.Allergies)
                .HasColumnType("text");

            entity.Property(e => e.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedAtUtc)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("NOW()");
        });

        // =========================
        // APPOINTMENTS
        // =========================
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("appointments");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.PatientId).IsRequired();

            entity.Property(x => x.ScheduledAtUtc)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            entity.Property(x => x.DurationMinutes)
                .IsRequired();

            entity.Property(x => x.Reason)
                .HasColumnType("text");

            entity.Property(x => x.Status)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("NOW()");

            // FK: Appointment -> Patient
            entity.HasOne<Patient>()
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice útil para agenda
            entity.HasIndex(x => x.ScheduledAtUtc);
        });

        // =========================
        // CLINICAL NOTES
        // =========================
        modelBuilder.Entity<ClinicalNote>(entity =>
        {
            entity.ToTable("clinical_notes");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.PatientId).IsRequired();

            entity.Property(x => x.NoteDateUtc)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            entity.Property(x => x.Reason)
                .IsRequired()
                .HasColumnType("text"); // mejor que MaxLength si quieres texto libre

            entity.Property(x => x.Symptoms).HasColumnType("text");
            entity.Property(x => x.Diagnosis).HasColumnType("text");
            entity.Property(x => x.Treatment).HasColumnType("text");
            entity.Property(x => x.Observations).HasColumnType("text");

            entity.Property(x => x.CreatedBy).HasMaxLength(120);

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("NOW()");

            // FK: ClinicalNote -> Patient
            entity.HasOne(x => x.Patient)
                .WithMany() // si Patient no tiene colección aún
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice útil para historial por paciente/fecha
            entity.HasIndex(x => new { x.PatientId, x.NoteDateUtc });

        });
        // DOCTORS
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("doctors");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.Identification)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasIndex(x => x.Identification).IsUnique();

            entity.Property(x => x.FullName)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Phone)
                  .HasMaxLength(20);

            entity.Property(x => x.Specialty)
                  .IsRequired()
                  .HasMaxLength(120);

            entity.Property(x => x.Role)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(x => x.IsActive)
                  .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("NOW()");
        });

    }
}
