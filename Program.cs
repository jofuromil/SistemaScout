using BackendScout.Data;
using BackendScout.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


// ✅ Activar licencias
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = "clave-secreta-super-segura-scout"; // Puedes reemplazarla luego por una más segura
builder.Configuration["JwtKey"] = jwtKey;

// ✅ Servicios
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // Permite (de)serializar enums por su nombre
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BackendScout",
        Version = "v1"
    });

    // 🔐 Configuración del esquema de seguridad para JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese su token JWT en este formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// ✅ Servicios personalizados
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UnidadService>();
builder.Services.AddScoped<FichaMedicaService>();
builder.Services.AddScoped<ObjetivoService>();
builder.Services.AddScoped<CargaObjetivosService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PdfObjetivosService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<MensajeService>();
builder.Services.AddScoped<EspecialidadService>();
builder.Services.AddScoped<EventoService>();
builder.Services.AddScoped<DocumentoEventoService>();


// ✅ Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ScoutDB.db"));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection(); // app.UseHttpsRedirection();

app.UseAuthentication();   // ✅ primero autenticación
app.UseAuthorization();    // ✅ luego autorización

app.MapControllers();

app.Run();