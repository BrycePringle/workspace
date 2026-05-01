using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : BaseViewModel 
{
    private readonly INavigationService _navigationService;
    private readonly ILocationService _locationService;
    private readonly IItemRepository _itemRepository;
    private ObservableCollection<Item> _items = new();

    public ObservableCollection<Item> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    [ObservableProperty]
    private double _radius = 10; // default 10km

    public ItemsListViewModel(IItemRepository itemRepository, ILocationService locationService, INavigationService navigationService)
    {
        _itemRepository = itemRepository;
        _locationService = locationService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        try
        {
            var location = await _locationService.GetCurrentLocationAsync();
            var effectiveRadius = _radius > 0 ? _radius : 10;

            // NTS Point: X = longitude, Y = latitude
            var items = await _itemRepository.GetNearbyAsync(location.Y, location.X, effectiveRadius);
            Items = new ObservableCollection<Item>(items);
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
    private async Task NavigateToItemDetailAsync(Item item)
    {
        await Shell.Current.GoToAsync("ItemDetailPage", new Dictionary<string, object>
        {
            { "itemId", item.Id }
        });
    }
}