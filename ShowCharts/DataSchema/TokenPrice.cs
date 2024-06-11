using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShowCharts.DataSchema;

[PrimaryKey("Id")]
[Index(nameof(Timestamp),nameof(Symbol), AllDescending = true)]
public class TokenPrice
{
    public int Id { get; set; }
    public double Price { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    [MaxLength(64)]
    public string Symbol { get; set; }
}