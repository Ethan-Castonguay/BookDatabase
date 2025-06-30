using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    homeShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    booksShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    aboutShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    privacyShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    settingsShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    darkModeShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    createBookShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    searchbarFocusShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    genreFilterShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    ownershipFilterShortcut = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userSettings");
        }
    }
}
