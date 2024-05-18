using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface IInventoryAdditional
    {
        CommonResponse FillTransactionAdditionals(int transactionId);
        CommonResponse GetTransPortationType();
        CommonResponse GetSalesArea();
        CommonResponse PopupVechicleNo();
        CommonResponse PopupDelivaryLocations(int salesManId);
        CommonResponse SaveTransactionAdditional(InvTransactionAdditionalDto fiTransactionAdditionalDto, int TransId, int voucherId);
       // CommonResponse UpdateTransactionAdditional(InvTransactionAdditionalDto fiTransactionAdditionalDto, int TransId);
        CommonResponse DeleteTransactionAdditional(int Id);
    }
}
