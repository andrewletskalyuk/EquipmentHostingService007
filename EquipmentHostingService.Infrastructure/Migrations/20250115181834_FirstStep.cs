using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentHostingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessEquipmentTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessEquipmentTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ProductionFacilities",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StandardAreaForEquipment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionFacilities", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentPlacementContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionFacilityCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcessEquipmentTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfUnits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentPlacementContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentPlacementContracts_ProcessEquipmentTypes_ProcessEquipmentTypeCode",
                        column: x => x.ProcessEquipmentTypeCode,
                        principalTable: "ProcessEquipmentTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentPlacementContracts_ProductionFacilities_ProductionFacilityCode",
                        column: x => x.ProductionFacilityCode,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityEquipmentTypes",
                columns: table => new
                {
                    ProductionFacilityCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcessEquipmentTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityEquipmentTypes", x => new { x.ProductionFacilityCode, x.ProcessEquipmentTypeCode });
                    table.ForeignKey(
                        name: "FK_FacilityEquipmentTypes_ProcessEquipmentTypes_ProcessEquipmentTypeCode",
                        column: x => x.ProcessEquipmentTypeCode,
                        principalTable: "ProcessEquipmentTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityEquipmentTypes_ProductionFacilities_ProductionFacilityCode",
                        column: x => x.ProductionFacilityCode,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentPlacementContracts_ProcessEquipmentTypeCode",
                table: "EquipmentPlacementContracts",
                column: "ProcessEquipmentTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentPlacementContracts_ProductionFacilityCode",
                table: "EquipmentPlacementContracts",
                column: "ProductionFacilityCode");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityEquipmentTypes_ProcessEquipmentTypeCode",
                table: "FacilityEquipmentTypes",
                column: "ProcessEquipmentTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessEquipmentTypes_Code",
                table: "ProcessEquipmentTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionFacilities_Code",
                table: "ProductionFacilities",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentPlacementContracts");

            migrationBuilder.DropTable(
                name: "FacilityEquipmentTypes");

            migrationBuilder.DropTable(
                name: "ProcessEquipmentTypes");

            migrationBuilder.DropTable(
                name: "ProductionFacilities");
        }
    }
}
