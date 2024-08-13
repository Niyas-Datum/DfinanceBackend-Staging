using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IItemReservationService
    {
        CommonResponse GetLoadData();
        CommonResponse FillMaster(int pageId, int? TransactionId = null);
        CommonResponse SaveItemReserv(ItemReservationDto ItemReservation, int PageId, int voucherId);
        CommonResponse UpdateItemReserv(ItemReservationDto ItemReservation, int PageId, int voucherId);
        CommonResponse FillById(int transId);
    }
}
