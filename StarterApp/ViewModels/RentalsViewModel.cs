using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class RentalsViewModel : BaseViewModel 
{
    [ObservableProperty]
    private Item? _item;

    [ObservableProperty]
    private User? currentUser;
    private int _itemId;

    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;
    private readonly IRentalRepository _rentalRepository;
    private readonly IItemRepository _itemRepository;

    private ObservableCollection<RentalDetail> _incomingRentals = new();
    public ObservableCollection<RentalDetail> IncomingRentals
    {
        get => _incomingRentals;
        set => SetProperty(ref _incomingRentals, value);
    }

    private ObservableCollection<RentalDetail> _outgoingRentals = new();
    public ObservableCollection<RentalDetail> OutgoingRentals
    {
        get => _outgoingRentals;
        set => SetProperty(ref _outgoingRentals, value);
    }

    [ObservableProperty]
    private int _userId;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _status = string.Empty;

    public RentalsViewModel(IAuthenticationService authService, IRentalRepository rentalRepository, INavigationService navigationService, IItemRepository itemRepository)
    {
        _authService = authService;
        _rentalRepository = rentalRepository;
        _itemRepository = itemRepository;
        _navigationService = navigationService;

        LoadUser();
    }

    private void LoadUser()
    {
        currentUser = _authService.CurrentUser;
    }


    [RelayCommand]
    private async Task LoadRentalsAsync()
    {
        try
        {
            // load both incoming and outgoing separately, RentalDetail class contains rental data + user data + item data
            IncomingRentals = new ObservableCollection<RentalDetail>(await _rentalRepository.GetIncomingAsync(currentUser.Id));
            OutgoingRentals = new ObservableCollection<RentalDetail>(await _rentalRepository.GetOutgoingAsync(currentUser.Id));
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
    private async Task AcceptRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        rental.Status = "Accepted";
        await _rentalRepository.UpdateAsync(rental);
        await LoadRentalsAsync();
    }

    [RelayCommand]
    private async Task DenyRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        rental.Status = "Denied";
        await _rentalRepository.UpdateAsync(rental);
        await LoadRentalsAsync();
    }
}