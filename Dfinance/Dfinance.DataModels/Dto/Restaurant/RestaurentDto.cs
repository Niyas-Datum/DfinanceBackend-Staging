using Dfinance.DataModels.Dto.Inventory;
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
        public List<InvRestTransItemDto> Items { get; set; }
    }
    public class InvRestTransItemDto
    {
        public int? TransactionId { get; set; }
        public int ItemId { get; set; }
        public string? ItemCode { get; set; }//popup
        public string? ItemName { get; set; }
        public UnitPopupDto? Unit { get; set; }//popup
        public string? BarCode { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? ArabicName { get; set; }
        public int? TaxTypeID { get; set; }
        public decimal? TaxPerc { get; set; }
        public int? CategoryID { get; set; }
        public int? CommodityID { get; set; }
        public int? TaxAccountID { get; set; }
        public string? ImagePath { get; set; }
        public string? ShipMark { get; set; }
        public string? PaintMark { get; set; }
        [DecimalValidation(4, ErrorMessage = "Quantity is not valid!!")]
        public decimal Qty { get; set; }
        [DecimalValidation(4, ErrorMessage = "Rate is not valid!!")]
        [NullValidation(ErrorMessage = "Rate must be greater than 0!!")]
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? DiscountPerc { get; set; }
    }
}
