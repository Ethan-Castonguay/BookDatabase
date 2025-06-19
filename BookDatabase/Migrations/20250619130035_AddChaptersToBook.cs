using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddChaptersToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "chapters",
                table: "Books",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chapters",
                table: "Books");
        }
    }
}
