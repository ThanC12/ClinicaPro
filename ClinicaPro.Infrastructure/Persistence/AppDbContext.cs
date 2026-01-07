using ClinicaPro.Domain.Entities;
using ClinicaPro.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClinicaPro.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<ClinicalNote> ClinicalNotes => Set<ClinicalNote>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PATIENTS
       
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("patients");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Identification).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Identification).IsUnique();

            entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);

            entity.Property(e => e.BirthDate).HasColumnType("timestamp with time zone");

            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.Allergies).HasColumnType("text");

            entity.Property(e => e.IsActive).IsRequired();

            entity.Property(e => e.CreatedAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("NOW()");
        });

       
        // APPOINTMENTS   CORREGIDO
        
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("appointments");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.PatientId).IsRequired();
            entity.Property(x => x.DoctorId).IsRequired();

            entity.Property(x => x.ScheduledAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .IsRequired();

            entity.Property(x => x.DurationMinutes).IsRequired();

            entity.Property(x => x.Reason).HasColumnType("text");

            //  Status es enum → guardarlo como string para evitar migración a integer
            entity.Property(x => x.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("NOW()");

            //  FK: Appointment -> Patient (usa navegación)
            entity.HasOne(a => a.Patient)
                  .WithMany()
                  .HasForeignKey(a => a.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);

            //  FK: Appointment -> Doctor (usa navegación)
            entity.HasOne(a => a.Doctor)
                  .WithMany()
                  .HasForeignKey(a => a.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.ScheduledAtUtc);
            entity.HasIndex(x => new { x.DoctorId, x.ScheduledAtUtc });
            entity.HasIndex(x => new { x.PatientId, x.ScheduledAtUtc });
        });

        
        // CLINICAL NOTES
        
        modelBuilder.Entity<ClinicalNote>(entity =>
        {
            entity.ToTable("clinical_notes");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.PatientId).IsRequired();

            entity.Property(x => x.NoteDateUtc)
                  .HasColumnType("timestamp with time zone")
                  .IsRequired();

            entity.Property(x => x.Reason).IsRequired().HasColumnType("text");

            entity.Property(x => x.Symptoms).HasColumnType("text");
            entity.Property(x => x.Diagnosis).HasColumnType("text");
            entity.Property(x => x.Treatment).HasColumnType("text");
            entity.Property(x => x.Observations).HasColumnType("text");

            entity.Property(x => x.CreatedBy).HasMaxLength(120);

            entity.Property(x => x.CreatedAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("NOW()");

            entity.HasOne(x => x.Patient)
                  .WithMany()
                  .HasForeignKey(x => x.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.PatientId, x.NoteDateUtc });
        });

        
        // DOCTORS   AJUSTE RECOMENDADO
        
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("doctors");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.Identification).IsRequired().HasMaxLength(20);
            entity.HasIndex(x => x.Identification).IsUnique();

            entity.Property(x => x.FullName).IsRequired().HasMaxLength(150);

            entity.Property(x => x.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Phone).HasMaxLength(20);

            entity.Property(x => x.Specialty).IsRequired().HasMaxLength(120);

            entity.Property(x => x.CreatedAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("NOW()");

            //  Relación Doctor -> User
            entity.HasOne(d => d.User)
                  .WithMany()
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(d => d.UserId).IsUnique();
        });

        
        // USERS
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.PasswordHash).IsRequired().HasColumnType("text");

            entity.Property(x => x.Role)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(20);

            entity.Property(x => x.IsActive).IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("NOW()");
        });
    }
}
