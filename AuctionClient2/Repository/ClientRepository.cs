using AuctionClient3.DbContext;
using AuctionClient3.Models;


namespace AuctionClient3.Repository;


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