using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ViennaRocks.Api.BusinessLogic.Contract;
using ViennaRocks.Api.Models;

namespace ViennaRocks.Api.BusinessLogic;

public class TicketMasterClient : ITicketMasterClient
{
    private readonly HttpClient _httpClient;
    private readonly ICache _cache;
    private readonly ILogger<TicketMasterClient> _logger;
    private readonly IConfiguration _config;
    private readonly string _apiKey;
    private readonly string _apiBaseUrl;

    private const string CacheKey = "concerts";

    public TicketMasterClient(HttpClient httpClient, ICache cache, ILogger<TicketMasterClient> logger, IConfiguration config)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
        _config = config;
        _apiKey = Environment.GetEnvironmentVariable("TicketMasterApiKey") ?? throw new NullReferenceException("api key not found");
        _apiBaseUrl = _config["TicketMasterApiBaseUrl"] ?? throw new NullReferenceException("api base url not found");
    }

    public async Task<IReadOnlyCollection<Concert>> GetConcerts()
    {
        var cachedValue = _cache.Get<IReadOnlyCollection<Concert>>(CacheKey);

        if (cachedValue is not null)
        {
            _logger.LogInformation("Cache hit!");
            return cachedValue;
        }

        // KZFzniwnSyZfZ7v7nJ segment id music
        // KnvZfZ7vAvt genre id metal
        // KnvZfZ7vAeA gerne id rock

        string uri = $"{_apiBaseUrl}/discovery/v2/events?apikey={_apiKey}&locale=*&city=Vienna&countryCode=AT&&segmentId=KZFzniwnSyZfZ7v7nJ&genreId=KnvZfZ7vAvt,KnvZfZ7vAeA";

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(uri),
        };

        var response = await _httpClient.SendAsync(request);

        //https://www.newtonsoft.com/json/help/html/QueryJsonLinq.htm
        var content = JObject.Parse(await response.Content.ReadAsStringAsync());

        var results = content["_embedded"]?["events"]?.Select<JToken, Concert>(eventObject =>
            new Concert()
            {
                Name = eventObject["name"]!.ToObject<string>()!,
                Url = eventObject["url"]!.ToObject<string>()!,
                Venue = eventObject["_embedded"]["venues"].First["name"].ToObject<string>()

            }
        )!.ToArray()!;

        return _cache.Set(CacheKey, results, TimeSpan.FromHours(24));
    }
}

