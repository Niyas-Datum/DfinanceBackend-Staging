using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services
{
    public interface IOpeningStockService
    {
        CommonResponse SaveOpeningStock(OpeningStockDto openingStockDto, int pageId, int voucherId);
        CommonResponse UpdateOpeningStock(OpeningStockDto openingStockDto, int pageId, int voucherId);
    }
}
