using Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Domain.Interfaces
{
    // ASÍ DEBE QUEDAR:
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
