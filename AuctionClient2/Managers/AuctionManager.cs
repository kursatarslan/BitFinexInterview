using AuctionClient3.Configs;
using AuctionClient3.DTOs;
using AuctionClient3.Models;
using AuctionClient3.Repository;

namespace AuctionClient3.Managers;

public interface IAuctionManager : IDisposable
{
    Task<bool> Add(PictureDTO picture);
    Task<bool> Bid(BidDTO bid);
    Task<(double MaxPid, Auction auction)> Finalize(AuctionBTO auc);
    Task<List<Auction>> GetOpenAuctions();
}

public class AuctionManager : IAuctionManager
{
    private readonly PictureRepository _picturerepository;
    private readonly AuctionRepository _auctionpository;
    private readonly IConfiguration _configuration;
    private ILogger<AuctionManager> _logger;
    private readonly INotificationManager _notificationManager;

    public AuctionManager(ILogger<AuctionManager> logger, INotificationManager notificationManager,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _notificationManager = notificationManager;
    }

    public async Task<bool> Add(PictureDTO picture)
    {
        var serverOptions = new ServerSettings();
        _configuration.GetSection(ServerSettings.SettingName).Bind(serverOptions);

        var newPicture = new Picture()
        {
            Name = picture.Name,
            Status = "Initial Price",
            ClientName = serverOptions.Name,
            CreatedAt = DateTime.Now
        };
        var entity = await _picturerepository.Add(newPicture);
        var newAuction = new Auction()
        {
            Name = $"{serverOptions.Name} sell {picture.Name}",
            WhoStart = serverOptions.Name,
            Last = serverOptions.Name,
            InitialPrice = picture.InitialPrice,
            LastPrice = picture.InitialPrice,
            PictureId = entity.Id,
            CreatedAt = DateTime.Now
        };

        await _auctionpository.Add(newAuction);
        _logger.LogInformation(
            $"{serverOptions.Name} opens auction: sell {picture.Name} for {picture.InitialPrice} USDt");

        //send notification to all clients
        _notificationManager.SendNewAuctionNotification(new AuctionGrpcServices.Auction()
            {
                Id = newAuction.Id.ToString(),
                Price = newAuction.InitialPrice
            }
        );

        return true; 
    }

    public async Task<bool> Bid(BidDTO bid)
    {
        var serverOptions = new ServerSettings();
        _configuration.GetSection(ServerSettings.SettingName).Bind(serverOptions);

        var auction = await _auctionpository.Get(bid.AuctionId);
        if (auction != null)
        {
            var picture = await _picturerepository.Get(auction.PictureId);
            _logger.LogInformation($"{serverOptions.Name} bids {bid} USDt for {auction.WhoStart}'s {picture.Name}.");
            auction.Last = serverOptions.Name;
            auction.LastPrice = bid.Price;
            await _auctionpository.Update(auction);
        }

        _notificationManager.SendNewBidNotification(new AuctionGrpcServices.BidNotifyMessage()
            {
                Client = auction.Last,
                Bid = bid.Price,
                Auction = new AuctionGrpcServices.Auction() { Id = auction.Id.ToString(), Price = auction.LastPrice }
            }
        );

        return true;
    }

    public async Task<(double MaxPid, Auction auction)> Finalize(AuctionBTO auc)
    {
        var auction = await _auctionpository.LastOrDefault(a => a.Id == auc.AuctionId);

        if (auction == null)
        {
            throw new Exception($"Auction not found to conclude. {auc.AuctionId}");
        }

        var serverOptions = new ServerSettings();
        _configuration.GetSection(ServerSettings.SettingName).Bind(serverOptions);
        _logger.LogInformation(
            $"{serverOptions.Name} finalizes auction, informing all about the sale to {auction.Last} at {auction.LastPrice}");

        
        _notificationManager.SendAuctionConclusionNotification(new AuctionGrpcServices.AuctionConclusion()
            {
                Client = auction.Last,
                Bid = auction.LastPrice,
                Auction = new AuctionGrpcServices.Auction() { Id = auc.AuctionId.ToString(), Price = auction.LastPrice }
            }
        );

        return (auction.LastPrice, auction);
    }

    public async Task<List<Auction>> GetOpenAuctions()
    {
        var auction = await _auctionpository.GetAll();
        return auction;
    }

    public void Dispose()
    {
    }
}