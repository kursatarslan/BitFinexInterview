using System.ComponentModel.DataAnnotations;
using AuctioneerService.Interfaces;

namespace AuctioneerService.Models;

public class Client : IEntity
{
    public long Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    [StringLength(60)]
    public string Url { get; set; }
    
    public Boolean Status { get; set; }
}