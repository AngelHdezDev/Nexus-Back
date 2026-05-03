namespace Nexus.Infrastructure.Repositories;
using Nexus.Domain.Entities;
using Nexus.Domain.Interfaces;


public class InMemoryUserRepository : IUserRepository
{
    // Cambiamos el Guid.NewGuid() por un número entero (int)
    private readonly List<User> _users = new()
    {
        new User 
        { 
            Id = 1, // Ahora coincide con el tipo 'int' de tu entidad
            Email = "admin@nexus.com", 
            PasswordHash = "admin123", // Cámbialo para que coincida con tu prueba de login
            Nombre = "Admin Nexus" 
        }
    };

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        return Task.FromResult(user);
    }
}