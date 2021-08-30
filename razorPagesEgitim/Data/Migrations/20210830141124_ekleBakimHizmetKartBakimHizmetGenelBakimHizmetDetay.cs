using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace razorPagesEgitim.Data.Migrations
{
    public partial class ekleBakimHizmetKartBakimHizmetGenelBakimHizmetDetay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BakimHizmetiGenel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakinaSayacSaat = table.Column<double>(type: "float", nullable: false),
                    ToplamFiyat = table.Column<double>(type: "float", nullable: false),
                    Detaylar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EklendigiTarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MakinaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakimHizmetiGenel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BakimHizmetiGenel_Makina_MakinaId",
                        column: x => x.MakinaId,
                        principalTable: "Makina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BakimHizmetKart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakinaId = table.Column<int>(type: "int", nullable: false),
                    BakimTipiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakimHizmetKart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BakimHizmetKart_bakimTipi_BakimTipiId",
                        column: x => x.BakimTipiId,
                        principalTable: "bakimTipi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BakimHizmetKart_Makina_MakinaId",
                        column: x => x.MakinaId,
                        principalTable: "Makina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BakimHizmetiDetay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BakimHizmetiGenelId = table.Column<int>(type: "int", nullable: false),
                    BakimTipiId = table.Column<int>(type: "int", nullable: false),
                    BakimFiyati = table.Column<double>(type: "float", nullable: false),
                    BakimAdi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakimHizmetiDetay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BakimHizmetiDetay_BakimHizmetiGenel_BakimHizmetiGenelId",
                        column: x => x.BakimHizmetiGenelId,
                        principalTable: "BakimHizmetiGenel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BakimHizmetiDetay_bakimTipi_BakimTipiId",
                        column: x => x.BakimTipiId,
                        principalTable: "bakimTipi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BakimHizmetiDetay_BakimHizmetiGenelId",
                table: "BakimHizmetiDetay",
                column: "BakimHizmetiGenelId");

            migrationBuilder.CreateIndex(
                name: "IX_BakimHizmetiDetay_BakimTipiId",
                table: "BakimHizmetiDetay",
                column: "BakimTipiId");

            migrationBuilder.CreateIndex(
                name: "IX_BakimHizmetiGenel_MakinaId",
                table: "BakimHizmetiGenel",
                column: "MakinaId");

            migrationBuilder.CreateIndex(
                name: "IX_BakimHizmetKart_BakimTipiId",
                table: "BakimHizmetKart",
                column: "BakimTipiId");

            migrationBuilder.CreateIndex(
                name: "IX_BakimHizmetKart_MakinaId",
                table: "BakimHizmetKart",
                column: "MakinaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BakimHizmetiDetay");

            migrationBuilder.DropTable(
                name: "BakimHizmetKart");

            migrationBuilder.DropTable(
                name: "BakimHizmetiGenel");
        }
    }
}
