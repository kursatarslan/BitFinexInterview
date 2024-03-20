using System.ComponentModel.DataAnnotations;
using AuctionClient3.Interfaces;

namespace AuctionClient3.Models;

public class Auction : IEntity
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Name { get; set; }
    
    public string WhoStart { get; set; }
    public string Last { get; set; }
    public double LastPrice { get; set; }
    public double InitialPrice { get; set; }
    public int PictureId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Picture Picture { get; set; }
    
}