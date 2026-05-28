using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealerPro.Modules.Leads.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityToLead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                schema: "leads",
                table: "Leads",
                type: "integer",
                maxLength: 1,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Priority",
                schema: "leads",
                table: "Leads",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Leads_Priority",
                schema: "leads",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "leads",
                table: "Leads");
        }
    }
}
