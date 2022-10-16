using System.Dynamic;

namespace ViennaRocks.Api.BusinessLogic.Contract;

public interface ICache
{
    T Get<T>(string key) where T : class;
    T Set<T>(string key, T value, TimeSpan timeout) where T : class;

}

