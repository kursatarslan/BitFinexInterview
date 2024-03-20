using AuctionClient3.Managers;
using AuctionGrpcServices;
using Grpc.Core;

namespace AuctionClient3.Services;

public class GenericAuctionServerService: AuctionServerService.AuctionServerServiceBase
{
    private ILogger<GenericAuctionServerService> _logger;
    private INotificationManager _notificationManager;

    public GenericAuctionServerService(INotificationManager notificationManager, ILogger<GenericAuctionServerService> logger)
    {
        _notificationManager = notificationManager;
        _logger = logger;
    }

    public override Task RegisterAuctionConclusionNotification(SubscribeMessage request, IServerStreamWriter<AuctionConclusion> responseStream, ServerCallContext context)
    {
            
        _notificationManager.Subscribe(request.Client, responseStream);
        return Task.CompletedTask;
    }

    public override Task RegisterAuctionNotification(SubscribeMessage request, IServerStreamWriter<Auction> responseStream, ServerCallContext context)
    {
        _notificationManager.Subscribe(request.Client, responseStream);
        return Task.CompletedTask;
    }

    public override Task RegisterBidNotification(SubscribeMessage request, IServerStreamWriter<BidNotifyMessage> responseStream, ServerCallContext context)
    {
        _notificationManager.Subscribe(request.Client, responseStream);
        return Task.CompletedTask;
    }
}