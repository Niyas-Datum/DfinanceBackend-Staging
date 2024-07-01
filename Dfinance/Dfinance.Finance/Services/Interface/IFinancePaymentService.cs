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
    public interface IFinancePaymentService
    {
        
        CommonResponse FillCash();
        CommonResponse FillCard();
        CommonResponse FillEpay();
        CommonResponse FillAdvance(int AccountId, string Drcr, DateTime? date);
        CommonResponse FillCheque();
        CommonResponse FillBankName();
        CommonResponse SaveCheque(ChequeDto chequeDto, int VEId, int? PartyId);
        CommonResponse SaveTransactionEntries(FinanceTransactionDto paymentVoucherDto, int pageId, int transactionId, int transPayId);
       
       CommonResponse UpdateTransactionEntries(FinanceTransactionDto paymentVoucherDto, int pageId, int transactionId, int transPayId);
    }
}
