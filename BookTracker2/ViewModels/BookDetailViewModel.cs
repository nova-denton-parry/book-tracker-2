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

        private string _authorSearch = string.Empty;
        public string AuthorSearch
        {
            get => _authorSearch;
            set
            {
                SetProperty(ref _authorSearch, value);
                FilterAuthors(value);
            }
        }

        private string _genreSearch = string.Empty;
        public string GenreSearch
        {
            get => _genreSearch;
            set {
                SetProperty(ref _genreSearch, value);
                FilterGenres(value);
            }
        }
        private List<Author> _filteredAuthors = [];
        public List<Author> FilteredAuthors
        {
            get => _filteredAuthors;
            set => SetProperty(ref _filteredAuthors, value);
        }

        private List<Genre> _filteredGenres = [];
        public List<Genre> FilteredGenres
        {
            get => _filteredGenres;
            set => SetProperty(ref _filteredGenres, value);
        }

        private bool _showAuthorSuggestions;
        public bool ShowAuthorSuggestions
        {
            get => _showAuthorSuggestions;
            set => SetProperty(ref _showAuthorSuggestions, value);
        }

        private bool _showGenreSuggestions;
        public bool ShowGenreSuggestions
        {
            get => _showGenreSuggestions;
            set => SetProperty(ref _showGenreSuggestions, value);
        }

        public BookDetailViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task InitializeAsync()
        {
            Authors = await _databaseService.GetAllAuthorsAsync();
            Genres = await _databaseService.GetAllGenresAsync();

            // restore search fields if editing an existing book
            if (AuthorId != 0)
            {
                var author = Authors.FirstOrDefault(a => a.AuthorId == AuthorId);
                if (author is not null)
                    AuthorSearch = author.Name;
            }

            if (GenreId != 0)
            {
                var genre = Genres.FirstOrDefault(g => g.GenreId == GenreId);
                if (genre is not null)
                    GenreSearch = genre.Name;
            }
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
            // find or create author
            if (AuthorId == 0 && !string.IsNullOrWhiteSpace(AuthorSearch))
            {
                var existing = Authors
                    .FirstOrDefault(a => a.Name.Equals(AuthorSearch, StringComparison.OrdinalIgnoreCase));

                if (existing is not null)
                {
                    AuthorId = existing.AuthorId;
                }
                else
                {
                    var newAuthor = new Author { Name = AuthorSearch };
                    await _databaseService.SaveAuthorAsync(newAuthor);
                    AuthorId = newAuthor.AuthorId;
                }
            }

            // find or create genre
            if (GenreId == 0 && !string.IsNullOrWhiteSpace(GenreSearch))
            {
                var existing = Genres
                    .FirstOrDefault(g => g.Name.Equals(GenreSearch, StringComparison.OrdinalIgnoreCase));

                if (existing is not null)
                {
                    GenreId = existing.GenreId;
                }
                else
                {
                    var newGenre = new Genre { Name = GenreSearch };
                    await _databaseService.SaveGenreAsync(newGenre);
                    GenreId = newGenre.GenreId;
                }
            }

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

        private void FilterAuthors(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                FilteredAuthors = [];
                ShowAuthorSuggestions = false;
                return;
            }

            FilteredAuthors = Authors
                .Where(a => a.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ShowAuthorSuggestions = FilteredAuthors.Count > 0;
        }

        private void FilterGenres(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                FilteredGenres = [];
                ShowGenreSuggestions = false;
                return;
            }

            FilteredGenres = Genres
                .Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ShowGenreSuggestions = FilteredGenres.Count > 0;
        }

        public void SelectAuthor(Author author)
        {
            AuthorId = author.AuthorId;
            AuthorSearch = author.Name;
            ShowAuthorSuggestions = false;
        }

        public void SelectGenre(Genre genre)
        {
            GenreId = genre.GenreId;
            GenreSearch = genre.Name;
            ShowGenreSuggestions = false;
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await SaveBookAsync();
            await Shell.Current.GoToAsync("..");
        });
    }
}
