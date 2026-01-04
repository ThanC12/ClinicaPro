using ClinicaPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using ClinicaPro.Application.Patients.Ports;
using ClinicaPro.Application.Patients.UseCases;
using ClinicaPro.Infrastructure.Patients;

using ClinicaPro.Application.Appointments.Ports;
using ClinicaPro.Application.Appointments.UseCases;
using ClinicaPro.Infrastructure.Appointments;

using ClinicaPro.Application.ClinicalHistory.Ports;
using ClinicaPro.Application.ClinicalHistory.UseCases;
using ClinicaPro.Infrastructure.ClinicalHistory;


using ClinicaPro.Application.Doctors.Ports;
using ClinicaPro.Application.Doctors.UseCases;
using ClinicaPro.Infrastructure.Doctors;



var builder = WebApplication.CreateBuilder(args);


// Doctors
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<CreateDoctorUseCase>();
builder.Services.AddScoped<GetAllDoctorsUseCase>();
builder.Services.AddScoped<GetDoctorByIdUseCase>();
builder.Services.AddScoped<UpdateDoctorUseCase>();
builder.Services.AddScoped<DeleteDoctorUseCase>();


// Clinical History (UseCases)
builder.Services.AddScoped<CreateClinicalNoteUseCase>();
builder.Services.AddScoped<GetClinicalNoteByIdUseCase>();
builder.Services.AddScoped<GetPatientNotesUseCase>();

// Clinical History (Repository)
builder.Services.AddScoped<IClinicalHistoryRepository, ClinicalHistoryRepository>();


//service
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<CreatePatientUseCase>();
builder.Services.AddScoped<GetAllPatientsUseCase>();
builder.Services.AddScoped<GetPatientByIdUseCase>();
builder.Services.AddScoped<UpdatePatientUseCase>();
builder.Services.AddScoped<DeletePatientUseCase>();

// Patients
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<CreatePatientUseCase>();
builder.Services.AddScoped<GetAllPatientsUseCase>();
builder.Services.AddScoped<GetPatientByIdUseCase>();
builder.Services.AddScoped<UpdatePatientUseCase>();
builder.Services.AddScoped<DeletePatientUseCase>();

// Appointments
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<CreateAppointmentUseCase>();
builder.Services.AddScoped<GetAllAppointmentsUseCase>();
builder.Services.AddScoped<GetAppointmentByIdUseCase>();
builder.Services.AddScoped<UpdateAppointmentUseCase>();
builder.Services.AddScoped<DeleteAppointmentUseCase>();
builder.Services.AddScoped<GetAgendaByDateUseCase>();


builder.Services.AddTransient<ClinicaPro.Api.Middlewares.ExceptionMiddleware>();



// Controllers
builder.Services.AddControllers();



// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ClinicaPro.Api.Middlewares.ExceptionMiddleware>();


app.MapControllers();

app.Run();
