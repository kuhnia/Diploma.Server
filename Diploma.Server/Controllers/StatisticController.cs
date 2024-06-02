using Diploma.Entities.Models;
using Diploma.Server.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Diploma.Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly ILogger<StatisticController> _logger;
        private readonly IStatisticService _statisticService;

        public StatisticController(
            ILogger<StatisticController> logger,
            IStatisticService statisticService)
        {
            _logger = logger;
            _statisticService = statisticService;
        }

        [HttpGet("GetAllStatistic")]
        public async Task<IActionResult> GetAllStatistic()
        {
            try
            {
                IEnumerable<CounterSnapshot> statistic = await _statisticService.GetAllStatistic();
                return statistic != null && statistic.Any() ? Ok(statistic) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
