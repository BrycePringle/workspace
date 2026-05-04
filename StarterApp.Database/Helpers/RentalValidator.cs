using StarterApp.Database.Models;

namespace StarterApp.Database.Helpers;

public static class RentalValidator
{
    public static bool CanRent(IEnumerable<Rental> existingRentals, DateTime startDate, DateTime endDate)
    {
        return !existingRentals.Any(r =>
            r.Status == "Approved" &&
            r.StartDate < endDate &&
            r.EndDate > startDate);
    }

    public static double GetEffectiveRadius(double radius)
    {
        return radius > 0 ? radius : 10;
    }
}