using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services
{
    public interface IPhyOpenStockService
    {
        CommonResponse SavePhyOpenStock(PhyOpenStockDto phyOpenStockDto, int pageId, int voucherId);
        CommonResponse UpdatePhyOpenStock(PhyOpenStockDto phyOpenStockDto, int pageId, int voucherId);
    }
}
