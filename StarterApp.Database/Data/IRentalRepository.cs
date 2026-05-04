using StarterApp.Database.Models;
namespace StarterApp.Database.Data;

public interface IRentalRepository
{
    Task<List<Rental>> GetAllAsync();
    Task<Rental> GetByIdAsync(int id);

    Task<List<Rental>> GetByItemIdAsync(int itemId);
    Task<List<Rental>> GetByUserIdAsync(int userId);
    Task<Rental> CreateAsync(Rental rental);
    Task UpdateAsync(Rental rental);
    Task<List<RentalDetail>> GetIncomingAsync(int userId);
    Task<List<RentalDetail>> GetOutgoingAsync(int userId);
}