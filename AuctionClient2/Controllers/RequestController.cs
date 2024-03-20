using AuctionClient3.Configs;
using AuctionClient3.Models;
using AuctionClient3.Repository;
using AuctionClient3.DTOs;
using AuctionClient3.Managers;

namespace AuctionClient3.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RequestController  : ControllerBase
{
    private readonly IAuctionManager _auctionManager;
    private readonly ILogger<RequestController> _logger;
    private readonly IConfiguration Configuration;


    public RequestController(ILogger<RequestController> logger,IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
    }
    
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> StartAuction(PictureDTO picture)
    {

        var result = await _auctionManager.Add(picture);

        //if (result.Succeeded)
        {
            return Ok(result);
        }
    }
    
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Bid(BidDTO bid)
    {
        var result = await _auctionManager.Bid(bid);
        //if (result.Succeeded)
        {
            return Ok(result);
        }
        //else
        {
            return BadRequest();
        }
    }
    
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Finalize(AuctionBTO auc)
    {
        
        var result = await _auctionManager.Finalize(auc);
        //if (result.Succeeded)
        {
            return Ok();
        }
        //else
        {
            return BadRequest();
        }
    }
}