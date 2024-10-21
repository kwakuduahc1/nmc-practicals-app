using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingPracticals.Migrations
{
    /// <inheritdoc />
    public partial class ClsScheRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_classschedules_mainclasses_mainclassesid",
                table: "classschedules");

            migrationBuilder.AlterColumn<int>(
                name: "mainclassesid",
                table: "classschedules",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_classschedules_mainclasses_mainclassesid",
                table: "classschedules",
                column: "mainclassesid",
                principalTable: "mainclasses",
                principalColumn: "mainclassesid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_classschedules_mainclasses_mainclassesid",
                table: "classschedules");

            migrationBuilder.AlterColumn<int>(
                name: "mainclassesid",
                table: "classschedules",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_classschedules_mainclasses_mainclassesid",
                table: "classschedules",
                column: "mainclassesid",
                principalTable: "mainclasses",
                principalColumn: "mainclassesid");
        }
    }
}
