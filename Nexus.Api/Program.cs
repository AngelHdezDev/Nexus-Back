using Nexus.Domain.Interfaces;
using Nexus.Infrastructure.Repositories;
using Nexus.Application.Services;
using Microsoft.EntityFrameworkCore;
using Nexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);




// --- AGREGAR ESTAS 3 LÍNEAS AQUÍ ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Esto quitará el error de la línea 10

// Tus inyecciones
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddDbContext<NexusDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. Definir la política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NexusPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // El puerto de tu React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// --- CONFIGURACIÓN DE SWAGGER ---
app.UseSwagger(); // Quitará el error de la línea 21
app.UseSwaggerUI(c => // Quitará el error de la línea 22
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("NexusPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();