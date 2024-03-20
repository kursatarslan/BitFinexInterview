using AuctionClient2.DbContext;
using AuctionClient2.Models;


namespace AuctionClient2.Repository;


public class PictureRepository : EfCoreRepository<Picture, ClientContext>
{
    public PictureRepository(ClientContext context) : base(context)
    {

    }
}

public class AuctionRepository : EfCoreRepository<Auction, ClientContext>
{
    public AuctionRepository(ClientContext context) : base(context)
    {

    }
}