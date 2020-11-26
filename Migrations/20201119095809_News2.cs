using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBuilder.Migrations
{
    public partial class News2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectNews_NewId",
                table: "ProjectNews",
                column: "NewId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNews_ProjectId",
                table: "ProjectNews",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectNews_News_NewId",
                table: "ProjectNews",
                column: "NewId",
                principalTable: "News",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectNews_Projects_ProjectId",
                table: "ProjectNews",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectNews_News_NewId",
                table: "ProjectNews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectNews_Projects_ProjectId",
                table: "ProjectNews");

            migrationBuilder.DropIndex(
                name: "IX_ProjectNews_NewId",
                table: "ProjectNews");

            migrationBuilder.DropIndex(
                name: "IX_ProjectNews_ProjectId",
                table: "ProjectNews");
        }
    }
}
