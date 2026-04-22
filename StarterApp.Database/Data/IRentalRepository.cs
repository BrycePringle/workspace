using StarterApp.Database.Models;
namespace StarterApp.Database.Data;

public interface IItemRepository
{
    Task<List<Rental>> GetAllAsync();
    Task<Rental> GetByIdAsync(int id);
    //Task<List<Item>> GetNearbyAsync(double lat, double lon, double radiusKm);
    Task<Rental> CreateAsync(Item item);
    Task UpdateAsync(Item item);
}