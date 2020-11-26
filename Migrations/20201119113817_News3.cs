using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBuilder.Migrations
{
    public partial class News3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectNews");

            migrationBuilder.CreateIndex(
                name: "IX_News_ProjectId",
                table: "News",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Projects_ProjectId",
                table: "News",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Projects_ProjectId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_ProjectId",
                table: "News");

            migrationBuilder.CreateTable(
                name: "ProjectNews",
                columns: table => new
                {
                    NewId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ProjectNews_News_NewId",
                        column: x => x.NewId,
                        principalTable: "News",
                        principalColumn: "NewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectNews_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNews_NewId",
                table: "ProjectNews",
                column: "NewId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNews_ProjectId",
                table: "ProjectNews",
                column: "ProjectId");
        }
    }
}
