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
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;
using Npgsql.EntityFrameworkCore.PostgreSQL;

// ✅ Activar licencia QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// ✅ Clave JWT desde appsettings.json o valor por defecto
var jwtKey = builder.Configuration["Jwt:Key"] ?? "clave-secreta-super-segura-scout";

// ✅ CORS para React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Servicios base
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddControllersWithViews();
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
builder.Services.AddTransient<EspecialidadImporter>();
builder.Services.AddScoped<EventoService>();
builder.Services.AddScoped<DocumentoEventoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordResetService>();

// ✅ Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Configuración JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = ClaimTypes.Role
        };
    });

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// 🔴 COMENTADO: para evitar error si no hay base de datos
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     context.LimpiarRequisitosInvalidos(); // método temporal
// }

// 🔴 COMENTADO: migraciones automáticas y limpieza temporal de requisitos
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     db.Database.Migrate(); // 👈 Esta línea es la que causaría el error si no hay BD
//     await db.EliminarRequisitosCumplidosInvalidos();
// }

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

// ✅ Exponer carpeta ArchivosMensajes
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "ArchivosMensajes")),
    RequestPath = "/archivos"
});

// ✅ CORS
app.UseCors("AllowReactApp");

// ✅ Seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
