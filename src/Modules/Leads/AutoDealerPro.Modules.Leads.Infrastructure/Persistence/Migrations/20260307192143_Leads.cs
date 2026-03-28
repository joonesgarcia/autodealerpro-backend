using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealerPro.Modules.Leads.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Leads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "leads");

            migrationBuilder.CreateTable(
                name: "Leads",
                schema: "leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TradeInMake = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TradeInModel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TradeInYear = table.Column<int>(type: "integer", nullable: true),
                    TradeInMileage = table.Column<int>(type: "integer", nullable: true),
                    AssignedToStaffId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContactedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StaffNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                schema: "leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeadId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextFollowUpDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUp_Leads_LeadId",
                        column: x => x.LeadId,
                        principalSchema: "leads",
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_CreatedAt",
                schema: "leads",
                table: "FollowUp",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_LeadId",
                schema: "leads",
                table: "FollowUp",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_AssignedToStaffId",
                schema: "leads",
                table: "Leads",
                column: "AssignedToStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CreatedAt",
                schema: "leads",
                table: "Leads",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Email",
                schema: "leads",
                table: "Leads",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Status",
                schema: "leads",
                table: "Leads",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Type",
                schema: "leads",
                table: "Leads",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_VehicleId",
                schema: "leads",
                table: "Leads",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUp",
                schema: "leads");

            migrationBuilder.DropTable(
                name: "Leads",
                schema: "leads");
        }
    }
}
