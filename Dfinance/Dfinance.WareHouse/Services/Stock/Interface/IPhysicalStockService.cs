using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services
{
    public interface IPhysicalStockService
    {
        CommonResponse SavePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId);
        CommonResponse UpdatePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId);
        CommonResponse DeletePhystock(int TransId, int pageId, string Msg);
        CommonResponse CancelPhysicalStock(int transId, string reason);
    }
}
