using AuctionClient1.Interfaces;

namespace AuctionClient1.Models;
using System.ComponentModel.DataAnnotations;

public class Picture : IEntity
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Name { get; set; }
    public string ClientName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public int AuctionId { get; set; }
    
    public Auction Auction { get; set; }

}