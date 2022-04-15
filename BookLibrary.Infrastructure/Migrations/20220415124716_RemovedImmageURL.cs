using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrary.Infrastructure.Migrations
{
    public partial class RemovedImmageURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Authors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
