using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;


namespace Dfinance.Application.General.Services
{
    public class CostCategoryDropDownService:ICostCategoryDropDownService
    {
        private readonly DFCoreContext _context
            ;
        public CostCategoryDropDownService(DFCoreContext context)
        {
                _context = context;
        }
        public CommonResponse FillCostCategory()
        {
            try
            {
                string criteria = "FillCostCategory";
                var data = _context.SpFillCostCategoryDropDown.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();               
                return CommonResponse.Ok(data);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
