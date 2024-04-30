using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Application.LabelAndGridSettings
{
    public class LabelAndGridSettings : ILabelAndGridSettings
    {
        
        private readonly IAuthService _authService;
        private readonly DFCoreContext _context;
        public LabelAndGridSettings(IAuthService authService,DFCoreContext dFCoreContext)
        {
           
            _authService = authService;
            _context = dFCoreContext;
        }
        public CommonResponse FillGridSettings()
        {
            try
            {
                int BranchId = _authService.GetBranchId().Value;
                string criteria = "FillGridSettings";
                var data = _context.FormGridView.FromSqlRaw($"Exec LabelSettingsSP @Criteria='{criteria}'").ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillFormLabelSettings()
        {
            try
            {
                int BranchId = _authService.GetBranchId().Value;
                string criteria = "FillFormLabelSettings";
                var data = _context.FormLabelView.FromSqlRaw($"Exec LabelSettingsSP @Criteria='{criteria}'").ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
