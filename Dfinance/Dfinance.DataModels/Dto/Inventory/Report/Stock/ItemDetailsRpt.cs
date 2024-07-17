using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class ItemDetailsRpt
    {
        public DateTime FromDate {  get; set; }
        public DateTime ToDate { get; set; }
        public PopUpDto? ItemId { get; set; }
        public DropdownDto? BranchId { get; set; }
    }
}
