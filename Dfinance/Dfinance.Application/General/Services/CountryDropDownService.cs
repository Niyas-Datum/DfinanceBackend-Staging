using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Infrastructure;
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
                // Execute the stored procedure
                var data = _dFCoreContext.SpCountryDropDown.FromSqlRaw("Exec DropDownListSP @Criteria = 'FillMaMisc',@StrParam='Country'").ToList();
                // Extract Id and Value from the result of the stored procedure
                var macountry = data.Select(item => new SpCountryDropDown
                {
                    ID = item.ID,
                    Value = item.Value
                }).ToList();
                
                return CommonResponse.Ok(macountry);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
            
        }
        
    }
}


