using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class ItemReservationDto
    {
        public int? Id { get; set; }

        public string? VoucherNo { get; set; }//TransNo

        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime Date { get; set; }
        public PopUpDto Party { get; set; }
        public PopUpDto Project { get; set; }//POPUP
        public string? Description { get; set; }
        public string? PartyInfo { get; set; }
        public DropDownDtoName? Warehouse { get; set; }
        public List<InvTransItemReservDto> Items { get; set; }
    }
    public class  InvTransItemReservDto
    {
        public int? TransactionId { get; set; }
        public int ItemId { get; set; }
        public string? ItemCode { get; set; }//popup
        public string? ItemName { get; set; }
        public UnitPopupDto? Unit { get; set; }//popup

        [DecimalValidation(4, ErrorMessage = "Quantity is not valid!!")]
        public decimal Qty { get; set; }
    }
}
