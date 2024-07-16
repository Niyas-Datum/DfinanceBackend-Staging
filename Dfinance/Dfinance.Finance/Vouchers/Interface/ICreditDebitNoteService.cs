using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface ICreditDebitNoteService
    {
        CommonResponse FillParty(int voucherId);
        CommonResponse SaveCreditDebitNote(DebitCreditDto debitCreditDto, int PageId, int VoucherId);
        CommonResponse UpdateCreditDebitNote(DebitCreditDto debitCreditDto, int PageId, int VoucherId);
        CommonResponse DeleteDebitCreditNote(int TransId, int pageId);
        CommonResponse CancelDebitCreditNote(int transId, string reason);

    }
}
