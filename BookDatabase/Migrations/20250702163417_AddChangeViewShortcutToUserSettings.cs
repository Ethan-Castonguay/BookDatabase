using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeViewShortcutToUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "changeViewShortcut",
                table: "userSettings",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "changeViewShortcut",
                table: "userSettings");
        }
    }
}
