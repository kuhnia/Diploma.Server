using Diploma.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Server.Business.Interfaces
{
    public interface IStatisticService
    {
        Task<bool> CreateStatisticRecord(CounterSnapshot counterSnapshot);
        Task<IEnumerable<CounterSnapshot>> GetAllStatistic();
    }
}
