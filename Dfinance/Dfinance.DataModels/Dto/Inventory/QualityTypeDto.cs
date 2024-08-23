using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class QualityTypeDto
    {
        public int? Id {  get; set; }
        public Popupdto? QualityinCarat {  get; set; }
        public decimal? Rate { get; set; }

    }
}
