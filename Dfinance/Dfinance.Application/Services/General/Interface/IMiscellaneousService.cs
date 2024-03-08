using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IMiscellaneousService
    {        
        CommonResponse GetPopup(string[] keys);
        CommonResponse GetDropDown(string[] keys);
    }
}
