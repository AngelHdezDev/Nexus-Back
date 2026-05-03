using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; // Necesario para Task
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration; // <--- Agregado para leer el appsettings
using Nexus.Domain.Interfaces;
using Nexus.Domain.Entities;
using Nexus.Application.DTOs;

namespace Nexus.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration; // <--- Inyectamos la configuración

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        // 1. Validar credenciales contra SQL Server
        if (user == null || user.PasswordHash != password) 
            return null;

        // 2. Generar el Token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Obtenemos la llave desde el appsettings.json
        var secretKey = _configuration["Jwt:Key"];
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

        // 3. Devolver el objeto completo
        return new LoginResponse
        {
            Token = tokenHandler.WriteToken(token),
            Email = user.Email,
            Nombre = user.Nombre
        };
    }
}