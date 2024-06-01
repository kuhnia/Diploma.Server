using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Entities.Models
{
    public class CounterSnapshot
    {
        public Guid Id { get; set; }
        public double CurrentValue { get; set; }
        public double DifferenceWithPreviousValue { get; set; }
    }
}
