namespace BookDatabase.Models
{
    public class UserSettings
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public char homeShortcut { get; set; } = 'h';
        public char booksShortcut { get; set; } = 'b';
        public char aboutShortcut {  get; set; } = 'a';
        public char privacyShortcut { get; set; } = 'p';
        public char settingsShortcut { get; set; } = 's';
        public char darkModeShortcut { get; set; } = 'm';
        public char createBookShortcut { get; set; } = 'n';
        public char searchbarFocusShortcut { get; set; } = '/';
        public char genreFilterShortcut { get; set; } = 'i';
        public char ownershipFilterShortcut { get; set; } = 'o';

    }
}
