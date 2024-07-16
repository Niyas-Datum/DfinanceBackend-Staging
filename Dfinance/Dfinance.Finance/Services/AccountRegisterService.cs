using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services
{
    public class AccountRegisterService : IAccountRegister
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountRegisterService> _logger;
        public AccountRegisterService(DFCoreContext context, IAuthService authService, ILogger<AccountRegisterService> logger)
        {
            _context = context;
            _authService = authService;
            _logger =  _logger;
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        public CommonResponse FillAccountRegister(int pageId)
             {
            try
            {
        int branchId = _authService.GetBranchId().Value;
        
        var cmd = _context.Database.GetDbConnection().CreateCommand();
        cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"EXEC AccountsSp @Criteria='FillAccountDetails', @BranchID='{branchId}'";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tb = new DataTable();
                 tb.Load(reader);

                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                  Dictionary<string, object> row;
                        foreach (DataRow dr in tb.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                           rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);

                    }
                }
                return CommonResponse.NotFound();

             
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }

        }
}
}
