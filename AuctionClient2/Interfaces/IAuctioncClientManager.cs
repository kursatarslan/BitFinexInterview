namespace AuctionClient3.Interfaces;

public interface IAuctionClientManager : IDisposable
{
    bool Add(string ClientId, string clientUrl);
}