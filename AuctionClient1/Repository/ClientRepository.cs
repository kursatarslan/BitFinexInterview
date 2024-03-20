using AuctionClient1.DbContext;
using AuctionClient1.Models;


namespace AuctionClient1.Repository;


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