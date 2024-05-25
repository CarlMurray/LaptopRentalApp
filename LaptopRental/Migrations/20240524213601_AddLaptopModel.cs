using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRental.Migrations
{
    /// <inheritdoc />
    public partial class AddLaptopModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_brands",
                table: "brands");

            _ = migrationBuilder.RenameTable(
                name: "brands",
                newName: "Brands");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            _ = migrationBuilder.CreateTable(
                name: "Laptops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Laptops", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Laptops_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Laptops_BrandId",
                table: "Laptops",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Laptops");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            _ = migrationBuilder.RenameTable(
                name: "Brands",
                newName: "brands");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_brands",
                table: "brands",
                column: "Id");
        }
    }
}
