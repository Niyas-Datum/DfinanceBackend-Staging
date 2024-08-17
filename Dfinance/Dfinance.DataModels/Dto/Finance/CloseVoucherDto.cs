using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Finance
{
    public class CloseVoucherDto
    {
        public PopUpDto? AccountId { get; set; }
        public PopUpDto? VTypeId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUpto { get; set; }
        public string? VNo { get; set; }
    }
}
