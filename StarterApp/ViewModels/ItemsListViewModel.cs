using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;



namespace StarterApp.ViewModels
{
    public partial class ItemsListViewModel : BaseViewModel {

    private readonly INavigationService _navigationService;
    private readonly IItemRepository _itemRepository;
    private ObservableCollection<ItemsListViewModel> _items;

    public ObservableCollection<Item> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public ICommand LoadItemsCommand { get; }

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

    /// @brief Navigates to the registration page
    /// @details Relay command that navigates to the user registration page
    /// @return A task representing the asynchronous navigation operation
    [RelayCommand]
    private async Task NavigateToItemsAsync()
    {
        await _navigationService.NavigateToAsync("RegisterPage");
    }

    }
}