using StarterApp.Database.Models;
namespace StarterApp.Database.Data;

public interface IItemRepository
{
    Task<List<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(int id);
    Task<Item> GetByUserIdAsync(int userId);
    Task<List<Item>> GetNearbyAsync(double lat, double lon, double radiusKm);
    Task<Item> CreateAsync(Item item);
    Task UpdateAsync(Item item);
}