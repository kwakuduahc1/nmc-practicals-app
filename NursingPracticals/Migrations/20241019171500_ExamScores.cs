using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NursingPracticals.Migrations
{
    /// <inheritdoc />
    public partial class ExamScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_steps_componenttasks_componenttasksid1",
                table: "steps");

            migrationBuilder.DropTable(
                name: "results");

            migrationBuilder.DropIndex(
                name: "IX_steps_componenttasksid1",
                table: "steps");

            migrationBuilder.DropColumn(
                name: "componenttasksid1",
                table: "steps");

            migrationBuilder.AlterColumn<short>(
                name: "componenttasksid",
                table: "steps",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    examsid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacherschedulesid = table.Column<int>(type: "integer", nullable: false),
                    componenttasksid = table.Column<short>(type: "smallint", nullable: false),
                    score = table.Column<byte>(type: "smallint", nullable: false),
                    studentsstudentid = table.Column<int>(type: "integer", nullable: true),
                    scores = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exams", x => x.examsid);
                    table.ForeignKey(
                        name: "fk_exams_componenttasks_componenttasksid",
                        column: x => x.componenttasksid,
                        principalTable: "componenttasks",
                        principalColumn: "componenttasksid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exams_students_studentsstudentid",
                        column: x => x.studentsstudentid,
                        principalTable: "students",
                        principalColumn: "studentid");
                });

            migrationBuilder.CreateTable(
                name: "teacherschedules",
                columns: table => new
                {
                    teacherschedulesid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tutor = table.Column<string>(type: "text", nullable: false),
                    classname = table.Column<string>(type: "text", nullable: false),
                    classschedulesid = table.Column<int>(type: "integer", nullable: false),
                    studentsschedules = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teacherschedules", x => x.teacherschedulesid);
                });

            migrationBuilder.UpdateData(
                table: "taskgroups",
                keyColumn: "taskgroupsid",
                keyValue: 1,
                column: "groupname",
                value: "Home Visit");

            migrationBuilder.InsertData(
                table: "taskgroups",
                columns: new[] { "taskgroupsid", "groupname", "programs" },
                values: new object[] { 4, "Child Welfare Clinic", new[] { 1, 2 } });

            migrationBuilder.CreateIndex(
                name: "IX_steps_componenttasksid",
                table: "steps",
                column: "componenttasksid");

            migrationBuilder.CreateIndex(
                name: "IX_exams_componenttasksid",
                table: "exams",
                column: "componenttasksid");

            migrationBuilder.CreateIndex(
                name: "IX_exams_studentsstudentid",
                table: "exams",
                column: "studentsstudentid");

            migrationBuilder.AddForeignKey(
                name: "fk_steps_componenttasks_componenttasksid",
                table: "steps",
                column: "componenttasksid",
                principalTable: "componenttasks",
                principalColumn: "componenttasksid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_steps_componenttasks_componenttasksid",
                table: "steps");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "teacherschedules");

            migrationBuilder.DropIndex(
                name: "IX_steps_componenttasksid",
                table: "steps");

            migrationBuilder.DeleteData(
                table: "taskgroups",
                keyColumn: "taskgroupsid",
                keyValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "componenttasksid",
                table: "steps",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<short>(
                name: "componenttasksid1",
                table: "steps",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "results",
                columns: table => new
                {
                    resultsid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    classschedulesid = table.Column<int>(type: "integer", nullable: false),
                    studentsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_results", x => x.resultsid);
                    table.ForeignKey(
                        name: "fk_results_students_studentsid",
                        column: x => x.studentsid,
                        principalTable: "students",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "taskgroups",
                keyColumn: "taskgroupsid",
                keyValue: 1,
                column: "groupname",
                value: "Public Health");

            migrationBuilder.CreateIndex(
                name: "IX_steps_componenttasksid1",
                table: "steps",
                column: "componenttasksid1");

            migrationBuilder.CreateIndex(
                name: "IX_results_studentsid",
                table: "results",
                column: "studentsid");

            migrationBuilder.AddForeignKey(
                name: "fk_steps_componenttasks_componenttasksid1",
                table: "steps",
                column: "componenttasksid1",
                principalTable: "componenttasks",
                principalColumn: "componenttasksid");
        }
    }
}
