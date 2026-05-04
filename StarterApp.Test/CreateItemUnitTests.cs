using System.Reflection;
using Xunit;
using StarterApp.Database.Helpers;
using StarterApp.Database.Models;
using GeoPoint = NetTopologySuite.Geometries.Point;

public class CreateItemViewModelTests
{
    [Fact]
    public void ValidateForm_WhenTitleIsEmpty_ReturnsFalse()
    {
        // Arrange
        var title = "";

        // Act
        var result = ValidateItemForm(title, "desc", "9.99", "Tools", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Title is required", error);
    }


    [Fact]
    public void ValidateForm_WhenDailyRateIsNegative_ReturnsFalse()
    {
        // Arrange
        var dailyRate = "-9.99";

        // Act
        var result = ValidateItemForm("title", "desc", dailyRate, "Tools", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("DailyRate cannot be negative", error);
    }

    [Fact]
    public void ValidateForm_WhenDailyRateIsString_ReturnsFalse()
    {
        // Arrange
        var dailyRate = "abcdefg";

        // Act
        var result = ValidateItemForm("title", "desc", dailyRate, "Tools", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Daily rate must be a valid decimal number", error);
    }
}
