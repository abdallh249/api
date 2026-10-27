using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Donation> Donations => Set<Donation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.ToTable("Donations");

            entity.HasKey(d => d.Id);

            entity.Property(d => d.Amount)
                .HasPrecision(18, 2);

            entity.Property(d => d.DonorName)
                .HasMaxLength(200)
                .IsRequired();
        });
    }
}