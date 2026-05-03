using Microsoft.AspNetCore.Mvc;
using Nexus.Application.Services;

namespace Nexus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    // Inyectamos el servicio de aplicación
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (result == null)
            return Unauthorized(new { message = "Credenciales incorrectas" });

        return Ok(result); // Esto devuelve el JSON con el token
    }
}

// Un pequeño DTO para recibir los datos
public record LoginRequest(string Email, string Password);