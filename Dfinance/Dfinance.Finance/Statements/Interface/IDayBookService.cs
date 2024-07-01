using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Statements.Interface
{
    public interface IDayBookService
    {
        CommonResponse FillVoucherAndUser();
        CommonResponse FillDayBook(DayBookDto dayBookDto, int pageId);
    }
}
