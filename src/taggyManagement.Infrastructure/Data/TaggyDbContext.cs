using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;

namespace taggyManagement.Infrastructure.Data;

public sealed class TaggyDbContext : DbContext
{
    public TaggyDbContext(DbContextOptions<TaggyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Id).ValueGeneratedNever();
            entity.Property(user => user.FullName).HasMaxLength(120).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(200).IsRequired();
            entity.Property(user => user.NormalizedEmail).HasMaxLength(200).IsRequired();
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.PasswordSalt).IsRequired();
            entity.Property(user => user.CreatedAt).IsRequired();
            entity.Property(user => user.UpdatedAt).IsRequired();
            entity.HasIndex(user => user.NormalizedEmail).IsUnique();
        });
    }
}