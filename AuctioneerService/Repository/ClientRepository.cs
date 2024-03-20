using AuctioneerService.DbContext;
using AuctioneerService.Models;

namespace AuctioneerService.Repository;


public class ClientRepository : EfCoreRepository<Client, ClientContext>
{
    public ClientRepository(ClientContext context) : base(context)
    {

    }
}
