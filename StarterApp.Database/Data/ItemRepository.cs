using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

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

    public async Task<Item> GetByUserIdAsync(int userId) =>
        await _context.Items.FindAsync(userId);

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

    public async Task<List<Item>> GetNearbyAsync(double lat, double lon, double radiusKm)
    {
        var point = new Point(lon, lat) { SRID = 4326 };
        var radiusMeters = radiusKm * 1000;
        return await _context.Items
                .FromSqlRaw(@"
                SELECT * FROM ""items""
                WHERE ST_DWithin(
                    ""Location""::geography,
                    ST_MakePoint({0}, {1})::geography,
                    {2}
                )", lon, lat, radiusMeters)
            .ToListAsync();
    }
}