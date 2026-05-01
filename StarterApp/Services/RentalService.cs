using System.Globalization;
using StarterApp.Database.Models;
using StarterApp.Database.Data;

namespace StarterApp.Services;
public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IItemRepository _itemRepository;

    public RentalService(IRentalRepository rentalRepository, IItemRepository itemRepository)
    {
        _rentalRepository = rentalRepository;
        _itemRepository = itemRepository;
    }
    public async Task<bool> CanRentItemAsync(int itemId, DateTime startDate, DateTime endDate)
    {
        // Check for date overlaps with existing approved rentals
        var existingRentals = await _rentalRepository.GetByItemIdAsync(itemId);
        return !existingRentals.AsEnumerable().Any<Rental>(r => // simplify
            r.Status == "Approved" &&
            r.StartDate < endDate &&
            r.EndDate > startDate);
    }

    public async Task<Rental> RequestRentalAsync(int itemId, int borrowerId, DateTime startDate, DateTime endDate)
    {
        var item = await _itemRepository.GetByIdAsync(itemId)
            ?? throw new Exception("Item not found");

        var available = await CanRentItemAsync(itemId, startDate, endDate);
        if (!available)
            throw new Exception("Item is not available for the selected dates");

        var days = (endDate - startDate).Days;
        if (days <= 0)
            throw new Exception("End date must be after start date");

        var rental = new Rental
        {
            ItemId = itemId,
            UserId = borrowerId,
            StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc),
            Status = "Pending",
            TotalCost = item.DailyRate * days
        };

        return await _rentalRepository.CreateAsync(rental);
    }

    public async Task ApproveRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId)
            ?? throw new Exception("Rental not found");

        if (rental.Status != "Pending")
            throw new Exception("Only pending rentals can be approved");

        rental.Status = "Approved";
        await _rentalRepository.UpdateAsync(rental);
    }

        public async Task RejectRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId)
            ?? throw new Exception("Rental not found");

        if (rental.Status != "Pending")
            throw new Exception("Only pending rentals can be rejected");

        rental.Status = "Rejected";
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task ReturnRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId)
            ?? throw new Exception("Rental not found");

        if (rental.Status != "Approved")
            throw new Exception("Only approved rentals can be returned");

        rental.Status = "Returned";
        await _rentalRepository.UpdateAsync(rental);
    }
}