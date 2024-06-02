using Diploma.UI.Entities.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Diploma.UI.Services.Http
{
    public class StatisticHttpService : HttpServiceBase
    {
        private readonly ILogger<StatisticHttpService> _logger;
        protected override string _apiControllerName { get; set; }

        public StatisticHttpService(IConfiguration appSettings, ILogger<StatisticHttpService> logger) : base(appSettings)
        {
            _logger = logger;
            _apiControllerName = "Statistic";
        }


        public async Task<IEnumerable<CounterSnapshot>> GetAllStatistic()
        {
            var httpResponse = await _client.GetAsync(Url("GetAllStatistic"));
            await EnsureSuccessStatusCodeAsync(httpResponse);
            return await httpResponse.Content.ReadAsAsync<IEnumerable<CounterSnapshot>>();
        }
    }
}
