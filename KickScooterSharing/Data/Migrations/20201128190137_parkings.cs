using Microsoft.EntityFrameworkCore.Migrations;

namespace KickScooterSharing.Data.Migrations
{
    public partial class parkings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLocation_Products_ProductId",
                table: "ProductLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductLocation",
                table: "ProductLocation");

            migrationBuilder.RenameTable(
                name: "ProductLocation",
                newName: "ProductLocations");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocation_ProductId",
                table: "ProductLocations",
                newName: "IX_ProductLocations_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductLocations",
                table: "ProductLocations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ParkingLocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingLocations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLocations_Products_ProductId",
                table: "ProductLocations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLocations_Products_ProductId",
                table: "ProductLocations");

            migrationBuilder.DropTable(
                name: "ParkingLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductLocations",
                table: "ProductLocations");

            migrationBuilder.RenameTable(
                name: "ProductLocations",
                newName: "ProductLocation");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocations_ProductId",
                table: "ProductLocation",
                newName: "IX_ProductLocation_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductLocation",
                table: "ProductLocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLocation_Products_ProductId",
                table: "ProductLocation",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
