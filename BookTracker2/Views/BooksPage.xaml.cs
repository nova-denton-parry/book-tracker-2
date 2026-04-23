using BookTracker2.Models;
using BookTracker2.ViewModels;

namespace BookTracker2.Views;

public partial class BooksPage : ContentPage
{
	private readonly BooksViewModel _viewModel;

	public BooksPage(BooksViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        _viewModel.FiltersCleared += OnFiltersCleared;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadBooksAsync();

        StatusFilterPicker.ItemsSource = new[] { "All" }
            .Concat(Enum.GetValues<ReadingStatus>().Select(s => s.ToString()))
            .ToList();
        FormatFilterPicker.ItemsSource = new[] { "All" }
            .Concat(Enum.GetValues<BookFormat>().Select(s => s.ToString()))
            .ToList();

        var allAuthors = new List<Author> { new Author { AuthorId = 0, Name = "All" } }
            .Concat(_viewModel.Authors)
            .ToList();
        AuthorFilterPicker.ItemsSource = allAuthors;

        var allGenres = new List<Genre> { new Genre { GenreId = 0, Name = "All" } }
            .Concat(_viewModel.Genres)
            .ToList();
        GenreFilterPicker.ItemsSource = allGenres;

        RatingFilterPicker.ItemsSource = new List<string> { "All", "1", "2", "3", "4", "5" };

        StatusFilterPicker.SelectedItem = "All";
        FormatFilterPicker.SelectedItem = "All";
        AuthorFilterPicker.SelectedItem = allAuthors[0];
        GenreFilterPicker.SelectedItem = allGenres[0];
        RatingFilterPicker.SelectedItem = "All";
    }

    private void OnStatusFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterStatus = picker.SelectedItem?.ToString() == "All" || picker.SelectedItem is null
                ? null
                : Enum.Parse<ReadingStatus>(picker.SelectedItem.ToString()!);
    }

    private void OnFormatFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterFormat = picker.SelectedItem?.ToString() == "All" || picker.SelectedItem is null
                ? null
                : Enum.Parse<BookFormat>(picker.SelectedItem.ToString()!);
    }

    private void OnAuthorFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is Author author)
            _viewModel.FilterAuthorId = author.AuthorId == 0 ? null : author.AuthorId;
    }

    private void OnGenreFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is Genre genre)
            _viewModel.FilterGenreId = genre.GenreId == 0 ? null : genre.GenreId;
    }

    private void OnRatingFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterRating = picker.SelectedItem?.ToString() == "All" || picker.SelectedItem is null
                ? null
                : int.Parse(picker.SelectedItem?.ToString()!);
    }

    private void OnFiltersCleared()
    {
        StatusFilterPicker.SelectedItem = "All";
        FormatFilterPicker.SelectedItem = "All";
        AuthorFilterPicker.SelectedItem = AuthorFilterPicker.ItemsSource?[0];
        GenreFilterPicker.SelectedItem = GenreFilterPicker.ItemsSource?[0];
        RatingFilterPicker.SelectedItem = "All";
    }
}