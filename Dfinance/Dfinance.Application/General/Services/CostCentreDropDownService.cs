using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.General.Services
{
    public class CostCentreDropDownService:ICostCentreDropDownService
    {
        private readonly DFCoreContext _context;
        public CostCentreDropDownService(DFCoreContext context)
        {
                _context = context;
        }
        public CommonResponse FillCostCentre()
        {
            try
            {
                string criteria = "FillCostCenterGroup";
                var data=_context.SpCostCentreDropDown.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();               
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
