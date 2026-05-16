using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Nexus.Domain.Interfaces;
using Nexus.Domain.Entities;
using Nexus.Application.DTOs;
using BCrypt.Net; // <--- Asegúrate de haber corrido: dotnet add package BCrypt.Net-Next

namespace Nexus.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    
    public async Task<LoginResponse?> LoginAsync(string email, string password)
{
    var user = await _userRepository.GetByEmailAsync(email);

    // 1. Validar credenciales usando BCrypt
    // Ya no comparamos directamente con '==', ahora verificamos el hash
    if (user == null || !BCrypt.Net.BCrypt.Verify(password.Trim(), user.PasswordHash.Trim()))
        return null;

    // 2. Generar el Token JWT
    var tokenHandler = new JwtSecurityTokenHandler();

    // Usamos el operador "!" para asegurar que la Key no sea nula y evitar el warning de la imagen image_fcdd04.png
    var secretKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada en appsettings.json");
    var key = Encoding.ASCII.GetBytes(secretKey);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
        Expires = DateTime.UtcNow.AddHours(8),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return new LoginResponse
    {
        Token = tokenHandler.WriteToken(token),
        Email = user.Email,
        Nombre = user.Nombre
    };
}
}