using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.General.Services.Interface
{
    public interface IStatusDropDownService
    {
        CommonResponse FillStatus();
    }
}
