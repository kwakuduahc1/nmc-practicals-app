using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingPracticals.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "componenttasksid",
                table: "classschedules");

            migrationBuilder.AddColumn<int>(
                name: "taskgroupsid",
                table: "classschedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_classschedules_taskgroupsid",
                table: "classschedules",
                column: "taskgroupsid");

            migrationBuilder.AddForeignKey(
                name: "fk_classschedules_taskgroups_taskgroupsid",
                table: "classschedules",
                column: "taskgroupsid",
                principalTable: "taskgroups",
                principalColumn: "taskgroupsid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_classschedules_taskgroups_taskgroupsid",
                table: "classschedules");

            migrationBuilder.DropIndex(
                name: "IX_classschedules_taskgroupsid",
                table: "classschedules");

            migrationBuilder.DropColumn(
                name: "taskgroupsid",
                table: "classschedules");

            migrationBuilder.AddColumn<string>(
                name: "componenttasksid",
                table: "classschedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
