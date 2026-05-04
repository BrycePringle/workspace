using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using System.Text.RegularExpressions;
using System.Net.ServerSentEvents;
using GeoPoint = NetTopologySuite.Geometries.Point;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;
    private readonly IItemRepository _itemRepository;
    private readonly ILocationService _locationService;

    [ObservableProperty]
    private User? currentUser;

    [ObservableProperty]
    private string itemTitle = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string dailyRate = string.Empty;

    [ObservableProperty]
    private string category = string.Empty;

    [ObservableProperty]
    private bool acceptTerms;

    private GeoPoint location;

    public CreateItemViewModel(INavigationService navigationService, IItemRepository itemRepository, ILocationService locationService, IAuthenticationService authService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _locationService = locationService;
        _itemRepository = itemRepository;
        Title = "List Item";

        LoadUser();
    }
    private void LoadUser()
    {
        CurrentUser = _authService.CurrentUser;
    }


    [RelayCommand]
    private async Task CreateItemAsync() {

        if (IsBusy) return;
        if (!ValidateForm()) return;

        try
        {
            IsBusy = true;

            // request location permission
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            if (status != PermissionStatus.Granted)
                throw new Exception();

            // if ok, then use location service
            var location = await _locationService.GetCurrentLocationAsync();

            var item = new Item
            {
                UserId = CurrentUser.Id,
                Title = itemTitle,
                Description = description,
                DailyRate = decimal.TryParse(dailyRate, out var rate) ? rate : 0,
                Category = category,
                Location = location
            };

            await _itemRepository.CreateAsync(item);
            await Application.Current.MainPage.DisplayAlert("Success", "Item created!", "OK");
            await _navigationService.NavigateBackAsync();
        }
        catch (Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // validation for user input
    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(itemTitle))
        {
            SetError("Title is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            SetError("Description is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(dailyRate))
        {
            SetError("Daily rate is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            SetError("Please enter a category");
            return false;
        }

        if (!decimal.TryParse(dailyRate, out var rate))
        {
            SetError("Daily rate must be a valid decimal number");
            return false;
        }

        if (rate < 0)
        {
            SetError("DailyRate cannot be negative");
            return false;
        }
        return true;
    }
}