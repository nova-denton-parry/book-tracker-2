using SQLite;

namespace BookTracker2.Models
{
    public class Genre
    {
        [PrimaryKey, AutoIncrement]
        public int GenreId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
    }
}
