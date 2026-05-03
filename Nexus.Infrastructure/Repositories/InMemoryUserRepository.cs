using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Domain.Interfaces;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    // Simulamos una tabla de usuarios
    private readonly List<User> _users = new()
    {
        new User { Id = Guid.NewGuid(), Email = "admin@nexus.com", PasswordHash = "12345", Nombre = "Admin Nexus" }
    };

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        return Task.FromResult(user);
    }
}