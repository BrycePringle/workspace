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
    private string title = string.Empty;

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

    public CreateItemViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _itemRepository = _itemRepository;
        Title = "List Item";
    }

    [RelayCommand]
    private async Task CreateAsync()
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

    /// @brief Navigates back to the login page
    /// @details Relay command that returns to the login page
    /// @return A task representing the asynchronous navigation operation
    [RelayCommand]
    private async Task NavigateBackToLoginAsync()
    {
        await _navigationService.NavigateBackAsync();
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(title))
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