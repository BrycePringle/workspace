using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;
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

    public async Task<Rental> CreateAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Items.Update(rental);
        await _context.SaveChangesAsync();
    }
/*
    public async Task<List<Rental>> GetNearbyAsync(double lat, double lon, double radiusKm)
    {
        // PostGIS spatial query abstracted here
        var point = new Point(lon, lat) { SRID = 4326 };
        return await _context.Items
            .Where(i => i.Location.Distance(point) <= radiusKm * 1000)
            .ToListAsync();
    }
*/


}