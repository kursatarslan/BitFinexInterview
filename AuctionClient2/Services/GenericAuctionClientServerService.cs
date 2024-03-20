using AuctionClient3.DTOs;
using AuctionClient3.Interfaces;
using AuctionClient3.Managers;
using AuctionGrpcServices;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace AuctionClient3.Services;

public class GenericAuctionClientServerService : AuctionClientService.AuctionClientServiceBase
{
    private readonly ILogger<GenericAuctionClientServerService> _logger;
    private readonly IAuctionManager _auctionManager;
    private readonly IAuctionClientManager _defaultAuctioncCientManager;

    public GenericAuctionClientServerService(IAuctionManager auctionManager, 
        ILogger<GenericAuctionClientServerService> logger, 
        IAuctionClientManager defaultAuctioncCientManager)
    {
        _auctionManager = auctionManager;
        _logger = logger;
        _defaultAuctioncCientManager = defaultAuctioncCientManager;
    }

    public override async Task<DefaultResponse> RegisterNewClient(RegisterMessage request, ServerCallContext context)
    {
        _defaultAuctioncCientManager.Add(request.Client, request.ClientUrl);

        return new DefaultResponse() { Ok = true};
    }

    public override async Task<OpenAuctions> GetOpenAuctions(Empty request, ServerCallContext context)
    {
        var auctions = new OpenAuctions();
        var aucList = await _auctionManager.GetOpenAuctions();
        auctions.Auctions.AddRange(aucList.Select(s => new Auction() { Id = s.Id.ToString(), Price = s.LastPrice }));
            
        return auctions;
    }

    public override async Task<DefaultResponse> Bid(BidMessage request, ServerCallContext context)
    {
        var result = await _auctionManager.Bid( new BidDTO() { AuctionId = Convert.ToInt32(request.Auction.Id), Price = request.Bid});
        return   new DefaultResponse() { Ok = result } ;
    }
}