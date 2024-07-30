using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services
{
    public interface IStockTransService
    {
         CommonResponse SaveFiTransaction(StockTransDto transaction);
         CommonResponse DeleteTransactions(int transId, int pageId, string reason);
         CommonResponse CancelTransaction(int transId, int pageId, string reason);
        CommonResponse SaveTransAdditionals(StockTransAdditional transAdditional);
        CommonResponse SaveStockInvTransItems(StockTransDto transDto);
        CommonResponse UpdateInvTransItems(StockTransDto transDto);
    }
}
