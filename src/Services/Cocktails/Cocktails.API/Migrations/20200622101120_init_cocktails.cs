using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cocktails.API.Migrations
{
    public partial class init_cocktails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cocktails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    deletedAt = table.Column<DateTime>(nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Origine = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Alcool = table.Column<bool>(nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(1,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocktails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    deletedAt = table.Column<DateTime>(nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Teneur_Alcool = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    Mesure = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Preparations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    deletedAt = table.Column<DateTime>(nullable: true),
                    ID_Cocktail = table.Column<int>(nullable: false),
                    ID_Ing = table.Column<int>(nullable: false),
                    Quantite = table.Column<decimal>(type: "decimal(3,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preparations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Preparations_Cocktails_ID_Cocktail",
                        column: x => x.ID_Cocktail,
                        principalTable: "Cocktails",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Preparations_Ingredients_ID_Ing",
                        column: x => x.ID_Ing,
                        principalTable: "Ingredients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Preparations_ID_Cocktail",
                table: "Preparations",
                column: "ID_Cocktail");

            migrationBuilder.CreateIndex(
                name: "IX_Preparations_ID_Ing",
                table: "Preparations",
                column: "ID_Ing");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preparations");

            migrationBuilder.DropTable(
                name: "Cocktails");

            migrationBuilder.DropTable(
                name: "Ingredients");
        }
    }
}
