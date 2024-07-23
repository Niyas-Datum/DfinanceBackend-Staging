using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto
{
    public class RestaurentDto
    {
        public int? Id { get; set; }
        //public string? VoucherNo { get; set; }//TransNo
        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime Date { get; set; }

       // public InvTransactionAdditionalDto? FiTransactionAdditional { get; set; }
        public List<InvTransItemDto> Items { get; set; }
    }
}
