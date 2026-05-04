using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace StarterApp.Database.Data;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rental>> GetAllAsync() =>
        await _context.Rentals.ToListAsync();

    public async Task<Rental> GetByIdAsync(int id) =>
        await _context.Rentals.FindAsync(id);

    public async Task<List<Rental>> GetByItemIdAsync(int itemId) =>
        await _context.Rentals.Where(r => r.ItemId == itemId).ToListAsync();
    
    public async Task<List<Rental>> GetByUserIdAsync(int userId) =>
        await _context.Rentals.Where(r => r.UserId == userId).ToListAsync();

    public async Task<Rental> CreateAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<List<RentalDetail>> GetIncomingAsync(int userId)
    {
        return await _context.Rentals
            .Where(r => r.UserId == userId)
            .Select(r => new RentalDetail
            {
                RentalId = r.Id,
                ItemId = r.Item.Id,
                ItemTitle = r.Item.Title,
                PersonName = r.Item.User.FirstName,  // owner you're renting from
                Status = r.Status,
                UserId = r.UserId
            })
            .ToListAsync();
    }

    public async Task<List<RentalDetail>> GetOutgoingAsync(int userId)
    {
        return await _context.Rentals
            .Where(r => r.Item.UserId == userId)
            .Select(r => new RentalDetail
            {
                RentalId = r.Id,
                ItemId = r.Item.Id,
                ItemTitle = r.Item.Title,
                PersonName = r.User.FirstName,  // person renting your item
                Status = r.Status,
                UserId = r.UserId
            })
            .ToListAsync();
    }
}