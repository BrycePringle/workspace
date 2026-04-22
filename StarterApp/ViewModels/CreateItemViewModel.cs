using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using System.Text.RegularExpressions;
using System.Net.ServerSentEvents;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IItemRepository _itemRepository;

    [ObservableProperty]
    private string itemTitle = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string dailyRate = string.Empty;

    [ObservableProperty]
    private string category = string.Empty;

    [ObservableProperty]
    private string location = string.Empty;

    [ObservableProperty]
    private bool acceptTerms;

    public CreateItemViewModel(INavigationService navigationService, IItemRepository itemRepository)
    {
        _navigationService = navigationService;
        _itemRepository = itemRepository;
        Title = "List Item";
    }
/*
    [RelayCommand]
    private async Task CreateItemAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        var item = new Item
        {
            Title = title,
            Description = description,
            DailyRate = decimal.Parse(dailyRate),
            Category = category,
            Location = location
        };

        // main page?
        await _itemRepository.CreateAsync(item);
        await Application.Current.MainPage.DisplayAlert("Success", "Item created!", "OK");
        await _navigationService.NavigateBackAsync();
    }
*/
    [RelayCommand]
    private async Task CreateItemAsync()
    {
        if (IsBusy) return;
        if (!ValidateForm()) return;

        try
        {
            IsBusy = true;
            var item = new Item
            {
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

        if (string.IsNullOrWhiteSpace(location))
        {
            SetError("Location is required");
            return false;
        }

        if (!isValidDecimal(dailyRate))
        {
            SetError("Daily rate must be a valid decimal number");
            return false;
        }
        return true;
    }

    /// @brief Validates an email address format
    /// @param email The email address to validate
    /// @return True if the email format is valid, false otherwise
    /// @details Uses regex pattern matching to validate email format
    private static bool isValidDecimal(string s)
    {
        return decimal.TryParse(s, out _);
    }
}
