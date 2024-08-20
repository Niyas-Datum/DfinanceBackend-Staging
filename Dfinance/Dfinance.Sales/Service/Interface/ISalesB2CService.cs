using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Sales.Service.Interface
{
    public interface ISalesB2CService
    {
        CommonResponse SaveSalesB2C(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
        CommonResponse UpdateSalesB2C(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
    }
}
