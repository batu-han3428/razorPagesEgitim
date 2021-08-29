using Microsoft.EntityFrameworkCore.Migrations;

namespace razorPagesEgitim.Data.Migrations
{
    public partial class ekleBakimTipi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bakimTipi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BakimAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BakimFiyati = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bakimTipi", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bakimTipi");
        }
    }
}
