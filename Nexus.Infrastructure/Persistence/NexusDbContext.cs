using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.Persistence;

public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options) { }

    // Esta es la tabla que se creará físicamente
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración básica para la tabla de usuarios
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Email).IsUnique(); // Para que no haya correos repetidos
            entity.Property(e => e.PasswordHash).IsRequired();
            
            // EF Core mapeará el Enum automáticamente como un entero (int)
            entity.Property(e => e.Role).IsRequired();
        });
    }
}