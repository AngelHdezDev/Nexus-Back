using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Domain.Entities;
using Nexus.Domain.Interfaces;

using Nexus.Domain.Enums;

namespace Nexus.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    
    // Aquí usamos el Enum
    public UserRole Role { get; set; } 
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
