using BookTracker2.ViewModels;

namespace BookTracker2.Views;

public partial class StatsPage : ContentPage
{
	private readonly StatsViewModel _viewModel;

	public StatsPage(StatsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		_viewModel.PeriodChanged += OnPeriodChanged;
		OnPeriodChanged(StatsViewModel.TimePeriod.AllTime);	// set initial state
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadStatsAsync();
	}

	private void OnPeriodChanged(StatsViewModel.TimePeriod period)
	{
		var selected = (Color)Application.Current!.Resources["PrimaryDark"];
		var deselected = (Color)Application.Current!.Resources["Gray500"];

		ThisMonthButton.BackgroundColor = period == StatsViewModel.TimePeriod.ThisMonth
			? selected : deselected;
        ThisYearButton.BackgroundColor = period == StatsViewModel.TimePeriod.ThisYear
            ? selected : deselected;
        AllTimeButton.BackgroundColor = period == StatsViewModel.TimePeriod.AllTime
            ? selected : deselected;
    }
}