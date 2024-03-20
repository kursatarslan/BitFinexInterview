using AuctionClient1.Models;

namespace AuctionClient1.DbContext;

using Microsoft.EntityFrameworkCore;

public class ClientContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public ClientContext(DbContextOptions<ClientContext> options)
        : base(options)
    {
    }

    public DbSet<Picture> Pictures { get; set; } = null!;
}