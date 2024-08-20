using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseWithoutTaxService
    {
        CommonResponse GetData(int voucherId);
        CommonResponse SavePurchaseWithoutTax(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
        CommonResponse UpdatePurchaseWithoutTax(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
        CommonResponse SaveTransactions(PurchaseWithoutTaxDto dto, int pageId, int voucherId);
        CommonResponse SaveTransactionPayment(PurchaseWithoutTaxDto dto, int TransId, int voucherId);
        CommonResponse SaveTransactionEntries(PurchaseWithoutTaxDto dto, int pageId, int transactionId, int transPayId);
        CommonResponse SaveTransactionAdditional(AdditionalDto dto, int TransId, int voucherId);
    }
}
