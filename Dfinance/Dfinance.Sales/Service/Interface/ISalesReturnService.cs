using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Sales
{
    public interface ISalesReturnService
    {
        CommonResponse SaveSalesReturn(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse UpdateSalesReturn(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse DeleteSalesReturn(int TransId,int pageId);
    }
}
