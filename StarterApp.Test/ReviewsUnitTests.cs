using System.Reflection;
using Xunit;
using StarterApp.Database.Helpers;
using StarterApp.Database.Models;
using GeoPoint = NetTopologySuite.Geometries.Point;
public class ReviewsViewModelTests
{
    [Fact]
    public void ValidateReviewForm_WhenNameIsEmpty_ReturnsFalse()
    {
        // Arrange & Act
        var result = ValidateReviewForm("", "Great!", "5", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Name is required", error);
    }

    [Fact]
    public void ValidateReviewForm_WhenDescriptionIsEmpty_ReturnsFalse()
    {
        // Arrange & Act
        var result = ValidateReviewForm("John", "", "5", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Description is required", error);
    }

    [Fact]
    public void ValidateReviewForm_WhenRatingIsEmpty_ReturnsFalse()
    {
        // Arrange & Act
        var result = ValidateReviewForm("John", "Great!", "", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Rating is required", error);
    }

    [Fact]
    public void ValidateReviewForm_WhenRatingIsAboveFive_ReturnsFalse()
    {
        // Arrange & Act
        var result = ValidateReviewForm("John", "Great!", "6", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Rating must be a number between 1 and 5", error);
    }

    [Fact]
    public void ValidateReviewForm_WhenRatingIsBelowOne_ReturnsFalse()
    {
        // Arrange & Act
        var result = ValidateReviewForm("John", "Great!", "0", out var error);

        // Assert
        Assert.False(result);
        Assert.Equal("Rating must be a number between 1 and 5", error);
    }

    [Fact]
    public void ValidateReviewForm_WhenAllFieldsValid_ReturnsTrue()
    {
        // Arrange & Act
        var result = ValidateReviewForm("John", "Great!", "4", out var error);

        // Assert
        Assert.True(result);
        Assert.Empty(error);
    }
}
