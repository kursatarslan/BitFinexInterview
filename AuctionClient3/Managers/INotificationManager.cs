using System.Collections.Concurrent;
using System.Reflection;
using AuctionGrpcServices;
using Grpc.Core;

namespace AuctionClient2.Managers;

public interface INotificationManager : IDisposable
{
    void Subscribe(string ClientId, IServerStreamWriter<BidNotifyMessage> stream);
    void Subscribe(string ClientId, IServerStreamWriter<AuctionConclusion> stream);
    void Subscribe(string ClientId, IServerStreamWriter<Auction> stream);

    ValueTask SendAuctionConclusionNotification(AuctionConclusion auctionConclusion);
    ValueTask SendNewBidNotification(BidNotifyMessage message);
    ValueTask SendNewAuctionNotification(AuctionGrpcServices.Auction auction);
}

public class NotificationManager : INotificationManager
{
    //List<AuctionServices.AuctionService>
    private ILogger<NotificationManager> _logger;

    private readonly ConcurrentDictionary<string, IServerStreamWriter<AuctionConclusion>> _AuctionConclusion_Streams;
    private readonly ConcurrentDictionary<string, IServerStreamWriter<BidNotifyMessage>> _BidNotifyMessage_Streams;
    private readonly ConcurrentDictionary<string, IServerStreamWriter<Auction>> _Auction_Streams;

    public NotificationManager(ILogger<NotificationManager> logger)
    {
        _logger = logger;
    }

    public void Subscribe(string clientId, IServerStreamWriter<AuctionConclusion> stream)
    {
        _AuctionConclusion_Streams.AddOrUpdate(clientId, s=> stream, (k,v)=> v = stream);
    }
    public async ValueTask SendAuctionConclusionNotification(AuctionConclusion auctionConclusion)
    {
        await Parallel.ForEachAsync(_AuctionConclusion_Streams.Values, async (stream, ctx) =>
        {
            try
            {
                await stream.WriteAsync(auctionConclusion);
            }
            catch (Exception ex) { _logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name} error "); }
        });
    }

    public void Subscribe(string clientId, IServerStreamWriter<BidNotifyMessage> stream)
    {
        _BidNotifyMessage_Streams.AddOrUpdate(clientId, s => stream, (k, v) => v = stream);
    }
    public async ValueTask SendNewBidNotification(BidNotifyMessage message)
    {
        
        await Parallel.ForEachAsync(_BidNotifyMessage_Streams.Values, async (stream, ctx) =>
        {
            try
            {
                await stream.WriteAsync(message);
            }
            catch (Exception ex) { _logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name} error "); }
        });
    }

    public void Subscribe(string clientId, IServerStreamWriter<AuctionGrpcServices.Auction> stream)
    {
        _Auction_Streams.AddOrUpdate(clientId, s => stream, (k, v) => v = stream);
    }

    public async ValueTask SendNewAuctionNotification(AuctionGrpcServices.Auction auction)
    {
        await Parallel.ForEachAsync(_Auction_Streams.Values, async (stream, ctx) =>
        {
            try
            {
                await stream.WriteAsync(auction);
            }
            catch (Exception ex) { _logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name} error "); }

        });
    }

    public void Dispose()
    {

    }
}