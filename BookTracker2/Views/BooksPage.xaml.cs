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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadBooksAsync();
    }
}