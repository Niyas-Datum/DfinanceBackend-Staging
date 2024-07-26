using Dfinance.DataModels.Dto.Inventory.Stock;
using Dfinance.Shared.Domain;

namespace Dfinance.Stock.Services.Interface
{
    public interface IOpeningStockService
    {
        CommonResponse SaveOpeningStock(OpeningStockDto openingStockDto, int pageId, int voucherId);
        CommonResponse UpdateOpeningStock(OpeningStockDto openingStockDto, int pageId, int voucherId);
    }
}
