using SQLite;
using BookTracker2.Models;

namespace BookTracker2.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        public DatabaseService(string dbPath)
        {
            _dbPath = dbPath;
        }

        private async Task InitializeAsync()
        {
            if (_database is not null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath);

            await _database.CreateTableAsync<Author>();
            await _database.CreateTableAsync<Genre>();
            await _database.CreateTableAsync<Book>();
        }

        // authors
        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Author>().ToListAsync();
        }

        public async Task<Author?> GetAuthorAsync(int authorId)
        {
            await InitializeAsync();
            return await _database!.Table<Author>()
                .Where(a => a.AuthorId == authorId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveAuthorAsync(Author author)
        {
            await InitializeAsync();
            if (author.AuthorId != 0)
                return await _database!.UpdateAsync(author);
            else
                return await _database!.InsertAsync(author);
        }

        public async Task<int> DeleteAuthorAsync(Author author)
        {
            await InitializeAsync();
            return await _database!.DeleteAsync(author);
        }

        // genres
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Genre>().ToListAsync();
        }

        public async Task<Genre?> GetGenreAsync(int genreId)
        {
            await InitializeAsync();
            return await _database!.Table<Genre>()
                .Where(g => g.GenreId == genreId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveGenreAsync(Genre genre)
        {
            await InitializeAsync();
            if (genre.GenreId != 0)
                return await _database!.UpdateAsync(genre);
            else
                return await _database!.InsertAsync(genre);
        }

        public async Task<int> DeleteGenreAsync(Genre genre)
        {
            await InitializeAsync();
            return await _database!.DeleteAsync(genre);
        }

        // books
        public async Task<List<Book>> GetAllBooksAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Book>().ToListAsync();
        }

        public async Task<List<Book>> GetFilteredBooksAsync(
            int? authorId = null,
            int? genreId = null,
            ReadingStatus? status = null,
            BookFormat? format = null)
        {
            await InitializeAsync();
            var books = await _database!.Table<Book>().ToListAsync();

            if (authorId.HasValue)
                books = books.Where(b => b.AuthorId == authorId.Value).ToList();

            if (genreId.HasValue)
                books = books.Where(b => b.GenreId == genreId.Value).ToList();

            if (status.HasValue)
                books = books.Where(b => b.Status == status.Value).ToList();

            if (format.HasValue)
                books = books.Where(b => b.Format == format.Value).ToList();

            return books;
        }

        public async Task<Book?> GetBookAsync(int bookId)
        {
            await InitializeAsync();
            return await _database!.Table<Book>()
                .Where(b => b.BookId == bookId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveBookAsync(Book book)
        {
            await InitializeAsync();
            if (book.BookId != 0)
                return await _database!.UpdateAsync(book);
            else
                return await _database!.InsertAsync(book);
        }

        public async Task<int> DeleteBookAsync(Book book)
        {
            await InitializeAsync();
            return await _database!.DeleteAsync(book);
        }
    }
}
