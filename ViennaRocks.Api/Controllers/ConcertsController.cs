using Microsoft.AspNetCore.Mvc;
using ViennaRocks.Api.BusinessLogic.Contract;
using ViennaRocks.Api.Models;

namespace ViennaRocks.Api.Controllers;

[ApiController]
public class ConcertsController : Controller
{
    private readonly ITicketMasterClient _ticketMasterClient;

    public ConcertsController(ITicketMasterClient ticketMasterClient)
    {
        _ticketMasterClient = ticketMasterClient;
    }

    // AoLgBamyDUK2uP1WGmIwDPhpIv5pYR04

    [HttpGet]
    [Route("/concerts")]
    public async Task<IReadOnlyCollection<Concert>> GetConcerts()
    {
        var x = await _ticketMasterClient.GetConcerts();
        return x;
    }
}