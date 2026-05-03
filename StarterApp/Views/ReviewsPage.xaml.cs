using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class ReviewsPage : ContentPage
{
    public ReviewsPage(ReviewsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ReviewsViewModel vm)
        {
            await vm.LoadReviewsCommand.ExecuteAsync(null);
        }
    }
}