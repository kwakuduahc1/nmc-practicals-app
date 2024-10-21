using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingPracticals.Migrations
{
    /// <inheritdoc />
    public partial class SchdStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "classschedules",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isactive",
                table: "classschedules");
        }
    }
}
