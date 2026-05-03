using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Persistence;
using Nexus.Domain.Interfaces;

namespace Nexus.Infrastructure.Repositories;

public class SqlUserRepository : IUserRepository 
{
    private readonly NexusDbContext _context;

    public SqlUserRepository(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}