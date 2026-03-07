using AutoDealerPro.Modules.Leads.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Persistence
{
    public class LeadsDbContext(DbContextOptions<LeadsDbContext> options) : DbContext(options)
    {
        public DbSet<Lead> Leads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("leads");

            modelBuilder.Entity<Lead>(entity =>
            {
                entity.HasKey(l => l.Id);

                // Indexes for common queries
                entity.HasIndex(l => l.Status);
                entity.HasIndex(l => l.Type);
                entity.HasIndex(l => l.VehicleId);
                entity.HasIndex(l => l.Email);
                entity.HasIndex(l => l.AssignedToStaffId);
                entity.HasIndex(l => l.CreatedAt);

                // Customer Info
                entity.Property(l => l.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(l => l.LastName).HasMaxLength(100).IsRequired();
                entity.Property(l => l.Email).HasMaxLength(256).IsRequired();
                entity.Property(l => l.Phone).HasMaxLength(20).IsRequired();

                // Inquiry Details
                entity.Property(l => l.VehicleId).IsRequired();
                entity.Property(l => l.Type).HasConversion<string>().IsRequired();
                entity.Property(l => l.Status).HasConversion<string>().IsRequired();
                entity.Property(l => l.Message).HasMaxLength(1000).IsRequired();

                // Trade-in Info
                entity.Property(l => l.TradeInMake).HasMaxLength(50);
                entity.Property(l => l.TradeInModel).HasMaxLength(50);
                entity.Property(l => l.TradeInYear);
                entity.Property(l => l.TradeInMileage);

                // Follow-up
                entity.Property(l => l.AssignedToStaffId);
                entity.Property(l => l.ContactedAt);
                entity.Property(l => l.StaffNotes).HasMaxLength(500);

                // Navigation property - FollowUps relationship
                entity.HasMany(l => l.FollowUps)
                    .WithOne()
                    .HasForeignKey(f => f.LeadId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Timestamp properties
                entity.Property(l => l.CreatedAt).IsRequired();
                entity.Property(l => l.UpdatedAt);
            });

            modelBuilder.Entity<FollowUp>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.HasIndex(f => f.LeadId);
                entity.HasIndex(f => f.CreatedAt);

                entity.Property(f => f.Notes).HasMaxLength(500).IsRequired();
                entity.Property(f => f.CreatedAt).IsRequired();
                entity.Property(f => f.NextFollowUpDate);
            });
        }
    }
}