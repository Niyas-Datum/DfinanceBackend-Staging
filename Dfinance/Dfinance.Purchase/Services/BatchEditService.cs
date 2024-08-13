using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Dfinance.Purchase.Services
{
    public class BatchEditService :IBatchEditService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<IBatchEditService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IUserTrackService _userTrackService;
        public BatchEditService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment,
            ILogger<IBatchEditService> logger, IUserTrackService userTrackService, IInventoryTransactionService transactionService) 
        { 
            _context = context;
            _authService=authService;
            _environment=hostEnvironment;
            _logger=logger;
            _transactionService=transactionService;
            _userTrackService=userTrackService;
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
        private CommonResponse GetItem()
        {
            var items=_context.ItemMaster.Where(im=>im.Active==true && im.IsGroup==false).Select(im=>new {im.ItemCode,im.ItemName,im.Unit,im.Id}).ToList();
            return CommonResponse.Ok(items);
        }
        private CommonResponse GetParties()
        {
            var parties = _context.FiMaAccounts.Where(a => a.IsGroup == false && a.Active == true && a.AccountCategory == 2).Select(a => new { AccountCode = a.Alias, AccountName = a.Name, a.Id }).ToList();
            return CommonResponse.Ok(parties);
        }
        private CommonResponse GetBatchDetails()
        {
            var branchId = _authService.GetBranchId();
            var batchNo=(from T in _context.FiTransaction
                         join TI  in _context.InvTransItems on T.Id equals TI.TransactionId
                         join V in _context.FiMaVouchers on T.VoucherId equals V.Id
                         join A in _context.FiMaAccounts on T.AccountId equals A.Id
                         join IM in _context.ItemMaster on TI.ItemId equals IM.Id
                         where T.CompanyId == branchId && T.Active==true && T.Posted==true && T.Cancelled==false && TI.InLocId!=null &&TI.BatchNo !=null
                         select TI.BatchNo).Distinct().ToList();
            return CommonResponse.Ok(batchNo);
        }
        public CommonResponse GetLoadData()
        {
            try
            {
                var items = GetItem().Data;
                var parties=GetParties().Data;
                var batch=GetBatchDetails().Data;
                return CommonResponse.Ok(new { Items = items , Parties=parties,BatchNos=batch});
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse BatchDetailsForUpdate(BatchEditDto batchEditDto)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;               
                var Items = batchEditDto.ItemId.Id != null ? batchEditDto.ItemId.Id.ToString() : "NULL";
                var party = batchEditDto.PartyId.Id != null ? batchEditDto.PartyId.Id.ToString() : "NULL";
                var batch = batchEditDto.BatchNo.Id != null ? batchEditDto.BatchNo.Id.ToString() : "NULL";
                //var UnitId = batchwiseStock.Unit.Name != null ? batchwiseStock.Unit.Code.ToString() : "NULL";
                var branchId = _authService.GetBranchId().Value;
                string startDate = batchEditDto.StartDate.HasValue ? $"'{batchEditDto.StartDate.Value.ToString("yyyy-MM-dd")}'" : "NULL";
                string endDate = batchEditDto.EndDate.HasValue ? $"'{batchEditDto.EndDate.Value.ToString("yyyy-MM-dd")}'" : "NULL";
                string expiryDate = batchEditDto.ExpiryDate.HasValue ? $"'{batchEditDto.ExpiryDate.Value.ToString("yyyy-MM-dd")}'" : "NULL";

                cmd.CommandText = $"Exec BatchWiseDetailsUpdateSP @Criteria='BatchDetailsForUpdate', @BranchID={branchId}, @DateFrom={startDate}, @DateUpto={endDate}, @ItemID={Items}, @AccountID={party}, @BatchNo={batch}, @ExpiryDate={expiryDate}";

                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count > 0)
                    {
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
                    return CommonResponse.NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse UpdateBatchNo(int pageId,string? vNo,string? batchNo=null,string? newBatchNo=null,DateTime? expiryDate=null)
        {
            try
            {
                var moduleName = _context.MaPageMenus.Where(p => p.Id == pageId).Select(p => p.MenuText).FirstOrDefault();
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Update " + moduleName);
                }
                string ExpiryDate = expiryDate.HasValue ? $"'{expiryDate.Value.ToString("yyyy-MM-dd")}'" : "NULL";
                var NewBatchNo = newBatchNo != null ? $"'{newBatchNo}'" : "NULL";
                var batch = batchNo!= null ? $"'{batchNo}'" : "NULL";
                //var UnitId = batchwiseStock.Unit.Name != null ? batchwiseStock.Unit.Code.ToString() : "NULL";
                var branchId = _authService.GetBranchId().Value;
                var data = _context.Database.ExecuteSqlRaw($"Exec BatchWiseDetailsUpdateSP @Criteria='UpdateBatch',@BranchID={branchId},@BatchNo={batch},@NewBatchNo={NewBatchNo},@ExpiryDate={ExpiryDate}");
                var batchNoData = new
                {
                    VoucherNo = vNo,
                    BatchNo = batchNo,
                    NewBatchNo = newBatchNo,
                    ExpiryDate = expiryDate,
                };
                var jsonBatch = JsonSerializer.Serialize(batchNoData);
                _userTrackService.AddUserActivity(vNo, (int)branchId, 1, "Updated", "FiTransactions", moduleName, 0, jsonBatch);
                return CommonResponse.Ok("Successfully Updated!!");               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.NoContent();
            }
        }

    }
}
