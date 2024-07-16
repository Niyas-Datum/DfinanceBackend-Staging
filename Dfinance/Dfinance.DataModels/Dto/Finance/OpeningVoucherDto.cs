using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Finance
{
    public class OpeningVoucherDto
    {
        public int? Id { get; set; }
        [NullValidation(ErrorMessage = "Voucher Number is Mandatory!!")]
        public string? VoucherNo { get; set; }
        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime? VoucherDate { get; set; }
        public string? Narration { get; set; }
        public List<AccountDetails>? AccountDetails { get; set; }
    }
}
