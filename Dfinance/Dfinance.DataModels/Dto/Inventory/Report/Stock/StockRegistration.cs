using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class StockRegistration
    {
        public Object? LocationID { get; set; }
        public Object? ItemID { get; set; }
        public Object? BranchID { get; set; }
        public DateTime ToDate { get; set; }
        public int? IsItemwise { get; set; }
        public Object? Barcode { get; set; }
        public Object? CommodityID { get; set; } = null;
        public Object? OriginID { get; set; } = null;
        public Object? BrandID { get; set; } = null;
        public Object? ColorID { get; set; } 
        public Object? AccountID { get; set; } 
        public String? BatchNo { get; set; } 
        public Object? SupplierID { get; set; }
        public Object? CustomerID { get; set; } 
        public Object? CategoryTypeID { get; set; }
    }
    public class ItemDetailsRpt
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public PopUpDto? ItemId { get; set; }
        public DropdownDto? BranchId { get; set; }
    }
    public class ItemStockRegisterRpt
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public PopUpDto? ItemID { get; set; }
        public DropdownDto? BranchID { get; set; }
        public String? BatchNo { get; set; }
    }
    public class WarehouseStockRegRpt
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DropdownDto? LocationId { get; set; }
    }
    public class CommodityStockRegRpt
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DropdownDto? LocationId { get; set; }
        //public DropdownDto? BranchId { get; set; }
        public DropdownDto? TyoeOfWood { get; set; }
    }
    public class StockReceiptIssueRpt
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DropdownDto? LocationId { get; set; }
        public DropdownDto? FromBranch { get; set; }
        public DropdownDto? ToBranch { get; set; }
        public PopUpDto? ItemId { get; set; }
        public DropdownDto? VType { get; set; }
    }
}
