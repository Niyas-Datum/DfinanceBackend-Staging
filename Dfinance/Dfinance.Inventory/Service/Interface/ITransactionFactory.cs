using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface ITransactionFactory
    {

        CommonResponse GetSalesman();
        CommonResponse SaveTransaction(PurchaseDto transactionDto);
    }
}
