using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IAccountReconciliationService
    {
        CommonResponse AccountPopUp();
        CommonResponse UpdateAccountReconcilation(int TranEntryId, DateTime BankDate);
        CommonResponse FillAccountReconcilation(DateOnly FromDate, DateOnly ToDate, int? AccountID);
    }
}
