using ViennaRocks.Api.Models;

namespace ViennaRocks.Api.BusinessLogic.Contract;

public interface ITicketMasterClient
{
    Task<IReadOnlyCollection<Concert>> GetConcerts();
}

