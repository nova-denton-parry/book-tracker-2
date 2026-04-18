using BookTracker2.Data;
using BookTracker2.Models;
using System.Windows.Input;

namespace BookTracker2.ViewModels
{
    [QueryProperty(nameof(BookId), "bookId")]
    public class BookDetailViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        private int _bookId;
        public int BookId
        {
            get => _bookId;
            set
            {
                SetProperty(ref _bookId, value);
                LoadBookAsync(value).ConfigureAwait(false);
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string? _isbn;
        public string? ISBN
        {
            get => _isbn;
            set => SetProperty(ref _isbn, value);
        }

        private int _authorId;
        public int AuthorId
        {
            get => _authorId;
            set => SetProperty(ref _authorId, value);
        }

        private int _genreId;
        public int GenreId
        {
            get => _genreId;
            set => SetProperty(ref _genreId, value);
        }

        private ReadingStatus _status = ReadingStatus.Unread;
        public ReadingStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private BookFormat? _format;
        public BookFormat? Format
        {
            get => _format;
            set => SetProperty(ref _format, value);
        }

        private int? _rating;
        public int? Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        private string? _comment;
        public string? Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        private List<Author> _authors = [];
        public List<Author> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }

        private List<Genre> _genres = [];
        public List<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BookDetailViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task InitializeAsync()
        {
            Authors = await _databaseService.GetAllAuthorsAsync();
            Genres = await _databaseService.GetAllGenresAsync();
        }

        private async Task LoadBookAsync(int bookId)
        {
            // do nothing if book doesn't exist
            if (bookId == 0)
                return;

            IsLoading = true;

            try
            {
                var book = await _databaseService.GetBookAsync(bookId);

                if (book is null)
                    return;

                Title = book.Title;
                ISBN = book.ISBN;
                AuthorId = book.AuthorId;
                GenreId = book.GenreId;
                Status = book.Status;
                Format = book.Format;
                Rating = book.Rating;
                Comment = book.Comment;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SaveBookAsync()
        {
            var book = new Book
            {
                BookId = BookId,
                Title = Title,
                ISBN = ISBN,
                AuthorId = AuthorId,
                GenreId = GenreId,
                Status = Status,
                Format = Format,
                Rating = Rating,
                Comment = Comment
            };

            if (Status == ReadingStatus.Finished && book.DateFinished is null)
                book.DateFinished = DateTime.UtcNow;

            await _databaseService.SaveBookAsync(book);
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await SaveBookAsync();
            await Shell.Current.GoToAsync("..");
        });
    }
}
