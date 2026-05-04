using Xunit;
using Xunit;
using StarterApp.Database.Helpers;
using StarterApp.Database.Models;
using GeoPoint = NetTopologySuite.Geometries.Point;

public class LocationServiceTests
{
    public interface ILocationService
    {
        Task<GeoPoint> GetCurrentLocationAsync();
    }
    public class MockLocationService : ILocationService
    {
        private readonly GeoPoint _point;
        private readonly Exception? _exception;

        // constructor takes GeoPoint, for success tests
        public MockLocationService(GeoPoint point) => _point = point;
        // constructor takes Exception, for fail tests
        public MockLocationService(Exception exception) => _exception = exception;

        public Task<GeoPoint> GetCurrentLocationAsync()
        {
            if (_exception != null) throw _exception;
            return Task.FromResult(_point);
        }
    }

    [Fact]
    public async Task GetCurrentLocation_ReturnsCorrectCoordinates()
    {
        // Arrange
        var expected = new GeoPoint(-1.5, 53.8) { SRID = 4326 };
        var service = new MockLocationService(expected);

        // Act
        var result = await service.GetCurrentLocationAsync();

        // Assert
        Assert.Equal(53.8, result.Y);
        Assert.Equal(-1.5, result.X);
        Assert.Equal(4326, result.SRID);
    }

    [Fact]
    public async Task GetCurrentLocation_WhenPermissionDenied_ThrowsException()
    {
        // Arrange
        var service = new MockLocationService(new Exception("Location permission was denied"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.GetCurrentLocationAsync());
        Assert.Equal("Location permission was denied", ex.Message);
    }

    [Fact]
    public async Task GetCurrentLocation_WhenGpsNotSupported_ThrowsException()
    {
        // Arrange
        var service = new MockLocationService(new Exception("GPS is not supported on this device"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.GetCurrentLocationAsync());
        Assert.Equal("GPS is not supported on this device", ex.Message);
    }
}