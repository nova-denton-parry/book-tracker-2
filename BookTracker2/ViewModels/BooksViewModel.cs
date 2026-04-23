using BookTracker2.Data;
using BookTracker2.Models;
using System.Windows.Input;

namespace BookTracker2.ViewModels
{
    public class BooksViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        private List<Book> _books = [];
        public List<Book> Books
        {
            get => _books;
            set => SetProperty(ref _books, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                ApplyFilters();
            }
        }

        private ReadingStatus? _filterStatus;
        public ReadingStatus? FilterStatus
        {
            get => _filterStatus;
            set
            {
                SetProperty(ref _filterStatus, value);
                ApplyFilters();
            }
        }

        private BookFormat? _filterFormat;
        public BookFormat? FilterFormat
        {
            get => _filterFormat;
            set
            {
                SetProperty(ref _filterFormat, value);
                ApplyFilters();
            }
        }

        private int? _filterAuthorId;
        public int? FilterAuthorId
        {
            get => _filterAuthorId;
            set
            {
                SetProperty(ref _filterAuthorId, value);
                ApplyFilters();
            }
        }

        private int? _filterGenreId;
        public int? FilterGenreId
        {
            get => _filterGenreId;
            set
            {
                SetProperty(ref _filterGenreId, value);
                ApplyFilters();
            }
        }

        private int? _filterRating;
        
        public int? FilterRating
        {
            get => _filterRating;
            set
            {
                value = value != null ? Math.Clamp((int)value, 0, 5) : value ;
                SetProperty(ref _filterRating, value);
                ApplyFilters();
            }
        }

        private List<Book> _allBooks = [];
        private List<Book> _filteredBooks = [];
        public List<Book> FilteredBooks
        {
            get => _filteredBooks;
            set => SetProperty(ref _filteredBooks, value);
        }

        private bool _showFilters;
        public bool ShowFilters
        {
            get => _showFilters;
            set => SetProperty(ref _showFilters, value);
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

        public event Action? FiltersCleared;

        public BooksViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task LoadBooksAsync()
        {
            IsLoading = true;

            try
            {
                _allBooks = await _databaseService.GetAllBooksAsync();
                Authors = await _databaseService.GetAllAuthorsAsync();
                Genres = await _databaseService.GetAllGenresAsync();
                ApplyFilters();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilters()
        {
            var filtered = _allBooks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(b => b.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (FilterStatus.HasValue)
                filtered = filtered.Where(b => b.Status == FilterStatus.Value);

            if (FilterFormat.HasValue)
                filtered = filtered.Where(b => b.Format == FilterFormat.Value);

            if (FilterAuthorId.HasValue)
                filtered = filtered.Where(b => b.AuthorId == FilterAuthorId.Value);

            if (FilterGenreId.HasValue)
                filtered = filtered.Where(b => b.GenreId == FilterGenreId.Value);

            if (FilterRating.HasValue)
                filtered = filtered.Where(b => b.Rating == FilterRating.Value);

            FilteredBooks = filtered.ToList();
        }

        public async Task DeleteBookAsync(Book book)
        {
            await _databaseService.DeleteBookAsync(book);
            await LoadBooksAsync();
        }

        public ICommand NavigateToAddBookCommand => new Command(async () =>
            await Shell.Current.GoToAsync("BookDetailPage"));

        public ICommand NavigateToEditBookCommand => new Command<int>(async (int bookId) =>
            await Shell.Current.GoToAsync($"BookDetailPage?bookId={bookId}"));

        public ICommand ToggleFiltersCommand => new Command(() => ShowFilters = !ShowFilters);

        public ICommand ClearFiltersCommand => new Command(() =>
        {
            _filterStatus = null;
            _filterFormat = null;
            _filterAuthorId = null;
            _filterGenreId = null;
            _filterRating = null;
            SearchText = string.Empty;
            OnPropertyChanged(nameof(FilterStatus));
            OnPropertyChanged(nameof(FilterFormat));
            OnPropertyChanged(nameof(FilterAuthorId));
            OnPropertyChanged(nameof(FilterGenreId));
            OnPropertyChanged(nameof(FilterRating));
            ApplyFilters();
            FiltersCleared?.Invoke();
        });
    }
}
