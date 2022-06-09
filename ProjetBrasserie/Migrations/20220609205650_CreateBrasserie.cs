using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBrasserie.Migrations
{
    public partial class CreateBrasserie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brasseries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brasseries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grossistes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grossistes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bieres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Degre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrasserieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bieres_Brasseries_BrasserieId",
                        column: x => x.BrasserieId,
                        principalTable: "Brasseries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrossisteStocks",
                columns: table => new
                {
                    GrossisteId = table.Column<int>(type: "int", nullable: false),
                    BiereId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrossisteStocks", x => new { x.GrossisteId, x.BiereId });
                    table.ForeignKey(
                        name: "FK_GrossisteStocks_Bieres_BiereId",
                        column: x => x.BiereId,
                        principalTable: "Bieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrossisteStocks_Grossistes_GrossisteId",
                        column: x => x.GrossisteId,
                        principalTable: "Grossistes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brasseries",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { 1, "Abbaye de Maredsous" },
                    { 2, "Brasserie d'Achouffe" },
                    { 3, "Trappist Westvleteren" }
                });

            migrationBuilder.InsertData(
                table: "Grossistes",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { 1, "GeneDrinks" },
                    { 2, "Top Beer" }
                });

            migrationBuilder.InsertData(
                table: "Bieres",
                columns: new[] { "Id", "BrasserieId", "Degre", "Nom", "Prix" },
                values: new object[,]
                {
                    { 1, 1, 6m, "Maredsous Blonde", 2.6m },
                    { 2, 1, 8m, "Maredsous Brune", 3m },
                    { 3, 1, 10m, "Maredsous Triple", 3m },
                    { 4, 2, 8m, "La Chouffe", 2.5m },
                    { 5, 2, 8m, "Cherry Chouffe", 2.5m },
                    { 6, 3, 5.8m, "Westvleteren Blond", 3.2m },
                    { 7, 3, 8m, "Westvleteren 8", 3.5m },
                    { 8, 3, 10.2m, "Westvleteren 12", 3.5m }
                });

            migrationBuilder.InsertData(
                table: "GrossisteStocks",
                columns: new[] { "BiereId", "GrossisteId", "Quantite" },
                values: new object[,]
                {
                    { 1, 1, 30 },
                    { 2, 1, 25 },
                    { 4, 2, 15 },
                    { 6, 1, 15 },
                    { 7, 2, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bieres_BrasserieId",
                table: "Bieres",
                column: "BrasserieId");

            migrationBuilder.CreateIndex(
                name: "IX_GrossisteStocks_BiereId",
                table: "GrossisteStocks",
                column: "BiereId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrossisteStocks");

            migrationBuilder.DropTable(
                name: "Bieres");

            migrationBuilder.DropTable(
                name: "Grossistes");

            migrationBuilder.DropTable(
                name: "Brasseries");
        }
    }
}
