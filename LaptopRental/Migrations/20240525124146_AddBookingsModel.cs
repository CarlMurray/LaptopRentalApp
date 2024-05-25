using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRental.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LaptopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Bookings", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Bookings_Laptops_LaptopId",
                        column: x => x.LaptopId,
                        principalTable: "Laptops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Bookings_LaptopId",
                table: "Bookings",
                column: "LaptopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
