using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace StarterApp.Database.Data;

// Class used to join rental, item and, user. Needed to display rentals with info
public class RentalDetail
{
    public int RentalId { get; set; }
    public string ItemTitle { get; set; } = string.Empty;
    public string PersonName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public bool IsPending => Status == "Pending";
}
