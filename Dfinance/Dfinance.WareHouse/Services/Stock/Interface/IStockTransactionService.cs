using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services
{
    public interface IStockTransactionService
    {
        CommonResponse FillDamageWH();
        CommonResponse SaveStockTrans(StockTransactionDto stockTransDto, int voucherId, int pageId);
        CommonResponse UpdateStockTrans(StockTransactionDto stockTransDto, int voucherId, int pageId);
    }
}
