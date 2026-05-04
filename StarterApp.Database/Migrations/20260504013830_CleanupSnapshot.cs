using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class CleanupSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId1",
                table: "rentals",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_rentals_ItemId1",
                table: "rentals",
                column: "ItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_rentals_items_ItemId1",
                table: "rentals",
                column: "ItemId1",
                principalTable: "items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentals_items_ItemId1",
                table: "rentals");

            migrationBuilder.DropIndex(
                name: "IX_rentals_ItemId1",
                table: "rentals");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "rentals");
        }
    }
}
