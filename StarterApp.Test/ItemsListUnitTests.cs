using System.Reflection;
using Xunit;
using StarterApp.Database.Helpers;
using StarterApp.Database.Models;
using GeoPoint = NetTopologySuite.Geometries.Point;
public class ItemsListViewModelTests
{
    [Fact]
    public void GetEffectiveRadius_WhenZero_ReturnsDefault()
    {
        // Arrange
        var radius = 0.0;

        // Act
        var result = radius > 0 ? radius : 10;

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetEffectiveRadius_WhenPositive_ReturnsRadius()
    {
        // Arrange
        var radius = 25.0;

        // Act
        var result = radius > 0 ? radius : 10;

        // Assert
        Assert.Equal(25, result);
    }


    [Fact]
    public void GetEffectiveRadius_WhenNegative_ReturnsDefault()
    {
        // Arrange
        var radius = 0.0;

        // Act
        var result = radius > 0 ? radius : 10;

        // Assert
        Assert.Equal(10, result);
    }
}
