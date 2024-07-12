using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IDeliveryInService
    {
        CommonResponse SaveDeliveyIn(InventoryTransactionDto invTranseDto, int pageId, int voucherId);
        CommonResponse UpdateDeliveryIn(InventoryTransactionDto invTranseDto, int pageId, int voucherId);
        CommonResponse DeleteDeliveryIn(int TransId, int pageId);
        CommonResponse CancelDeliveryIn(int TransId, int pageId, string reason);
    }
}
