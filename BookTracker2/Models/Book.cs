using SQLite;

namespace BookTracker2.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int BookId { get; set; }

        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public int AuthorId { get; set; }
        public int GenreId { get; set; }

        [MaxLength(13)]
        public string? ISBN { get; set; }

        public ReadingStatus Status { get; set; } = ReadingStatus.Unread;
        public BookFormat? Format { get; set; }

        public int? Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime? DateFinished { get; set; }
    }
}
