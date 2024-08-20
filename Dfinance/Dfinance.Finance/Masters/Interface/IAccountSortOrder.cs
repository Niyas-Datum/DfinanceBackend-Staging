using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Masters.Interface
{
    public  interface IAccountSortOrder
    {
        CommonResponse FillAccountSortOrder(int pageId);
        CommonResponse UpdateAccountSortOrder(AccountSortOrderDto accountSortOrderDto);
    }
}
