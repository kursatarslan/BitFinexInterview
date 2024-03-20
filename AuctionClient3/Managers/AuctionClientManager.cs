using System.Collections.Concurrent;
using AuctionClient2.Configs;
using AuctionClient2.Interfaces;
using AuctionClient2.Services;
using AuctionGrpcServices;
using Grpc.Net.Client;

namespace AuctionClient2.Managers;

public class AuctionClientManager : IAuctionClientManager
{
    private ILogger<AuctionClientManager> _logger; 
    private IConfiguration _configuration; 
    private readonly ConcurrentDictionary<string, (AuctionServerService.AuctionServerServiceClient NotificationClient, AuctionClientService.AuctionClientServiceClient ServerClient)> _clientConnections;
    
    public AuctionClientManager(ILogger<AuctionClientManager> logger, INotificationManager notificationManager,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _clientConnections = new(); 
    }

    public bool Add(string clientId, string clientUrl)
    {
        if (_clientConnections.ContainsKey(clientId))
            return false;

        using var channel = GrpcChannel.ForAddress(clientUrl);
        var notificationClient = new GenericAuctioncCientService(_logger, channel);
        var serverOptions = new ServerSettings();
        _configuration.GetSection(ServerSettings.SettingName).Bind(serverOptions);
        //Register this client to the requester client.
        notificationClient.RegisterAuctionConclusionNotification(new SubscribeMessage() { Client = serverOptions.Name });
        notificationClient.RegisterAuctionNotification(new SubscribeMessage() { Client = serverOptions.Name });
        notificationClient.RegisterBidNotification(new SubscribeMessage() { Client = serverOptions.Name });


        var serverClient = new AuctionClientService.AuctionClientServiceClient(channel);
        serverClient.RegisterNewClient(new RegisterMessage() { Client = serverOptions.Name, ClientUrl = $"{serverOptions.Url}" });

        _clientConnections.AddOrUpdate(clientId, s => (notificationClient, serverClient), (k, v) => v = (notificationClient, serverClient));
        return true;
    }
    public void Dispose()
    {

    }
}