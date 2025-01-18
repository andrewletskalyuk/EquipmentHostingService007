using EquipmentHostingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentHostingService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }

    public DbSet<ProductionFacility> ProductionFacilities { get; set; }

    public DbSet<ProcessEquipmentType> ProcessEquipmentTypes { get; set; }

    public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }

    public DbSet<FacilityEquipmentType> FacilityEquipmentTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureModel(modelBuilder);
    }

    private void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FacilityEquipmentType>()
            .HasKey(fet => new { fet.ProductionFacilityCode, fet.ProcessEquipmentTypeCode });

        // Relationship between FacilityEquipmentType and ProductionFacility
        modelBuilder.Entity<FacilityEquipmentType>()
            .HasOne(fet => fet.ProductionFacility)
            .WithMany(pf => pf.FacilityEquipmentTypes)
            .HasForeignKey(fet => fet.ProductionFacilityCode);

        // Relationship between FacilityEquipmentType and ProcessEquipmentType
        modelBuilder.Entity<FacilityEquipmentType>()
            .HasOne(fet => fet.ProcessEquipmentType)
            .WithMany(pt => pt.FacilityEquipmentTypes)
            .HasForeignKey(fet => fet.ProcessEquipmentTypeCode);

        // q Relationship between EquipmentPlacementContract and ProductionFacility
        modelBuilder.Entity<EquipmentPlacementContract>()
            .HasOne(epc => epc.ProductionFacility)
            .WithMany(pf => pf.EquipmentPlacementContracts)
            .HasForeignKey(epc => epc.ProductionFacilityCode);

        // Relationship between EquipmentPlacementContract and ProcessEquipmentType
        modelBuilder.Entity<EquipmentPlacementContract>()
            .HasOne(epc => epc.ProcessEquipmentType)
            .WithMany(pt => pt.EquipmentPlacementContracts)
            .HasForeignKey(epc => epc.ProcessEquipmentTypeCode);

        // optioinal - performance!!!
        modelBuilder.Entity<ProductionFacility>()
            .HasIndex(pf => pf.Code)
            .IsUnique();

        modelBuilder.Entity<ProcessEquipmentType>()
            .HasIndex(pt => pt.Code)
            .IsUnique();
    }

    public static void SeedData(ApplicationDbContext context)
    {
        if (!context.ProductionFacilities.Any())
        {
            context.ProductionFacilities.AddRange(
                new ProductionFacility { Code = "PF001", Name = "Main Facility", StandardAreaForEquipment = 2500.0 },
                new ProductionFacility { Code = "PF002", Name = "Secondary Facility", StandardAreaForEquipment = 1800.0 },
                new ProductionFacility { Code = "PF003", Name = "Tertiary Facility", StandardAreaForEquipment = 950.0 }
            );

            context.ProcessEquipmentTypes.AddRange(
                new ProcessEquipmentType { Code = "PET001", Name = "Conveyor Belt", Area = 300.0 },
                new ProcessEquipmentType { Code = "PET002", Name = "Packing Machine", Area = 250.0 },
                new ProcessEquipmentType { Code = "PET003", Name = "Sorting Machine", Area = 150.0 }
            );

            context.SaveChanges();
        }
    }

}
