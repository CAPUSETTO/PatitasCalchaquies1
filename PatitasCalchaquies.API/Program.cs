using PatitasCalchaquies.Infrastructure.Extensions;
using PatitasCalchaquies.API.AutoMapper;
using FluentValidation;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.API.Validadores;
using PatitasCalchaquies.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias de AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappeoObjeto>());

// Servicios
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// El puente a Infrastructure (MySQL)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

// Validadores
builder.Services.AddScoped<IValidator<PersonaCrearDto>, PersonaCreateValidator>();
builder.Services.AddScoped<IValidator<PersonaActualizarDto>, PersonaUpdateValidator>();
builder.Services.AddScoped<IValidator<MedicamentoCrearDto>, MedicamentoCreateValidator>();
builder.Services.AddScoped<IValidator<VacunaCrearDto>, VacunaCreateValidator>();
builder.Services.AddScoped<IValidator<RazaCrearDto>, RazaCreateValidator>();
builder.Services.AddScoped<IValidator<AnimalCrearDto>, AnimalCreateValidator>();
builder.Services.AddScoped<IValidator<AnimalActualizarDto>, AnimalUpdateValidator>();
builder.Services.AddScoped<IValidator<AdopcionCrearDto>, AdopcionCreateValidator>();
builder.Services.AddScoped<IValidator<AdopcionActualizarDto>, AdopcionUpdateValidator>();
builder.Services.AddScoped<IValidator<AlimentacionCrearDto>, AlimentacionCreateValidator>();
builder.Services.AddScoped<IValidator<AlimentacionActualizarDto>, AlimentacionUpdateValidator>();
builder.Services.AddScoped<IValidator<AlojamientoAnimalCrearDto>, AlojamientoAnimalCreateValidator>();
builder.Services.AddScoped<IValidator<AlojamientoAnimalActualizarDto>, AlojamientoAnimalUpdateValidator>();
builder.Services.AddScoped<IValidator<AtencionVeterinariaCrearDto>, AtencionVeterinarioCreateValidator>();
builder.Services.AddScoped<IValidator<AtencionVeterinarioActualizarDto>, AtencionVeterinarioUpdateValidator>();
builder.Services.AddScoped<IValidator<DevolucionAdopcionCrearDto>, DevolucionAdopcionCreateValidator>();
builder.Services.AddScoped<IValidator<DevolucionAdopcionActualizarDto>, DevolucionAdopcionUpdateValidator>();
builder.Services.AddScoped<IValidator<EspecieCrearDto>, EspecieCreateValidator>();
builder.Services.AddScoped<IValidator<EspecieActualizarDto>, EspecieUpdateValidator>();
builder.Services.AddScoped<IValidator<EstadoAnimalCrearDto>, EstadoAnimalCreateValidator>();
builder.Services.AddScoped<IValidator<EstadoAnimalActualizarDto>, EstadoAnimalUpdateValidator>();
builder.Services.AddScoped<IValidator<EsterilizacionCrearDto>, EsterilizacionCreateValidator>();
builder.Services.AddScoped<IValidator<EsterilizacionActualizarDto>, EsterilizacionUpdateValidator>();
builder.Services.AddScoped<IValidator<IngresoAnimalCrearDto>, IngresoAnimalCreateValidator>();
builder.Services.AddScoped<IValidator<IngresoAnimalActualizarDto>, IngresoAnimalUpdateValidator>();
builder.Services.AddScoped<IValidator<FotoAnimalCrearDto>, FotoAnimalCreateValidator>();
builder.Services.AddScoped<IValidator<FotoAnimalActualizarDto>, FotoAnimalUpdateValidator>();
builder.Services.AddScoped<IValidator<PlanAlimentacionCrearDto>, PlanAlimentacionCreateValidator>();
builder.Services.AddScoped<IValidator<PlanAlimentacionActualizarDto>, PlanAlimentacionUpdateValidator>();
builder.Services.AddScoped<IValidator<PredioCrearDto>, PredioCreateValidator>();
builder.Services.AddScoped<IValidator<PredioActualizarDto>, PredioUpdateValidator>();
builder.Services.AddScoped<IValidator<RolCrearDto>, RolCreateValidator>();
builder.Services.AddScoped<IValidator<RolActualizarDto>, RolUpdateValidator>();
builder.Services.AddScoped<IValidator<SectorCrearDto>, SectorCreateValidator>();
builder.Services.AddScoped<IValidator<SectorActualizarDto>, SectorUpdateValidator>();
builder.Services.AddScoped<IValidator<SeguimientoAdopcionCrearDto>, SeguimientoAdopcionCreateValidator>();
builder.Services.AddScoped<IValidator<SeguimientoAdopcionActualizarDto>, SeguimientoAdopcionUpdateValidator>();
builder.Services.AddScoped<IValidator<SexoCrearDto>, SexoCreateValidator>();
builder.Services.AddScoped<IValidator<SexoActualizarDto>, SexoUpdateValidator>();
builder.Services.AddScoped<IValidator<SolicitudAdopcionCrearDto>, SolicitudAdopcionCreateValidator>();
builder.Services.AddScoped<IValidator<SolicitudAdopcionActualizarDto>, SolicitudAdopcionUpdateValidator>();
builder.Services.AddScoped<IValidator<TratamientoCrearDto>, TratamientoCreateValidator>();
builder.Services.AddScoped<IValidator<TratamientoActualizarDto>, TratamientoUpdateValidator>();
builder.Services.AddScoped<IValidator<TratamientoMedicamentoCrearDto>, TratamientoMedicamentoCreateValidator>();
builder.Services.AddScoped<IValidator<TratamientoMedicamentoActualizarDto>, TratamientoMedicamentoUpdateValidator>();
builder.Services.AddScoped<IValidator<UsuarioRolCrearDto>, UsuarioRolCreateValidator>();
builder.Services.AddScoped<IValidator<UsuarioCrearDto>, UsuarioCreateValidator>();
builder.Services.AddScoped<IValidator<UsuarioActualizarDto>, UsuarioUpdateValidator>();
builder.Services.AddScoped<IValidator<VacunacionCrearDto>, VacunacionCreateValidator>();
builder.Services.AddScoped<IValidator<VacunacionActualizarDto>, VacunacionUpdateValidator>();
builder.Services.AddScoped<IValidator<VeterinarioCrearDto>, VeterinarioCreateValidator>();
builder.Services.AddScoped<IValidator<VeterinarioActualizarDto>, VeterinarioUpdateValidator>();

// Inyectar el servicio de autenticación
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionService>();

// Configurar JWT
var jwtKey = builder.Configuration["Jwt:llave_secreta"];
var jwtIssuer = builder.Configuration["Jwt:emisor"];
var jwtAudience = builder.Configuration["Jwt:audiencia"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
