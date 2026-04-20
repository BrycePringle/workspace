using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StarterApp.Database.Models;

[Table("items")]
[PrimaryKey(nameof(Id))]

public class Item
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal DailyRate { get; set; }
    public string Category { get; set; }
    public string Location { get; set; }
}