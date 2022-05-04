using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore_DiscoveryService.Migrations
{
    public partial class BookIdId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Books",
                newName: "BookId");
        }
    }
}
