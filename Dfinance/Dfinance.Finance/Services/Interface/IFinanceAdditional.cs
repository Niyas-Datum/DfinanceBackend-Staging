using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Finance.Services.Interface
{
    public interface IFinanceAdditional
    {
        //CommonResponse FillTransactionAdditionals(int transactionId);
        //CommonResponse GetTransPortationType();
        //CommonResponse GetSalesArea();
        //CommonResponse PopupVechicleNo();
        // CommonResponse PopupDelivaryLocations(int salesManId);
        CommonResponse SaveTransactionAdditional(PaymentVoucherDto paymentVoucherDto, int TransId, int voucherId);
        // CommonResponse UpdateTransactionAdditional(InvTransactionAdditionalDto fiTransactionAdditionalDto, int TransId);
       // CommonResponse DeleteTransactionAdditional(int Id);
    }
}
