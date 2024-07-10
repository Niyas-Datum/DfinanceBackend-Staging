using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services.Interface
{
    public interface IAccountConfigurationService
    {
        CommonResponse FillAccConfig();
        CommonResponse SaveAccConfig(List<AccConfigDto> accConfig);
    }
}
