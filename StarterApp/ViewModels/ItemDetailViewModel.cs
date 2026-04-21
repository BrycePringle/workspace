using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Item? item;

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

    public ItemDetailViewModel(IItemRepository itemRepository, INavigationService navigationService)
    {
        _itemRepository = itemRepository;
        _navigationService = navigationService;
        Title = "Item Detail";
    }

    private async Task LoadItemAsync()
    {
        Item = await _itemRepository.GetByIdAsync(_itemId);
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await _navigationService.NavigateToAsync("..");
    }
}