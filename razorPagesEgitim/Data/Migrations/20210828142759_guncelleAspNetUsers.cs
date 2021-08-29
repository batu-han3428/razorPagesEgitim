using Microsoft.EntityFrameworkCore.Migrations;

namespace razorPagesEgitim.Data.Migrations
{
    public partial class guncelleAspNetUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "adSoyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "adres",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "postaKodu",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sehir",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "adSoyad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "adres",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "postaKodu",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "sehir",
                table: "AspNetUsers");
        }
    }
}
