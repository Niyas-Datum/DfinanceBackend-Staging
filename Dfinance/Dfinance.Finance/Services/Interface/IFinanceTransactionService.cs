using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services.Interface
{
    public interface IFinanceTransactionService
    {
        //CommonResponse GetSalesman();
        CommonResponse GetAutoVoucherNo(int voucherid);
        // CommonResponse GetReference(int voucherno, DateTime? date = null);
        CommonResponse SaveTransaction(FinanceTransactionDto paymentVoucherDto, int PageId, int VoucherId, string Status);
        CommonResponse SaveTransactionPayment(InventoryTransactionDto transactionDto, int TransId, string Status, int VoucherId);
        //CommonResponse SaveTransReference(int transId, List<int?> referIds);
        //CommonResponse UpdateTransReference(int? transId, List<int?> referIds);
        CommonResponse SaveVoucherAllocation(int transId, int transpayId, FinanceTransactionDto paymentVoucherDto);
        CommonResponse UpdateVoucherAllocation(int transId, int transpayId, FinanceTransactionDto paymentVoucherDto);
        //CommonResponse DeletePurchase(int TransId);

        //CommonResponse FillVoucherType(int voucherId);
        //CommonResponse FillImportItemList(int? transId, int? voucherId);

        //CommonResponse FillReference(List<ReferenceDto> referenceDto);

        CommonResponse EntriesAmountValidation(int TransId);

        CommonResponse InventoryAmountValidation(int TransId);
    }
}
