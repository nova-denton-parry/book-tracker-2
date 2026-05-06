using BookTracker2.Data;
using BookTracker2.Models;
using System.Windows.Input;

namespace BookTracker2.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private List<Book> _allBooks = [];
        private List<Author> _allAuthors = [];
        private List<Genre> _allGenres = [];

        public event Action<TimePeriod>? PeriodChanged;

        public enum TimePeriod { ThisMonth, ThisYear, AllTime }

        private TimePeriod _selectedPeriod = TimePeriod.AllTime;
        public TimePeriod SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                SetProperty(ref _selectedPeriod, value);
                PeriodChanged?.Invoke(value);
                CalculateStats();
            }
        }

        // time period stats
        private int _booksFinished;
        public int BooksFinished
        {
            get => _booksFinished;
            set => SetProperty(ref _booksFinished, value);
        }

        private double? _averageRating;
        public double? AverageRating
        {
            get => _averageRating;
            set => SetProperty(ref _averageRating, value);
        }

        private string? _favoriteAuthor;
        public string? FavoriteAuthor
        {
            get => _favoriteAuthor;
            set => SetProperty(ref _favoriteAuthor, value);
        }

        private string? _favoriteGenre;
        public string? FavoriteGenre
        {
            get => _favoriteGenre;
            set => SetProperty(ref _favoriteGenre, value);
        }

        private string? _mostAbandonedGenre;
        public string? MostAbandonedGenre
        {
            get => _mostAbandonedGenre;
            set => SetProperty(ref _mostAbandonedGenre, value);
        }

        // all time stats
        private int _totalUnread;
        public int TotalUnread
        {
            get => _totalUnread; 
            set => SetProperty(ref _totalUnread, value);
        }

        private int _totalReading;
        public int TotalReading
        {
            get => _totalReading;
            set => SetProperty(ref _totalReading, value);
        }

        private int _totalFinished;
        public int TotalFinished
        {
            get => _totalFinished;
            set => SetProperty(ref _totalFinished, value);
        }

        private int _totalAbandoned;
        public int TotalAbandoned
        {
            get => _totalAbandoned;
            set => SetProperty(ref _totalAbandoned, value);
        }

        private int _totalPhysical;
        public int TotalPhysical
        {
            get => _totalPhysical;
            set => SetProperty(ref _totalPhysical, value);
        }

        private int _totalEbook;
        public int TotalEbook
        {
            get => _totalEbook;
            set => SetProperty(ref _totalEbook, value);
        }

        private int _totalAudiobook;
        public int TotalAudiobook
        {
            get => _totalAudiobook;
            set => SetProperty(ref _totalAudiobook, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public StatsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task LoadStatsAsync()
        {
            IsLoading = true;

            try
            {
                _allBooks = await _databaseService.GetAllBooksAsync();
                _allAuthors = await _databaseService.GetAllAuthorsAsync();
                _allGenres = await _databaseService.GetAllGenresAsync();
                CalculateStats();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculateStats()
        {
            // all time stats
            TotalUnread = _allBooks.Count(b => b.Status == ReadingStatus.Unread);
            TotalReading = _allBooks.Count(b => b.Status == ReadingStatus.Reading);
            TotalFinished = _allBooks.Count(b => b.Status == ReadingStatus.Finished);
            TotalAbandoned = _allBooks.Count(b => b.Status == ReadingStatus.Abandoned);

            TotalPhysical = _allBooks.Count(b => b.Format == BookFormat.Physical);
            TotalEbook = _allBooks.Count(b => b.Format == BookFormat.Ebook);
            TotalAudiobook = _allBooks.Count(b => b.Format == BookFormat.Audiobook);

            // filter by time period
            var now = DateTime.UtcNow;
            var periodBooks = SelectedPeriod switch
            {
                TimePeriod.ThisMonth => _allBooks.Where(b =>
                    b.DateFinished.HasValue &&
                    b.DateFinished.Value.Year == now.Year &&
                    b.DateFinished.Value.Month == now.Month),
                TimePeriod.ThisYear => _allBooks.Where(b =>
                    b.DateFinished.HasValue &&
                    b.DateFinished.Value.Year == now.Year),
                _ => _allBooks.Where(b => b.DateFinished.HasValue)
            };

            var finishedBooks = periodBooks.Where(b => b.Status == ReadingStatus.Finished).ToList();
            var abandonedBooks = periodBooks
                .Where(b => b.Status == ReadingStatus.Abandoned)
                .ToList();

            BooksFinished = finishedBooks.Count;

            AverageRating = finishedBooks.Any(b => b.Rating.HasValue)
                ? finishedBooks.Where(b => b.Rating.HasValue).Average(b => (double)b.Rating!.Value)
                : null;

            var topAuthorId = finishedBooks
                .GroupBy(b => b.AuthorId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            FavoriteAuthor = topAuthorId.HasValue
                ? _allAuthors.FirstOrDefault(a => a.AuthorId == topAuthorId.Value)?.Name
                : null;

            var topGenreId = finishedBooks
                .GroupBy(b => b.GenreId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            FavoriteGenre = topGenreId.HasValue
                ? _allGenres.FirstOrDefault(g => g.GenreId == topGenreId.Value)?.Name
                : null;

            var topAbandonedGenreId = abandonedBooks
                .GroupBy(b => b.GenreId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            MostAbandonedGenre = topAbandonedGenreId.HasValue
                ? _allGenres.FirstOrDefault(ag => ag.GenreId == topAbandonedGenreId.Value)?.Name
                : null;
        }

        public ICommand SelectPeriodCommand => new Command<string>(period =>
            SelectedPeriod = Enum.Parse<TimePeriod>(period));
    }
}
