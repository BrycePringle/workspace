using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.ServerSentEvents;
using System.Reflection.Metadata;

namespace StarterApp.Database.Models;

[Table("reviews")]
[PrimaryKey(nameof(Id))]
public class Review
{
    public int Id { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTimeOffset DatePublished { get; set; }
    
    [Required]
    public decimal Rating { get; set; }

    [Required]
    public string Description { get; set; }

    public Item Item { get; set; } = null!;
}