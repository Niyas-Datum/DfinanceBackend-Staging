using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{

    public interface IInventoryTransactionService
    {
        //Emplo
        CommonResponse FillPayType();
        CommonResponse GetSalesman();
        CommonResponse GetAutoVoucherNo(int voucherid);
        CommonResponse GetReference(int voucherno, DateTime? date = null);
        CommonResponse SaveTransaction(InventoryTransactionDto transactionDto,int PageId,int VoucherId, string Status);
        CommonResponse SaveTransactionPayment(InventoryTransactionDto transactionDto, int TransId, string Status, int VoucherId);
        CommonResponse SaveTransReference(int transId, List<int?> referIds);
        CommonResponse UpdateTransReference(int? transId, List<int?> referIds);
        CommonResponse SaveVoucherAllocation(int transId, int transpayId, InvTransactionEntriesDto transactionAdvance);
        CommonResponse UpdateVoucherAllocation(int transId, int transpayId, InvTransactionEntriesDto transactionAdvance);
        CommonResponse DeleteTransactions(int transId);
        CommonResponse CancelTransaction(int transId,  string reason);
        CommonResponse FillVoucherType(int voucherId);
        CommonResponse FillImportItemList(int? transId, int? voucherId);

        CommonResponse FillReference(List<ReferenceDto> referenceDto);

        CommonResponse EntriesAmountValidation(int TransId);

        CommonResponse InventoryAmountValidation(int TransId);

        int GetPrimaryVoucherID(int voucherid);

        CommonResponse InventoryTransactions(InventoryTransactionsDto inventoryTransactionDto ,int? ModuleID);

    }
}
