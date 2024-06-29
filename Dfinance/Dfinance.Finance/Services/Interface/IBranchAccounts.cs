using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IBranchAccounts
    {
        CommonResponse FillBranchAccounts();
        CommonResponse UpdateBranchAccounts(BranchAccountsDto branchAccountsDto);
    }
}
