namespace AuctionClient2.Interfaces;

public interface IAuctionClientManager : IDisposable
{
    bool Add(string ClientId, string clientUrl);
}