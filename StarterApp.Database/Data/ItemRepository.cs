using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;
namespace StarterApp.Database.Data;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllAsync() =>
        await _context.Items.ToListAsync();

    public async Task<Item> GetByIdAsync(int id) =>
        await _context.Items.FindAsync(id);

    public async Task<Item> CreateAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }
/*
    public async Task<List<Item>> GetNearbyAsync(double lat, double lon, double radiusKm)
    {
        // PostGIS spatial query abstracted here
        var point = new Point(lon, lat) { SRID = 4326 };
        return await _context.Items
            .Where(i => i.Location.Distance(point) <= radiusKm * 1000)
            .ToListAsync();
    }
*/


}