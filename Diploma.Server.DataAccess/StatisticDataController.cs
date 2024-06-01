using Dapper;
using Diploma.Entities.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Server.DataAccess
{
    public class StatisticDataController : DataController
    {
        public StatisticDataController(IConfiguration configuration) : base(configuration, "IssueReports")
        {
        }

        public Task<bool> CreateStatisticRecord(CounterSnapshot counterSnapshot)
        {
            if (counterSnapshot == null)
                throw new ArgumentNullException(nameof(counterSnapshot));

            var parameters = new DynamicParameters();
            parameters.Add("@Id", counterSnapshot.Id);
            parameters.Add("@CurrentValue", counterSnapshot.CurrentValue);
            parameters.Add("@DifferenceWithPreviousValue", counterSnapshot.DifferenceWithPreviousValue);
            return PerformNonQuery(parameters, "Create");
        }

        public Task<IEnumerable<CounterSnapshot>> GetAllStatistic()
        {
            return GetManyAsync<CounterSnapshot>("GetAll");
        }
    }
}
