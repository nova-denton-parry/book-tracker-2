using BookTracker2.Models;
using BookTracker2.ViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace BookTracker2.Views;

public partial class BookDetailPage : ContentPage
{
	private readonly BookDetailViewModel _viewModel;

	public BookDetailPage(BookDetailViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
		_viewModel.NotificationRequested += async (message) =>
		{
			await DisplayAlertAsync("Book Lookup", message, "OK");
		};
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await _viewModel.InitializeAsync();

		StatusPicker.ItemsSource = Enum.GetValues<ReadingStatus>().ToList();
		StatusPicker.SelectedItem = _viewModel.Status;

		FormatPicker.ItemsSource = Enum.GetValues<BookFormat>().ToList();
		FormatPicker.SelectedItem = _viewModel.Format;

        RatingPicker.ItemsSource = new List<string> { "No rating", "1", "2", "3", "4", "5" };
        RatingPicker.SelectedItem = _viewModel.Rating.HasValue
            ? _viewModel.Rating.Value.ToString()
            : "No rating";
    }

	private void OnAuthorSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Author author)
			_viewModel.SelectAuthor(author);
	}

	private void OnGenreSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Genre genre)
			_viewModel.SelectGenre(genre);
	}

	private void OnStatusSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker && picker.SelectedItem is ReadingStatus status)
			_viewModel.Status = status;
	}

	private void OnFormatSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker && picker.SelectedItem is BookFormat format)
			_viewModel.Format = format;
	}

	private void OnRatingSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker)
		{
			_viewModel.Rating = picker.SelectedItem?.ToString() == "No rating"
				? null
				: int.Parse(picker.SelectedItem?.ToString()!);
		}
	}
}