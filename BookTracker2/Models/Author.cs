using SQLite;

namespace BookTracker2.Models
{
    public class Author
    {
        [PrimaryKey, AutoIncrement]
        public int AuthorId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
    }
}
