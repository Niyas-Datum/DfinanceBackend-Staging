using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Item.Services.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services
{
    public class AccountConfigurationService:IAccountConfigurationService
    {
        private readonly DFCoreContext _context;
        private readonly ILogger<AccountConfigurationService> _logger;
        public AccountConfigurationService(DFCoreContext context, ILogger<AccountConfigurationService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public CommonResponse FillAccConfig()
        {
            var data = _context.AccountConfigView.FromSqlRaw("exec FIMaUniqueAccountsSP @Criteria='Fill'").ToList();
            return CommonResponse.Ok(data);
        }
        public CommonResponse SaveAccConfig(List<AccConfigDto> accConfig)
        {
            try
            {
                if (accConfig == null)
                    return CommonResponse.NoContent("No Data");
                string criteria = "UpdateFIMaUniqueAccounts";
                foreach (var acc in accConfig)
                {
                    _context.Database.ExecuteSqlRaw($"Exec FIMaUniqueAccountsSP @Criteria='{criteria}',@AccID={acc.Account.Id},@Keyword='{acc.Keyword}'");
                }

                return CommonResponse.Ok("Updated Successfully");
            }
            catch 
            {
                _logger.LogError("Failed to Update Account Configuration");
                return CommonResponse.Error("Failed to Update Account Configuration");
            }
        }
    }
}
