using Microsoft.Maui.Devices.Sensors;
using GeoPoint = NetTopologySuite.Geometries.Point;

namespace StarterApp.Services;

public class LocationService : ILocationService
{
    public async Task<GeoPoint> GetCurrentLocationAsync()
    {
        try
        {
            var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });

            location ??= await Geolocation.Default.GetLastKnownLocationAsync();

            if (location == null)
                throw new Exception("Unable to get location");

            return new GeoPoint(location.Longitude, location.Latitude) { SRID = 4326 };
        }
        catch (FeatureNotSupportedException)
        {
            throw new Exception("GPS is not supported on this device");
        }
        catch (PermissionException)
        {
            throw new Exception("Location permission was denied");
        }
    }
}