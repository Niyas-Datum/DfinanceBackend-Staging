using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Sales.Service.Interface
{
    public interface ISalesB2BService
    {
        CommonResponse SaveSalesB2B(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
        CommonResponse UpdateSalesB2B(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
    }
}
