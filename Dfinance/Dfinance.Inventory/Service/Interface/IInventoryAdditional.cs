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
        CommonResponse SaveTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto, int TransId);
        CommonResponse UpdateTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto, int TransId);
        CommonResponse DeleteTransactionAdditional(int Id);
    }
}
