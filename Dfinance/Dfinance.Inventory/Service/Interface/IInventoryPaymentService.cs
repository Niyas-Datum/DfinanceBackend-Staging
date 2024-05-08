using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface IInventoryPaymentService
    {
        CommonResponse FillTax();
        CommonResponse FillAddCharge();
        CommonResponse FillCash();
        CommonResponse FillCard();
        CommonResponse FillEpay();
        CommonResponse FillAdvance(int AccountId, string Drcr, DateTime? date);
        CommonResponse FillCheque();
        CommonResponse FillBankName();
        CommonResponse SaveCheque(ChequesDto chequeDto, int VEId, int PartyId, string Status);
        CommonResponse SaveTransactionEntries(PurchaseDto purchaseDto, int pageId, int transactionId, int transPayId);
        CommonResponse SaveTransactionExpenses(List<AccountDetailsDto> accountDetailsDtos, int transactionId, string tranType);
        CommonResponse SaveTransCollections(TransactionEntries transactionEntries, int transactionId);
        CommonResponse SaveTransCollectionsAllocation(TransactionEntries transactionEntries, int transCollectionId, int vAllocId);
    }
}
