using AuctioneerService.DbContext;
using AuctioneerService.Models;
using AuctioneerService.Repository;
using Grpc.Core;

namespace AuctioneerService.Services;

public class ClientRegistration : ClientRegisterService.ClientRegisterServiceBase
{
    private readonly ILogger<ClientRegistration> _logger;
    private readonly ClientContext _context;
    private readonly ClientRepository _repository;
    public ClientRegistration(ILogger<ClientRegistration> logger)
    {
        _logger = logger;
    }

    public override async Task<RegisterReply> RegisterClient(RegisterRequest request, ServerCallContext context)
    {
        
        _logger.LogInformation($"{request.Name} want to register ");
        var entity = await _repository.Add(new Client() { Name = request.Name, Status = true, Url = request.Url });
        var clients = await _repository.GetAll();
        if(!clients.Any( c=> c.Name == entity.Name)) clients.Add(entity);
        
        List<ClientReturn> clientResponses = clients
            .Select(client => new ClientReturn
            {
                Id = client.Id,
                Name = client.Name,
                Url = client.Url,
                Status = client.Status
            })
            .ToList();

        RegisterReply replyModel = new RegisterReply();
        replyModel.Clients.AddRange(clientResponses);
        replyModel.Message = $"{entity.Name} has succesfully added";
        return replyModel;
    }
}