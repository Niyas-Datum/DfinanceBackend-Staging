using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.WareHouse
{
    public class LocationTypeView
    {
        public int ID { get; set; }
        public string LocationType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
    }
}
