using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class ReviewsViewModel : BaseViewModel 
{
    [ObservableProperty]
    private Item? _item;

    private int _itemId;
    public int ItemId
    {
        get => _itemId;
        set
        {
            _itemId = value;
        }
    }

    private readonly INavigationService _navigationService;
    private readonly ILocationService _locationService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IItemRepository _itemRepository;

    private ObservableCollection<Review> _reviews = new();
    public ObservableCollection<Review> Reviews
    {
        get => _reviews;
        set => SetProperty(ref _reviews, value);
    }

    [ObservableProperty]
    private string _newReviewName = string.Empty;

    [ObservableProperty]
    private string _newReviewDescription = string.Empty;

    [ObservableProperty]
    private string _newReviewRating = string.Empty;

    public ReviewsViewModel(IReviewRepository reviewRepository, ILocationService locationService, INavigationService navigationService, IItemRepository itemRepository)
    {
        _reviewRepository = reviewRepository;
        _itemRepository = itemRepository;
        _locationService = locationService;
        _navigationService = navigationService;
    }

    private async Task LoadItemAsync()
    {
        Item = await _itemRepository.GetByIdAsync(_itemId);
    }

    [RelayCommand]
    private async Task LoadReviewsAsync()
    {
        try
        {
            var reviews = await _reviewRepository.GetByItemIdAsync(_itemId); // was itemId (lowercase)
            await Application.Current.MainPage.DisplayAlert("Debug", $"ItemId: {_itemId}, Reviews found: {reviews.Count}", "OK");
            Reviews = new ObservableCollection<Review>(reviews);        
        }
        catch (Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            System.Diagnostics.Debug.WriteLine($"CRASH: {message}");
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);

            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }
    }

    [RelayCommand]
    private async Task AddReviewAsync()
    {
        if (IsBusy) return;
        if (!ValidateForm()) return;

        try
        {
            IsBusy = true;

            decimal.TryParse(_newReviewRating, out var rating);
            var review = new Review
            {
                ItemId = _itemId,
                Name = _newReviewName,
                Description = _newReviewDescription,
                Rating = rating,
                DatePublished = DateTimeOffset.UtcNow
            };

            await _reviewRepository.CreateAsync(review);
            await Application.Current.MainPage.DisplayAlert("Success", "Review submitted!", "OK");
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
        if (string.IsNullOrWhiteSpace(_newReviewName))
        {
            SetError("Name is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_newReviewDescription))
        {
            SetError("Description is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_newReviewRating))
        {
            SetError("Rating is required");
            return false;
        }

        if (!decimal.TryParse(_newReviewRating, out var rating) || rating < 1 || rating > 5)
        {
            SetError("Rating must be a number between 1 and 5");
            return false;
        }

        return true;
    }

    private static bool IsValidDecimal(string s)
    {
        return decimal.TryParse(s, out _);
    }
}