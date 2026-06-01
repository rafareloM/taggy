using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;

namespace taggyManagement.Infrastructure.Data;

public sealed class TaggyDbContext : DbContext
{
    public TaggyDbContext(DbContextOptions<TaggyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<TagAccount> TagAccounts => Set<TagAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<AutoRefillSettings> AutoRefillSettings => Set<AutoRefillSettings>();
    public DbSet<Trip> Trips => Set<Trip>();

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

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(vehicle => vehicle.Id);
            entity.Property(vehicle => vehicle.Id).ValueGeneratedNever();
            entity.Property(vehicle => vehicle.Plate).HasMaxLength(20).IsRequired();
            entity.Property(vehicle => vehicle.Brand).HasMaxLength(80).IsRequired();
            entity.Property(vehicle => vehicle.Model).HasMaxLength(80).IsRequired();
            entity.Property(vehicle => vehicle.Year).IsRequired();
            entity.Property(vehicle => vehicle.Propulsion).IsRequired();
            entity.Property(vehicle => vehicle.FuelConsumptionKmPerLiter);
            entity.Property(vehicle => vehicle.CO2GramsPerKm);
            entity.Property(vehicle => vehicle.BatteryKwhPerKm);
            entity.HasIndex(vehicle => vehicle.Plate).IsUnique();
        });

        modelBuilder.Entity<TagAccount>(entity =>
        {
            entity.HasKey(tagAccount => tagAccount.Id);
            entity.Property(tagAccount => tagAccount.Id).ValueGeneratedNever();
            entity.Property(tagAccount => tagAccount.UserId).IsRequired();
            entity.Property(tagAccount => tagAccount.Balance).IsRequired();
            entity.Property(tagAccount => tagAccount.CreatedAt).IsRequired();
            entity.Property(tagAccount => tagAccount.UpdatedAt).IsRequired();
            entity.HasIndex(tagAccount => tagAccount.UserId).IsUnique();
            entity.HasOne<User>()
                .WithOne()
                .HasForeignKey<TagAccount>(tagAccount => tagAccount.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(transaction => transaction.Id);
            entity.Property(transaction => transaction.Id).ValueGeneratedNever();
            entity.Property(transaction => transaction.TagAccountId).IsRequired();
            entity.Property(transaction => transaction.Type).IsRequired();
            entity.Property(transaction => transaction.Amount).IsRequired();
            entity.Property(transaction => transaction.Description).HasMaxLength(250).IsRequired();
            entity.Property(transaction => transaction.CreatedAt).IsRequired();
            entity.HasIndex(transaction => transaction.TagAccountId);
            entity.HasOne<TagAccount>()
                .WithMany()
                .HasForeignKey(transaction => transaction.TagAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AutoRefillSettings>(entity =>
        {
            entity.HasKey(settings => settings.Id);
            entity.Property(settings => settings.Id).ValueGeneratedNever();
            entity.Property(settings => settings.UserId).IsRequired();
            entity.Property(settings => settings.Enabled).IsRequired();
            entity.Property(settings => settings.MinimumBalance).IsRequired();
            entity.Property(settings => settings.RechargeAmount).IsRequired();
            entity.Property(settings => settings.CreatedAt).IsRequired();
            entity.Property(settings => settings.UpdatedAt).IsRequired();
            entity.HasIndex(settings => settings.UserId).IsUnique();
            entity.HasOne<User>()
                .WithOne()
                .HasForeignKey<AutoRefillSettings>(settings => settings.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(trip => trip.Id);
            entity.Property(trip => trip.Id).ValueGeneratedNever();
            entity.Property(trip => trip.UserId).IsRequired();
            entity.Property(trip => trip.VehicleId).IsRequired();
            entity.Property(trip => trip.DistanceKm).IsRequired();
            entity.Property(trip => trip.TollCost).IsRequired();
            entity.Property(trip => trip.FuelCost).IsRequired();
            entity.Property(trip => trip.EnergyCost).IsRequired();
            entity.Property(trip => trip.TotalCost).IsRequired();
            entity.Property(trip => trip.CO2EmissionKg).IsRequired();
            entity.Property(trip => trip.CreatedAt).IsRequired();
            entity.HasIndex(trip => trip.UserId);
            entity.HasIndex(trip => trip.VehicleId);
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(trip => trip.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Vehicle>()
                .WithMany()
                .HasForeignKey(trip => trip.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
