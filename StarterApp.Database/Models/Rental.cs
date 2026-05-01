using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.ServerSentEvents;
using System.Reflection.Metadata;

namespace StarterApp.Database.Models;

[Table("rentals")]
[PrimaryKey(nameof(Id))]
public class Rental
{
    public int Id { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public string Status { get; set; } = "Pending";

    [Required]
    public decimal TotalCost { get; set; }

    public Item Item { get; set; } = null!;
    public User User { get; set; } = null!;
}