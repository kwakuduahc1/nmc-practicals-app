using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingPracticals.Migrations
{
    /// <inheritdoc />
    public partial class StdID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exams_students_studentsstudentid",
                table: "exams");

            migrationBuilder.RenameColumn(
                name: "studentid",
                table: "students",
                newName: "studentsid");

            migrationBuilder.RenameColumn(
                name: "studentsstudentid",
                table: "exams",
                newName: "studentsid");

            migrationBuilder.RenameIndex(
                name: "IX_exams_studentsstudentid",
                table: "exams",
                newName: "IX_exams_studentsid");

            migrationBuilder.AddForeignKey(
                name: "fk_exams_students_studentsid",
                table: "exams",
                column: "studentsid",
                principalTable: "students",
                principalColumn: "studentsid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exams_students_studentsid",
                table: "exams");

            migrationBuilder.RenameColumn(
                name: "studentsid",
                table: "students",
                newName: "studentid");

            migrationBuilder.RenameColumn(
                name: "studentsid",
                table: "exams",
                newName: "studentsstudentid");

            migrationBuilder.RenameIndex(
                name: "IX_exams_studentsid",
                table: "exams",
                newName: "IX_exams_studentsstudentid");

            migrationBuilder.AddForeignKey(
                name: "fk_exams_students_studentsstudentid",
                table: "exams",
                column: "studentsstudentid",
                principalTable: "students",
                principalColumn: "studentid");
        }
    }
}
