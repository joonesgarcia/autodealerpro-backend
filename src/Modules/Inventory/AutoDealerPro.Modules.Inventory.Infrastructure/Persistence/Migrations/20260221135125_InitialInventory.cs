using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Make = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    PlateNumber = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Trim = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Mileage = table.Column<int>(type: "integer", nullable: false),
                    ExteriorColor = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    InteriorColor = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Transmission = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FuelType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BodyType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AskingPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PhotoUrls = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ViewCount = table.Column<int>(type: "integer", nullable: false),
                    SoldAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Make_Model",
                schema: "inventory",
                table: "Vehicles",
                columns: new[] { "Make", "Model" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PlateNumber",
                schema: "inventory",
                table: "Vehicles",
                column: "PlateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Status",
                schema: "inventory",
                table: "Vehicles",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "inventory");
        }
    }
}
