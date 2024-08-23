using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Serilog;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Security.Principal;
using Dfinance.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using static Dfinance.Shared.Routes.InvRoute;
using System.Buffers.Text;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ZstdSharp.Unsafe;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Identity.Client;
using Dfinance.Core.Views.Inventory.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Channels;
using Microsoft.Extensions.FileSystemGlobbing;
using System.IO;
using Dfinance.Shared.Enum;
using System.Xml.Linq;

namespace Dfinance.Finance.Services
{
    public class FinanceRegisterService : IFinanceRegister
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<FinanceRegisterService> _logger;

        public FinanceRegisterService(DFCoreContext context, IAuthService authService, ILogger<FinanceRegisterService> logger)
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
        public CommonResponse FillFinanceRegister(
      DateTime DateFrom, DateTime DateUpto, int BranchID, int? BasicVTypeID = null, int? VTypeID = null, bool Detailed = false, bool Inventory = false, bool Columnar = false, bool GroupItem = false, string Criteria = "", int? AccountID = null, int? PaymentTypeID = null, int? ItemID = null, int? CounterID = null, string PartyInvNo = "", string BatchNo = "", int? UserID = null, int? StaffID = null, int? AreaID = null, int? pageId = null)

        {
            try
            {
                int branchId = _authService.GetBranchId().Value;

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $@"
                EXEC InventoryRegisterSP
                @DateFrom = '{DateFrom:yyyy-MM-dd HH:mm:ss}', 
                @DateUpto = '{DateUpto:yyyy-MM-dd HH:mm:ss}', 
                @BranchID = {branchId}, 
                @BasicVTypeID = {(BasicVTypeID.HasValue ? BasicVTypeID.Value.ToString() : "NULL")},
                @VTypeID = {(VTypeID.HasValue ? VTypeID.Value.ToString() : "NULL")},
                @Detailed = {(Detailed ? 1 : 0)}, 
                @Inventory = {(Inventory ? 1 : 0)}, 
                @Columnar = {(Columnar ? 1 : 0)}, 
                @IsGroupItemReport = {(GroupItem ? 1 : 0)}, 
                @Criteria = {(string.IsNullOrEmpty(Criteria) ? "NULL" : $"'{Criteria}'")}, 
                @AccountID = {(AccountID.HasValue ? AccountID.Value.ToString() : "NULL")},
                @PaymentTypeID = {(PaymentTypeID.HasValue ? PaymentTypeID.Value.ToString() : "NULL")},
                @ItemID = {(ItemID.HasValue ? ItemID.Value.ToString() : "NULL")},
                @CounterID = {(CounterID.HasValue ? CounterID.Value.ToString() : "NULL")},
                @PartyInvNo = {(string.IsNullOrEmpty(PartyInvNo) ? "NULL" : $"'{PartyInvNo}'")}, 
                @BatchNo = {(string.IsNullOrEmpty(BatchNo) ? "NULL" : $"'{BatchNo}'")}, 
                @UserID = {(UserID.HasValue ? UserID.Value.ToString() : "NULL")}, 
                @StaffID = {(StaffID.HasValue ? StaffID.Value.ToString() : "NULL")}, 
                @AreaID = {(AreaID.HasValue ? AreaID.Value.ToString() : "NULL")}  ";
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
                _logger.LogError(ex, "An error occurred while filling the finance register");
                return CommonResponse.Error(ex.Message);
            }
        }



        public CommonResponse FillAccountPopup()
        {
            try
            {
                var query = (from a in _context.FiMaAccounts
                             join ba in _context.FiMaBranchAccounts on a.Id equals ba.AccountId
                             join mr in _context.CRMMemberRegister on a.Id equals mr.AccountId into mrGroup
                             from mr in mrGroup.DefaultIfEmpty()
                             join hr in _context.CRMHouseReg on mr.HouseId equals hr.Id into hrGroup
                             from hr in hrGroup.DefaultIfEmpty()
                             join p in _context.Parties on a.Id equals p.AccountId into pGroup
                             from p in pGroup.DefaultIfEmpty()
                             where !a.IsGroup && a.Active && ba.BranchId == 1
                             select new
                             {
                                 AccountCode = a.Alias,
                                 AccountName = a.Name,
                                 Details = (hr != null
                                            ? (hr.HouseName ?? "") + " " + (hr.Address1 ?? "") + " " + (hr.Address2 ?? "")
                                            : "") + " " + (p != null
                                                            ? (p.AddressLineOne ?? "") + " " + (p.AddressLineTwo ?? "")
                                                            : ""),
                                 ID = a.Id,
                                 IsBillWise = a.IsBillWise ?? false,
                                 IsCostCentre = a.IsCostCentre ?? false
                             })
                     .Take(15)
                     .ToList();

                return CommonResponse.Ok(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving top accounts.");
                return CommonResponse.Error(ex.Message); // Return the actual exception message
            }

        }
        public CommonResponse FillVoucherType(int primaryVoucherId)
        {
            try
            {
                var results = (from v in _context.FiMaVouchers
                               join pm in _context.MaPageMenus on v.Id equals pm.VoucherId
                               where v.PrimaryVoucherId == primaryVoucherId && (pm.Active ?? false)
                               select new
                               {
                                   v.Id,
                                   v.Name
                               })
                               .Distinct()
                               .ToList();

                return CommonResponse.Ok(new { Data = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving voucher data.");
                return CommonResponse.Error("An unexpected error occurred.");
            }
        }
        public CommonResponse FillBasicType()
        {
            try
            {
                var results = (from pv in _context.FiPrimaryVouchers
                               join mv in _context.FiMaVouchers on pv.Id equals mv.PrimaryVoucherId
                               where mv.ModuleType == 1 && (mv.Active ?? false) // Handle nullable boolean
                               select new
                               {
                                   pv.Id,
                                   pv.Name
                               })
                                      .Distinct()
                                      .ToList();

                return CommonResponse.Ok(new { Data = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving primary vouchers.");
                return CommonResponse.Error("An unexpected error occurred.");
            }
        }
    }
    }






























