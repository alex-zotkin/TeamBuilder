using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBuilder.Migrations
{
    public partial class Marks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMarks_Marks_MarkId",
                table: "UserMarks");

            migrationBuilder.DropIndex(
                name: "IX_UserMarks_MarkId",
                table: "UserMarks");

            migrationBuilder.DropColumn(
                name: "MarkId",
                table: "UserMarks");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Marks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Marks");

            migrationBuilder.AddColumn<int>(
                name: "MarkId",
                table: "UserMarks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMarks_MarkId",
                table: "UserMarks",
                column: "MarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMarks_Marks_MarkId",
                table: "UserMarks",
                column: "MarkId",
                principalTable: "Marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
