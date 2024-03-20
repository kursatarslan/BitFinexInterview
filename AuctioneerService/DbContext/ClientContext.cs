using AuctioneerService.Models;

namespace AuctioneerService.DbContext;

using Microsoft.EntityFrameworkCore;

public class ClientContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public ClientContext(DbContextOptions<ClientContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; } = null!;
}