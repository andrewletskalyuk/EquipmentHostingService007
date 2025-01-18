﻿// <auto-generated />
using EquipmentHostingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EquipmentHostingService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250115181834_FirstStep")]
    partial class FirstStep
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.EquipmentPlacementContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("NumberOfUnits")
                        .HasColumnType("int");

                    b.Property<string>("ProcessEquipmentTypeCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductionFacilityCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessEquipmentTypeCode");

                    b.HasIndex("ProductionFacilityCode");

                    b.ToTable("EquipmentPlacementContracts");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.FacilityEquipmentType", b =>
                {
                    b.Property<string>("ProductionFacilityCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProcessEquipmentTypeCode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProductionFacilityCode", "ProcessEquipmentTypeCode");

                    b.HasIndex("ProcessEquipmentTypeCode");

                    b.ToTable("FacilityEquipmentTypes");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.ProcessEquipmentType", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Area")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ProcessEquipmentTypes");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.ProductionFacility", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("StandardAreaForEquipment")
                        .HasColumnType("float");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ProductionFacilities");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.EquipmentPlacementContract", b =>
                {
                    b.HasOne("EquipmentHostingService.Domain.Entities.ProcessEquipmentType", "ProcessEquipmentType")
                        .WithMany("EquipmentPlacementContracts")
                        .HasForeignKey("ProcessEquipmentTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EquipmentHostingService.Domain.Entities.ProductionFacility", "ProductionFacility")
                        .WithMany("EquipmentPlacementContracts")
                        .HasForeignKey("ProductionFacilityCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessEquipmentType");

                    b.Navigation("ProductionFacility");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.FacilityEquipmentType", b =>
                {
                    b.HasOne("EquipmentHostingService.Domain.Entities.ProcessEquipmentType", "ProcessEquipmentType")
                        .WithMany("FacilityEquipmentTypes")
                        .HasForeignKey("ProcessEquipmentTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EquipmentHostingService.Domain.Entities.ProductionFacility", "ProductionFacility")
                        .WithMany("FacilityEquipmentTypes")
                        .HasForeignKey("ProductionFacilityCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessEquipmentType");

                    b.Navigation("ProductionFacility");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.ProcessEquipmentType", b =>
                {
                    b.Navigation("EquipmentPlacementContracts");

                    b.Navigation("FacilityEquipmentTypes");
                });

            modelBuilder.Entity("EquipmentHostingService.Domain.Entities.ProductionFacility", b =>
                {
                    b.Navigation("EquipmentPlacementContracts");

                    b.Navigation("FacilityEquipmentTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
