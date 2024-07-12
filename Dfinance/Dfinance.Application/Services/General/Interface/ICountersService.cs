using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface ICountersService
    {
        CommonResponse FillMaster();
        CommonResponse FillCountersById(int Id);
        CommonResponse GetNameandIp();
        CommonResponse SaveCounters(CounterDto counterDto);
        CommonResponse DeleteCounter(int Id, int PageId);
        CommonResponse UpdateCounters(CounterDto counterDto, int PageId);
    }
}
