using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBuilder.Migrations
{
    public partial class TeamLead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamLeadUserId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamLeadUserId",
                table: "Teams",
                column: "TeamLeadUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_TeamLeadUserId",
                table: "Teams",
                column: "TeamLeadUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_TeamLeadUserId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TeamLeadUserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamLeadUserId",
                table: "Teams");
        }
    }
}
