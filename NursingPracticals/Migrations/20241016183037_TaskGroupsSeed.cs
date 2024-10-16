using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NursingPracticals.Migrations
{
    /// <inheritdoc />
    public partial class TaskGroupsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "componenttaskssteps");

            migrationBuilder.AddColumn<short>(
                name: "componenttasksid1",
                table: "steps",
                type: "smallint",
                nullable: true);

            migrationBuilder.InsertData(
                table: "taskgroups",
                columns: new[] { "taskgroupsid", "groupname", "programs" },
                values: new object[,]
                {
                    { 1, "Public Health", new[] { 1, 2 } },
                    { 2, "General Nursing", new[] { 1, 3 } },
                    { 3, "Midwifery", new[] { 3 } }
                });

            migrationBuilder.CreateIndex(
                name: "IX_steps_componenttasksid1",
                table: "steps",
                column: "componenttasksid1");

            migrationBuilder.AddForeignKey(
                name: "fk_steps_componenttasks_componenttasksid1",
                table: "steps",
                column: "componenttasksid1",
                principalTable: "componenttasks",
                principalColumn: "componenttasksid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_steps_componenttasks_componenttasksid1",
                table: "steps");

            migrationBuilder.DropIndex(
                name: "IX_steps_componenttasksid1",
                table: "steps");

            migrationBuilder.DeleteData(
                table: "taskgroups",
                keyColumn: "taskgroupsid",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "taskgroups",
                keyColumn: "taskgroupsid",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "taskgroups",
                keyColumn: "taskgroupsid",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "componenttasksid1",
                table: "steps");

            migrationBuilder.CreateTable(
                name: "componenttaskssteps",
                columns: table => new
                {
                    componenttasksid = table.Column<short>(type: "smallint", nullable: false),
                    stepsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_componenttaskssteps", x => new { x.componenttasksid, x.stepsid });
                    table.ForeignKey(
                        name: "fk_componenttaskssteps_componenttasks_componenttasksid",
                        column: x => x.componenttasksid,
                        principalTable: "componenttasks",
                        principalColumn: "componenttasksid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_componenttaskssteps_steps_stepsid",
                        column: x => x.stepsid,
                        principalTable: "steps",
                        principalColumn: "stepsid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_componenttaskssteps_stepsid",
                table: "componenttaskssteps",
                column: "stepsid");
        }
    }
}
