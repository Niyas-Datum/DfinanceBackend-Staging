using Dfinance.DataModels.Dto.Common;
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
    public class StockTransactionDto//dto for stock transfer,issue,receipt,request,write off
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; }
        public DropdownDto? FromBranch { get; set; }
        public DropdownDto? ToBranch { get; set; }
        public DropdownDto? FromWarehouse { get; set; }
        public DropdownDto? ToWarehouse { get; set; }
        public DateTime VoucherDate { get; set; }
        public string? Description { get; set; }
        public string? Terms { get; set; }
        public string? Reference { get; set; }
        public List<StockTransItemDto>? StockItems { get; set; }
        public List<ReferenceDto>? references { get; set; }
    }
    public class StockTransItemDto
    {
        public int ItemId {  get; set; }
        public int? TransactionId { get; set; }
        public string? ItemCode { get; set; }//popup
        public string? ItemName { get; set; }
        public UnitPopupDto? Unit { get; set; }//popup
        [DecimalValidation(4, ErrorMessage = "Quantity is not valid!!")]
        public decimal Qty { get; set; }
        [DecimalValidation(4, ErrorMessage = "Rate is not valid!!")]
        [NullValidation(ErrorMessage = "Rate must be greater than 0!!")]
        public decimal? Rate { get; set; }
        [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
        public decimal? StockQty { get; set; }
        public decimal? BasicQty { get; set; }
        public decimal Amount { get; set; }
        public int? Pcs { get; set; }
        public PopUpDto? SizeMaster { get; set; }
        public int? TaxTypeId { get; set; }
        public decimal? TaxPerc { get; set; }
        public decimal? TaxValue { get; set; }
        public int? TaxAccountId { get; set; }
        public List<InvUniqueItemDto> UniqueItems { get; set; }
    }
   
}
