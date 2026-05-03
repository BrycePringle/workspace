using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace StarterApp.Database.Data;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetByItemIdAsync(int itemId) =>
        await _context.Reviews.Where(r => r.ItemId == itemId).ToListAsync();

    public async Task<Review> GetByIdAsync(int id) =>
        await _context.Reviews.FindAsync(id);

    public async Task<Review> CreateAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }
}