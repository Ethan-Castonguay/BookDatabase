using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddBookExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Completion",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "genre",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "review",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "starRating",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completion",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "genre",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "review",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "starRating",
                table: "Books");
        }
    }
}
