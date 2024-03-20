using AuctionClient1.Managers;
using AuctionGrpcServices;
using Grpc.Core;
using Grpc.Net.Client;

namespace AuctionClient1.Services;

public class GenericAuctioncCientService : AuctionServerService.AuctionServerServiceClient
{
    public ILogger _logger;

    public GenericAuctioncCientService(ILogger logger, GrpcChannel channel)
        : base(channel)
    {
        _logger = logger;
    }

    public override AsyncServerStreamingCall<AuctionConclusion> RegisterAuctionConclusionNotification(
        SubscribeMessage request, CallOptions options)
    {
        var reply = base.RegisterAuctionConclusionNotification(request, options);
        Task.Run(async () =>
            {
                while (true)
                {
                    await foreach (var message in reply.ResponseStream.ReadAllAsync())
                    {
                        _logger.LogCritical($"Auction ended. Auction Id: {message.Auction.Id}, Bid : {message.Bid} ");
                    }
                }
            }
        );

        return reply;
    }

    public override AsyncServerStreamingCall<Auction> RegisterAuctionNotification(SubscribeMessage request,
        CallOptions options)
    {
        var reply = base.RegisterAuctionNotification(request, options);
        Task.Run(async () =>
            {
                while (true)
                {
                    await foreach (var message in reply.ResponseStream.ReadAllAsync())
                    {
                        _logger.LogCritical(
                            $"Auction created. Auction Id: {message.Id}, Initial Price : {message.Price} ");
                    }
                }
            }
        );

        return reply;
    }

    public override AsyncServerStreamingCall<BidNotifyMessage> RegisterBidNotification(SubscribeMessage request,
        CallOptions options)
    {
        var reply = base.RegisterBidNotification(request, options);
        Task.Run(async () =>
            {
                while (true)
                {
                    await foreach (var message in reply.ResponseStream.ReadAllAsync())
                    {
                        _logger.LogCritical(
                            $"A new bid is made. Auction Id: {message.Auction.Id}, Bid : {message.Bid} , By Client: {message.Client}");
                    }
                }
            }
        );

        return reply;
    }
}