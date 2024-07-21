using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class FillCounters
    {
        public int? ID {  get; set; }
        public string? MachineName { get; set; }
        public string? CounterCode { get; set;}
        public string? CounterName { get; set;}
        public string? MachineIP { get; set; }
        public bool? Active { get; set; }
    }
    public class FillCountersById
    {
        public string? MachineName { get; set; }
        public string? CounterCode { get; set; }
        public string? CounterName { get; set; }
        public string? MachineIP { get; set; }
        public bool? Active { get; set; }
    }
    public class CounterDto : FillCounters
    {
        
    }
}
