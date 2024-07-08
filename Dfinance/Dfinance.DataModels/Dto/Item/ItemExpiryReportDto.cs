using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Item
{
    public class ItemExpiryReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DropdownDto Item {  get; set; }
        public DropdownDto Barcode { get; set; }
        public DropdownDto Origin { get; set; }
        public DropdownDto Brand { get; set;}
        public DropdownDto Commodity { get; set; }
        public DropdownDto Color { get; set; }
        public int? ExpiryDays {  get; set; }    

    }
}
