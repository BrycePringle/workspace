using Xunit;
using Xunit;
using StarterApp.Database.Helpers;
using StarterApp.Database.Models;
using GeoPoint = NetTopologySuite.Geometries.Point;
public class RentalServiceTests
{
    public bool CanRent(IEnumerable<Rental> existingRentals, DateTime startDate, DateTime endDate)
    {
        return !existingRentals.Any(r =>
            r.Status == "Approved" &&
            r.StartDate < endDate &&
            r.EndDate > startDate);
    }

    [Fact]
    public void CanRent_WhenDatesOverlap_ReturnsFalse()
    {
        // Arrange
        var existingRentals = new List<Rental>
        {
            new() { Status = "Approved", StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(5) }
        };

        // Act
        var result = CanRent(existingRentals, DateTime.Today.AddDays(3), DateTime.Today.AddDays(7));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanRent_WhenNoOverlap_ReturnsTrue()
    {
        // Arrange
        var existingRentals = new List<Rental>
        {
            new() { Status = "Approved", StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(5) }
        };

        // Act
        var result = CanRent(existingRentals, DateTime.Today.AddDays(6), DateTime.Today.AddDays(10));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanRent_WhenRentalIsPending_ReturnsTrue()
    {
        // Arrange
        var existingRentals = new List<Rental>
        {
            new() { Status = "Pending", StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(5) }
        };

        // Act
        var result = CanRent(existingRentals, DateTime.Today.AddDays(3), DateTime.Today.AddDays(7));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanRent_WhenNoExistingRentals_ReturnsTrue()
    {
        // Arrange
        var existingRentals = new List<Rental>();

        // Act
        var result = CanRent(existingRentals, DateTime.Today, DateTime.Today.AddDays(5));

        // Assert
        Assert.True(result);
    }
}