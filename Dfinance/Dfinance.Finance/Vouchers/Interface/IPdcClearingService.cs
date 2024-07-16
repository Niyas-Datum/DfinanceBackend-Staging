using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IPdcClearingService
    {
        CommonResponse SavePdcClearing(PdcClearingDto PDCDto, int PageId, int voucherId);
        CommonResponse FillChequeDetails(int BankId);
        //CommonResponse DeletePdcClearing(PdcClearingDto pdcDto, int pageId, int voucherId, bool? cancel = false);
        CommonResponse UpdatePDCclearing(PdcClearingDto pdcClearingDto, int PageId, int voucherId);
    }
}
