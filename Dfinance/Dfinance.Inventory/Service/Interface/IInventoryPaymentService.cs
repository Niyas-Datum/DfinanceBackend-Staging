using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface IInventoryPaymentService
    {
        CommonResponse FillTax();
        CommonResponse FillAddCharge();
        CommonResponse FillPaymentDetails(string description);
        //CommonResponse FillCard();
        //CommonResponse FillEpay();
        CommonResponse FillAdvance(int AccountId, string Drcr, DateTime? date);
        CommonResponse FillCheque();
        CommonResponse FillBankName();
        CommonResponse SaveCheque(InvChequesDto chequeDto, int VEId, int? PartyId);
        CommonResponse SaveTransactionEntries(InventoryTransactionDto purchaseDto, int pageId, int transactionId, int transPayId);
        CommonResponse SaveTransactionExpenses(List<InvAccountDetailsDto> accountDetailsDtos, int transactionId, string tranType);
        CommonResponse UpdateTransactionExpenses(List<InvAccountDetailsDto> accountDetailsDtos, int transactionId, string tranType);
        CommonResponse SaveTransCollections(InvTransactionEntriesDto transactionEntries, int transactionId);
        CommonResponse SaveTransCollectionsAllocation(InvTransactionEntriesDto transactionEntries, int transCollectionId, int vAllocId);
    }
}
