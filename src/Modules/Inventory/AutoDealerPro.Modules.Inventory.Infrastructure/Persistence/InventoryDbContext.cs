using AutoDealerPro.Modules.Inventory.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Persistence;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("inventory");

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.HasIndex(v => v.PlateNumber).IsUnique();
            entity.HasIndex(v => v.Status);
            entity.HasIndex(v => new { v.Make, v.Model });

            entity.Property(v => v.PlateNumber).HasMaxLength(7).IsRequired();
            entity.Property(v => v.Make).HasMaxLength(50).IsRequired();
            entity.Property(v => v.Model).HasMaxLength(50).IsRequired();
            entity.Property(v => v.Trim).HasMaxLength(50);
            entity.Property(v => v.ExteriorColor).HasMaxLength(30);
            entity.Property(v => v.InteriorColor).HasMaxLength(30);
            entity.Property(v => v.Transmission).HasMaxLength(20);
            entity.Property(v => v.FuelType).HasMaxLength(20);
            entity.Property(v => v.BodyType).HasMaxLength(20);
            entity.Property(v => v.Notes).HasMaxLength(1000);

            entity.Property(v => v.PurchasePrice).HasColumnType("decimal(18,2)");
            entity.Property(v => v.AskingPrice).HasColumnType("decimal(18,2)");
            entity.Property(v => v.SellingPrice).HasColumnType("decimal(18,2)");

            entity.Property(v => v.Status).HasConversion<string>();

            // Store photo URLs as JSON array (simple approach)
            entity.Property(v => v.PhotoUrls)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(2000);
        });
    }
}
