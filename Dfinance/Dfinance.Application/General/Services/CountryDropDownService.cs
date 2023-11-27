using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Application.General.Services
{

    public class CountryDropDownService : ICountryDropDownService
    {
        private readonly DFCoreContext _dFCoreContext;
        public CountryDropDownService(DFCoreContext dFCoreContext)
        {

            _dFCoreContext = dFCoreContext;
        }
        public CommonResponse FillCountry()
        {
            try
            {               
                var data = _dFCoreContext.SpDropDownCommon1.FromSqlRaw("Exec DropDownListSP @Criteria = 'FillMaMisc',@StrParam='Country'").ToList();
                
                
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
            
        }
        
    }
}


