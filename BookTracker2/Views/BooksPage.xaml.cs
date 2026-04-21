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

        StatusFilterPicker.ItemsSource = Enum.GetValues<ReadingStatus>().ToList();
        FormatFilterPicker.ItemsSource = Enum.GetValues<BookFormat>().ToList();
    }

    private void OnStatusFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterStatus = picker.SelectedItem is ReadingStatus status ? status : null;
    }

    private void OnFormatFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterFormat = picker.SelectedItem is BookFormat format ? format : null;
    }

    private void OnAuthorFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterAuthorId = picker.SelectedItem is Author author ? author.AuthorId : null;
    }

    private void OnGenreFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
            _viewModel.FilterGenreId = picker.SelectedItem is Genre genre ? genre.GenreId : null;
    }

    private void OnFiltersCleared()
    {
        StatusFilterPicker.SelectedItem = null;
        FormatFilterPicker.SelectedItem = null;
        AuthorFilterPicker.SelectedItem = null;
        GenreFilterPicker.SelectedItem = null;
    }
}