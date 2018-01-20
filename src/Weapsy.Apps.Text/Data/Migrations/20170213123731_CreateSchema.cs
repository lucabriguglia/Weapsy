using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Weapsy.Apps.Text.Data.Migrations
{
    public partial class CreateSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextModule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TextModuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextVersion_TextModule_TextModuleId",
                        column: x => x.TextModuleId,
                        principalTable: "TextModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextLocalisation",
                columns: table => new
                {
                    TextVersionId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextLocalisation", x => new { x.TextVersionId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_TextLocalisation_TextVersion_TextVersionId",
                        column: x => x.TextVersionId,
                        principalTable: "TextVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextLocalisation_TextVersionId",
                table: "TextLocalisation",
                column: "TextVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TextVersion_TextModuleId",
                table: "TextVersion",
                column: "TextModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextLocalisation");

            migrationBuilder.DropTable(
                name: "TextVersion");

            migrationBuilder.DropTable(
                name: "TextModule");
        }
    }
}
