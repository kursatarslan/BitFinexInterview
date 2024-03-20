using AuctionClient1.Configs;
using AuctionClient1.Interfaces;
using AuctionClient1.Managers;
using AuctionClient1.Services;
using AuctioneerService.Services;
using Grpc.Net.Client;
using RegisterRequest = AuctioneerService.Services.RegisterRequest;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

string registerUrl = configuration["RegisterUrl"]; 

using var channel = GrpcChannel.ForAddress(registerUrl);

var client = new ClientRegisterService.ClientRegisterServiceClient(channel);
var settings = builder.Configuration.Get<ServerSettings>();

var request = new RegisterRequest
{
    Name = settings.Name,
    Url = settings.Url
};

try
{
    var reply = await client.RegisterClientAsync(request);
    
    Console.WriteLine($"Message: {reply.Message}");
    foreach (var clientReturn in reply.Clients)
    {
        Console.WriteLine($"ID: {clientReturn.Id}, Name: {clientReturn.Name}, Url: {clientReturn.Url}, Status: {clientReturn.Status}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"RpcException: {ex.Message}");
}

builder.Services.AddSingleton<IAuctionManager, AuctionManager>();
builder.Services.AddSingleton<INotificationManager, NotificationManager>();
builder.Services.AddSingleton<IAuctionClientManager, AuctionClientManager>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapGrpcService<GenericAuctioncCientService>();
app.MapGrpcService<GenericAuctionServerService>();
app.MapGrpcService<GenericAuctionClientServerService>();

app.MapControllers();


app.Run();

