using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class RentalsPage : ContentPage
{
    public RentalsViewModel ViewModel => BindingContext as RentalsViewModel; // for buttons to work
    public RentalsPage(RentalsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RentalsViewModel vm)
        {
            await vm.LoadRentalsCommand.ExecuteAsync(null);
        }
    }
}