using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dfinance.Inventory.Reports.Interface
{
    public interface IInventoryRegister
    {
        CommonResponse FillInventoryRegister(DateTime DateFrom, DateTime DateUpto, int BranchID, int? BasicVTypeID, int? VTypeID = null,
    int? AccountID = null, int? SalesManID = null, int? ItemID = null, int? BrandID = null, int? OriginID = null,
    int? ColorID = null, int? CommodityID = null, int? LocationID = null, string Manufacturer = "",
    string GroupBy = "", int? AreaID = null, string VoucherNo = "", int? pageId = null);

        CommonResponse PopManufacturer(int type);
    }
}
