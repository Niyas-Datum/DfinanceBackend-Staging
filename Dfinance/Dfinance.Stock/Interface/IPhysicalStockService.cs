using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Stock.Interface
{
    public interface IPhysicalStockService
    {
        CommonResponse SavePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId);
        CommonResponse UpdatePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId);
        CommonResponse DeletePhystock(int TransId, int pageId, string Msg);
        CommonResponse CancelPhysicalStock(int transId, string reason);
    }
}
