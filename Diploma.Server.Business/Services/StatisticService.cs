using Diploma.Entities.Models;
using Diploma.Server.Business.Interfaces;
using Diploma.Server.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Server.Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly ILogger<StatisticService> _logger;

        private readonly StatisticDataController _statisticDataController;

        public StatisticService(ILogger<StatisticService> logger, StatisticDataController statisticDataController)
        {
            _logger = logger;
            _statisticDataController = statisticDataController;
        }

        public async Task<bool> CreateStatisticRecord(CounterSnapshot counterSnapshot)
        {
            try
            {
                //var res = await _statisticDataController.CreateStatisticRecord(counterSnapshot);
                return true;// res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<CounterSnapshot>> GetAllStatistic()
        {
            try
            {
                var res = await _statisticDataController.GetAllStatistic();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
