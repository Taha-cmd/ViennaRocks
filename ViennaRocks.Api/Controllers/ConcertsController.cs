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

    [HttpGet]
    [Route("/concerts")]
    public async Task<IReadOnlyCollection<Concert>> GetConcerts()
    {
        return await _ticketMasterClient.GetConcerts();
    }
}