using PatitasCalchaquies.Infrastructure.Extensions;
using PatitasCalchaquies.API.AutoMapper;
using FluentValidation;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.API.Validators;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias de AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappeoObjeto>());

// Servicios
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// El puente a Infrastructure (MySQL)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

//Validadores
builder.Services.AddScoped<IValidator<PersonaCrearDto>, PersonaCreateValidator>();
builder.Services.AddScoped<IValidator<MedicamentoCrearDto>, MedicamentoCreateValidator>();
builder.Services.AddScoped<IValidator<VacunaCrearDto>, VacunaCreateValidator>();
builder.Services.AddScoped<IValidator<RazaCrearDto>, RazaCreateValidator>();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();