using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Sales.Service.Interface
{
    public interface ISalesPosService
    {
        CommonResponse SaveSalesPos(InventoryTransactionDto salesDto, int PageId, int voucherId);

    }
}
