using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class BatchEditDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PopUpDto? BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public PopUpDto? PartyId { get; set; }
        public PopUpDto? ItemId { get; set; }
    }
}
