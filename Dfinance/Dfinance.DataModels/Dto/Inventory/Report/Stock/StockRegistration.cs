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
}
