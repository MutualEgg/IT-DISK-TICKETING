using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Migrations
{
    /// <inheritdoc />
    public partial class updateworker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Users");
        }
    }
}
