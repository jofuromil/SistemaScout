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

// ✅ Activar licencia QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// ✅ Clave JWT desde appsettings.json o fija
var jwtKey = builder.Configuration["Jwt:Key"] ?? "clave-secreta-super-segura-scout";

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
builder.Services.AddScoped<EventoService>();
builder.Services.AddScoped<DocumentoEventoService>();
builder.Services.AddScoped<AuthService>();

// ✅ Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ScoutDB.db"));

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
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BackendScout",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BackendScoutUsuarios",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();
// Ejecutar migraciones automáticamente al iniciar (Render gratuito)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles(); // Sirve index.html automáticamente
app.UseStaticFiles();  // Habilita archivos estáticos como CSS y JS

// ✅ Seguridad
app.UseAuthentication();   // Debe ir primero
app.UseAuthorization();    // Luego autorización

app.MapControllers();

app.Run();
