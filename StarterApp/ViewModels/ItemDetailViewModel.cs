using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authService;
    private readonly IItemRepository _itemRepository;
    private readonly INavigationService _navigationService;
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private Item? item;

    [ObservableProperty]
    private User? currentUser;


    [ObservableProperty]
    private int _userId;
    
    private int _itemId;

    public int ItemId
    {
        get => _itemId;
        set
        {
            _itemId = value;
            _ = Task.Run(LoadItemAsync);
        }
    }

    [ObservableProperty]
    private string startDate = string.Empty;

    [ObservableProperty]
    private string endDate = string.Empty;

    public ItemDetailViewModel(IItemRepository itemRepository, INavigationService navigationService, IRentalService rentalService, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _navigationService = navigationService;
        _rentalService = rentalService;
        _authService = authService;
        Title = "Item Detail";
    }

    private async Task LoadItemAsync()
    {
        Item = await _itemRepository.GetByIdAsync(_itemId);
    }

    private void LoadUser()
    {
        CurrentUser = _authService.CurrentUser;
        UserId = CurrentUser.Id;
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await _navigationService.NavigateToAsync("..");
    }

    [RelayCommand]
    private async Task CreateRentalAsync()
    {
        LoadUser();

        try
        {
            DateTime convertedStartDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime convertedEndDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // display message if double booking
            bool canRent = await _rentalService.CanRentItemAsync(ItemId, convertedStartDate, convertedEndDate);

            if (!canRent)
            {
                string message = "Dates overlap with existing booking!";
                await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
                return;
            }
            // checks if valid, no need for logic
            await _rentalService.RequestRentalAsync(ItemId, UserId, convertedStartDate, convertedEndDate);
            await Application.Current.MainPage.DisplayAlert("Success", "Rental requested!", "OK");
            
            await _navigationService.NavigateBackAsync();
        }
        catch (Exception ex)
        {
            //SetError($"Rental request failed: {ex.Message}");
            var message = ex.InnerException?.Message ?? ex.Message;
            await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }
    }

        [RelayCommand]
        private async Task NavigateToReviewsAsync(Item item)
        {    
            System.Diagnostics.Debug.WriteLine($"NavigateToReviews called, item: {item?.Id} - {item?.Title}");
    
            if (item == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Item is null", "OK");
                return;
            }
            
            await Shell.Current.GoToAsync("ReviewsPage", new Dictionary<string, object>
            {
                { "itemId", item.Id }
            });
        }
}