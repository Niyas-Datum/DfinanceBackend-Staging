using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services
{
    public interface IStockRtnAdjustService
    {
        CommonResponse SaveStockRtnAdjust(StockRtnAdjustDto stockAdjustmentDto, int voucherId, int pageId);
        CommonResponse UpdateStockRtnAdjust(StockRtnAdjustDto stockAdjustmentDto, int voucherId, int pageId);
        CommonResponse DeleteTransactions(int transId, int pageId, string reason);
        CommonResponse CancelTransaction(int transId, int pageId, string reason);
    }
}
