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

}