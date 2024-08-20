using AutoMapper;
using Azure;
using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Inventory.Reports.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using Dfinance.Core.Domain;
using Microsoft.Extensions.FileSystemGlobbing;
using static Azure.Core.HttpHeader;
using static Dfinance.Shared.Routes.v1.ApiRoutes;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection.Metadata;
using Dfinance.AuthCore.Domain;
using FluentValidation.Validators;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using System.Security.Cryptography;
using ZstdSharp.Unsafe;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting.Server;
namespace Dfinance.Inventory.Reports
{
    public class InventoryRegisterService : IInventoryRegister
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<InventoryRegisterService> _logger;
        private readonly IHostEnvironment _environment;


        public InventoryRegisterService(DFCoreContext context, IAuthService authService, ILogger<InventoryRegisterService> logger, IHostEnvironment hostEnvironment)
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

        public CommonResponse FillInventoryRegister(
     DateTime DateFrom, DateTime DateUpto, int BranchID, int? BasicVTypeID, int? VTypeID = null,
     int? AccountID = null, int? SalesManID = null, int? ItemID = null, int? BrandID = null, int? OriginID = null,
     int? ColorID = null, int? CommodityID = null, int? LocationID = null, string Manufacturer = "",
     string GroupBy = "", int? AreaID = null, string VoucherNo = "", int? pageId = null)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $@"
            EXEC ItemwiseInventorySP 
            @DateFrom = '{DateFrom:yyyy-MM-dd HH:mm:ss}', 
            @DateUpto = '{DateUpto:yyyy-MM-dd HH:mm:ss}', 
            @BranchID = {branchId}, 
            @BasicVTypeID = {(BasicVTypeID.HasValue ? BasicVTypeID.Value.ToString() : "NULL")}, 
            @VTypeID = {(VTypeID.HasValue ? VTypeID.Value.ToString() : "NULL")}, 
            @AccountID = {(AccountID.HasValue ? AccountID.Value.ToString() : "NULL")}, 
            @SalesManID = {(SalesManID.HasValue ? SalesManID.Value.ToString() : "NULL")}, 
            @ItemID = {(ItemID.HasValue ? ItemID.Value.ToString() : "NULL")},  
            @BrandID = {(BrandID.HasValue ? BrandID.Value.ToString() : "NULL")}, 
            @OriginID = {(OriginID.HasValue ? OriginID.Value.ToString() : "NULL")}, 
            @ColorID = {(ColorID.HasValue ? ColorID.Value.ToString() : "NULL")}, 
            @CommodityID = {(CommodityID.HasValue ? CommodityID.Value.ToString() : "NULL")}, 
            @LocationID = {(LocationID.HasValue ? LocationID.Value.ToString() : "NULL")}, 
            @Manufacturer = {(string.IsNullOrEmpty(Manufacturer) ? "NULL" : $"'{Manufacturer}'")}, 
            @GroupBy = {(string.IsNullOrEmpty(GroupBy) ? "NULL" : $"'{GroupBy}'")}, 
            @AreaID = {(AreaID.HasValue ? AreaID.Value.ToString() : "NULL")}, 
            @VoucherNo = {(string.IsNullOrEmpty(VoucherNo) ? "NULL" : $"'{VoucherNo}'")} ";


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
                _logger.LogError(ex, "An error occurred while filling the inventory register");
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse PopManufacturer(int type)
        {
            try
            {
                if (type == 1)
                {
                    // Get distinct manufacturers
                    var manufacturers = _context.ItemMaster
                        .Where(item => !string.IsNullOrEmpty(item.Manufacturer))
                        .Select(item => item.Manufacturer)
                        .Distinct()
                        .ToList();

                    return CommonResponse.Ok(new { Type = "Manufacturers", Data = manufacturers });
                }
                else if (type == 2)
                {
                    // Get items from MaMisc based on key and active status
                    var itemBrands = _context.MaMisc
                        .Where(m => m.Key.Contains("Item Brand") && m.Active)
                        .Select(m => new
                        {
                            ID = m.Id,
                            Description = m.Value
                        })
                        .ToList();

                    return CommonResponse.Ok(new { Type = "ItemBrands", Data = itemBrands });
                }
                else if (type == 3)
                {

                    var result = (from pv in _context.FiPrimaryVouchers
                                  join v in _context.FiMaVouchers on pv.Id equals v.PrimaryVoucherId
                                  join pm in _context.MaPageMenus on v.Id equals pm.VoucherId
                                  where (v.ModuleType == 2 || v.ModuleType == 3)
                                        && (v.Active ?? false) == true
                                        && (pm.Active ?? false) == true
                                  select new
                                  {
                                      pv.Id,
                                      pv.Name
                                  })
                          .Distinct()
                          .ToList();

                    return CommonResponse.Ok(new { Type = "PrimaryVouchers", Data = result });
                }
                else
                {
                    return CommonResponse.Error("Invalid type parameter.");
                }
            }
            catch (Exception ex)
            {
                // Log detailed exception information
                _logger.LogError(ex, "An error occurred while fetching data for type {Type}", type);

                // Return a more informative response
                return CommonResponse.Error($"Internal server error. Details: {ex.Message}");
            }
        }
    }
}





















