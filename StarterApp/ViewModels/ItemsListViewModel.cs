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
    private readonly IItemRepository _itemRepository;
    private ObservableCollection<Item> _items;

    public ObservableCollection<Item> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public AsyncRelayCommand LoadItemsCommand { get; }

    public ItemsListViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
    }

    private async Task LoadItemsAsync()
    {
        var items = await _itemRepository.GetAllAsync();
        Items = new ObservableCollection<Item>(items);
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