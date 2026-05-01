using StarterApp.Database.Models;
using GeoPoint = NetTopologySuite.Geometries.Point;

namespace StarterApp.Services;

public interface ILocationService
{
    Task<GeoPoint> GetCurrentLocationAsync();
}