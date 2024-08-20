using Azure;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Masters.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dfinance.Finance.Masters
{
    public class AccountSortOrderService : IAccountSortOrder
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountSortOrderService> _logger;
        public AccountSortOrderService(DFCoreContext context, IAuthService authService, ILogger<AccountSortOrderService> logger)
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
        public CommonResponse FillAccountSortOrder(int pageId)

        {
            try
            {
                //int branchId = _authService.GetBranchId().Value;

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec AccountsSp  @Criteria='AccountsForSorting'";
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
                    {
                        return CommonResponse.NoContent("No Data");
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
      

        public CommonResponse UpdateAccountSortOrder(AccountSortOrderDto accountSortOrderDto)
        {
            try
            {
                // Define the criteria
                string criteria = "UpdateGroupOrder";

                // Execute the stored procedure with the given parameters
                var result = _context.Database.ExecuteSqlRaw(
                    "EXEC AccountsSp @Criteria = {0}, @ID = {1}, @SortField = {2}",
                    criteria, accountSortOrderDto.Id, accountSortOrderDto.SortField);

                if (result == 0)
                {
                    return CommonResponse.Error("Update failed or no rows affected.");
                }

                return CommonResponse.Created("Updated Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}