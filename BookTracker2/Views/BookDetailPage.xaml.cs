using BookTracker2.Models;
using BookTracker2.ViewModels;

namespace BookTracker2.Views;

public partial class BookDetailPage : ContentPage
{
	private readonly BookDetailViewModel _viewModel;

	public BookDetailPage(BookDetailViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.InitializeAsync();

		StatusPicker.ItemsSource = Enum.GetValues<ReadingStatus>().ToList();
		StatusPicker.SelectedItem = _viewModel.Status;

		FormatPicker.ItemsSource = Enum.GetValues<BookFormat>().ToList();
		FormatPicker.SelectedItem = _viewModel.Format;
	}

	private void OnAuthorSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker && picker.SelectedItem is Author author)
			_viewModel.AuthorId = author.AuthorId;
	}

	private void OnGenreSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker && picker.SelectedItem is ReadingStatus status)
			_viewModel.Status = status;
	}

	private void OnStatusSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker && picker.SelectedItem is BookFormat format)
			_viewModel.Format = format;
	}
	
	private void OnFormatSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is Picker picker && picker.SelectedItem is BookFormat format)
			_viewModel.Format = format;
	}
}