using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Finance.Services
{
    public class CustomerRegisterService : ICustomerRegister
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<CustomerRegisterService> _logger;
        public CustomerRegisterService(DFCoreContext context, IAuthService authService, ILogger<CustomerRegisterService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
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
       

        public CommonResponse FillCustomerRegister(string PartyType, int? AccountID, int? PartyCategory, int pageId)
        
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                int mode = 1;
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec spPartyRegister @Mode={mode}, @Party='{PartyType}', @BranchID={branchId}, @AccountID={(AccountID.HasValue ? AccountID.Value.ToString() : "NULL")}, @PartyCategory={(PartyCategory.HasValue ? PartyCategory.Value.ToString() : "NULL")}";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count > 0)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                        foreach (DataRow dr in tb.Rows)
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                            rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);
                    }
                    else
                    {                        return CommonResponse.NoContent("No Data");
                    }
                }
            
                 return CommonResponse.NotFound();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
            

        }
    }
}
