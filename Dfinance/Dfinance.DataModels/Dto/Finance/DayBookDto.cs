using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Finance
{
    public class DayBookDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUpto { get; set; }
        public DropdownDto? VoucherType { get; set; }
        public DropdownDto? Branch { get; set; }
        public PopUpDto? User { get; set; }
        public bool? Detailed { get; set; }
        public bool? Posted { get; set; }
    }
}
