using AutoMapper;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthCore.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Inventory.Reports.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;

namespace Dfinance.Inventory.Reports
{
    public class InventoryApprovalService : IInventoryApproval
    {

        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<InventoryApprovalService> _logger;
        private readonly IHostEnvironment _environment;
        public InventoryApprovalService(DFCoreContext context, IAuthService authService, ILogger<InventoryApprovalService> logger, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _environment = hostEnvironment;
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

        public CommonResponse FillInventoryApproval(DateTime FromDate, DateTime ToDate, int BranchID, int ApprovalStatus,int pageId, int? ModeID = null, string? MachineName = null, int? VTypeID = null, int? Detailed = null, int? UserID = null, int? VoucherID = null, bool? AutoEntry = null, int? TransactionID = null)
        {
            try
            {

                int branchId = _authService.GetBranchId().Value;
                var moduleid = GetModuleIDPrivate( pageId).Data;
        
                    var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Construct the SQL command text
                cmd.CommandText = $@"
            EXEC TransactionApprovalSP 
    @DateFrom = '{FromDate:yyyy-MM-dd HH:mm:ss}', 
    @DateUpto = '{ToDate:yyyy-MM-dd HH:mm:ss}', 
    @BranchID = {branchId},
    @Criteria = 'FillApprovalStatus', 
    @ModuleID = {moduleid}, 
    @VTypeID = {(VTypeID.HasValue ? VTypeID.Value.ToString() : "NULL")}, 
    @ModeID = {(ModeID.HasValue ? ModeID.Value.ToString() : "NULL")}, 
    @MachineName = {(string.IsNullOrEmpty(MachineName) ? "NULL" : $"'{MachineName}'")},
    @Detailed = {(Detailed.HasValue ? Detailed.Value.ToString() : "NULL")},
    @UserID = {(UserID.HasValue ? UserID.Value.ToString() : "NULL")},
    @VoucherID = {(VoucherID.HasValue ? VoucherID.Value.ToString() : "NULL")},
    @AutoEntry = {(AutoEntry.HasValue ? (AutoEntry.Value ? "1" : "0") : "NULL")},
    @TransactionID = {(TransactionID.HasValue ? TransactionID.Value.ToString() : "NULL")};";
              

                _context.Database.GetDbConnection().Open();

                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);

                    if (tb.Rows.Count > 0)
                    {
                        var rows = new List<Dictionary<string, object>>();

                        foreach (DataRow dr in tb.Rows)
                        {
                            var row = new Dictionary<string, object>();
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filling the inventory approval");
                return CommonResponse.Error(ex.Message);
            }
        }
       
        private CommonResponse GetModuleIDPrivate(int pageId)
        {
            // Call the stored procedure and return the ModuleID

            string criteria = "GetModuleIDUSingPageID";
            var result = _context.MaPageMenus.Where(p=>p.Id == pageId).Select(p=>p.ModuleId).FirstOrDefault();
            return CommonResponse.Ok(result);
            
        }
    }
}


