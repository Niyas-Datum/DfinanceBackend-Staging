using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services.Interface
{
    public interface IBudgetRegisterService
    {
        CommonResponse FillBudgetPopup(int voucherId);
        CommonResponse FillBudgetReport(int pageId, string criteria);
    }
}
