using AuctionClient3.Models;

namespace AuctionClient3.DbContext;

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