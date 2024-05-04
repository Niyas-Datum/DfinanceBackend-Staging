using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.General
{
    public class UserTrackDto
    {
        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUpto { get; set; }
        public string? UserName { get; set; }
        public string? TableName { get; set; }
        public string? ModuleName { get; set; }
        public string? MachineName { get; set; }
        public decimal? Amount { get; set; }
        public long? RowID { get; set; }
        public string? Reference { get; set; }
        public string? Reason { get; set; }
        public DropdownDto? Action { get; set; }
    }
}
