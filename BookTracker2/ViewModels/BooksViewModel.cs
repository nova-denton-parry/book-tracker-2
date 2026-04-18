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

        public BooksViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task LoadBooksAsync()
        {
            IsLoading = true;

            try
            {
                Books = await _databaseService.GetAllBooksAsync();
            }
            finally
            {
                IsLoading = false;
            }
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
    }
}
