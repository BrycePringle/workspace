using StarterApp.Database.Models;
namespace StarterApp.Database.Data;

public interface IReviewRepository
{
    Task<List<Review>> GetByItemIdAsync(int itemId);
    Task<Review> GetByIdAsync(int id);
    Task<Review> CreateAsync(Review review);
    Task UpdateAsync(Review review);
}