﻿using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.Warehouse.Services
{
    public class StockReportService : IStockReportService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<StockReportService> _logger;
        private readonly AuthCoreContext _authContext;
        public StockReportService(DFCoreContext context, IAuthService authService, IHostEnvironment environment, ILogger<StockReportService> logger, AuthCoreContext authCoreContext)
        {
            _authService = authService;
            _environment = environment;
            _logger = logger;
            _context = context;
            _authContext = authCoreContext;
        }
        private CommonResponse GetItem()
        {
            var item = (from itemmaster in _context.ItemMaster
                        join misc in _context.MaMisc on itemmaster.ColorId equals misc.Id into miscJoin
                        from misc in miscJoin.DefaultIfEmpty()
                        where itemmaster.Active == true && itemmaster.IsGroup == false
                        select new
                        {
                            ItemCode = itemmaster.ItemCode,
                            ItemName = itemmaster.ItemName,
                            Unit = itemmaster.Unit,
                            BarCode = itemmaster.BarCode,
                            Color = misc.Value,
                            ItemId = itemmaster.Id
                        }).ToList();
            return CommonResponse.Ok(item);
        }
        private CommonResponse GetSize()
        {
            var size = _context.InvSizeMaster.Where(s => s.Active == true).Select(s => new { s.Code, s.Name, s.Id }).ToList();
            return CommonResponse.Ok(size);
        }
        private CommonResponse FillVoucherLocations()
        {
            var location = _context.LocationViewList.FromSqlRaw("Exec DropDownListSP @Criteria='FillVoucherLocations',@StrParam='StockReg Location wise'").ToList();
            return CommonResponse.Ok(location);
        }
        public CommonResponse FillVoucherLocations(int branchId)
        {
            var location = _context.LocationViewList.FromSqlRaw($"Exec DropDownListSP @Criteria='FillVoucherLocations',@StrParam='StockReg Location wise',@IntParam={branchId}").ToList();
            return CommonResponse.Ok(location);
        }
        private CommonResponse FillTypeOfWoodForReports()
        {
            var branchId = _authService.GetBranchId();
            var location = (from tw in _context.CategoryType
                            join c in _context.MaBranches on tw.CreatedBranchId equals c.Id
                            where tw.ActiveFlag == 1 && c.BranchCompanyId == branchId
                            orderby tw.Description
                            select new
                            {
                                Name = tw.Description,
                                Id = tw.Id
                            }).Distinct().ToList();
            return CommonResponse.Ok(location);
        }
        private CommonResponse GetUnit()
        {
            var unit = _context.UnitMaster.Select(u => new { u.Unit, u.BasicUnit, u.Factor }).ToList();
            return CommonResponse.Ok(unit);
        }
        private CommonResponse GetBarCode()
        {
            var barcode = _context.ItemMaster.Where(i => i.BarCode != null && i.Active == true).Select(u => new { u.Id, Code = u.BarCode, Name = u.ItemName }).Distinct().ToList();
            return CommonResponse.Ok(barcode);
        }
        private CommonResponse GetOrginBrandColor(string key)
        {
            var orgin = _context.MaMisc.Where(i => i.Key == key && i.Active == true).Select(u => new { u.Id, Description = u.Value }).Distinct().ToList();
            return CommonResponse.Ok(orgin);
        }
        private CommonResponse GetParty(string nature)
        {
            var parti = (from party in _context.Parties
                         join ac in _context.FiMaAccounts on party.AccountId equals ac.Id
                         where ac.Active == true && party.Nature == nature
                         select new
                         {
                             AccountCode = ac.Alias,
                             AccountName = ac.Name,
                             ac.Id
                         }).ToList();
            return CommonResponse.Ok(parti);
        }
        private CommonResponse GetParties()
        {
            var parties = _context.FiMaAccounts.Where(a => a.IsGroup == false && a.Active == true).Select(a => new { AccountCode = a.Alias, AccountName = a.Name, a.Id }).ToList();
            return CommonResponse.Ok(parties);
        }
        private CommonResponse GetCategoryType()
        {
            var woods = _context.CategoryType.Where(w => w.ActiveFlag == 1).Select(w => new { w.Code, w.Description, w.Id }).ToList();
            return CommonResponse.Ok(woods);
        }
        private CommonResponse GetCommodity()
        {
            var commoditi = _context.Category.Where(c => c.ActiveFlag == 1).Select(w => new { w.CategoryCode, w.Description, w.Id }).ToList();
            return CommonResponse.Ok(commoditi);
        }
        private CommonResponse GetUser()
        {
            var user = _context.MaEmployees.Where(c => c.Active == true).Select(e => new { e.Id, e.Username, e.FirstName, e.LastName, e.EmailId, e.MobileNumber }).ToList();
            return CommonResponse.Ok(user);
        }
        public CommonResponse GetLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var user = GetUser().Data;
                var commditti = GetCommodity().Data;
                var category = GetCategoryType().Data;
                var cust = GetParty("C").Data;
                var supplier = GetParty("S").Data;
                var orgin = GetOrginBrandColor("Item Origin").Data;
                var brand = GetOrginBrandColor("Item Brand").Data;
                var color = GetOrginBrandColor("Item Color").Data;
                var unit = GetUnit().Data;
                var barcode = GetBarCode().Data;
                var size = GetSize().Data;
                var location = FillVoucherLocations().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item,
                    User = user,
                    Commodity = commditti,
                    Category = category,
                    Customer = cust,
                    Supplier = supplier,
                    Orgin = orgin,
                    Brand = brand,
                    Color = color,
                    Unit = unit,
                    BarCode = barcode,
                    Size = size,
                    Location = location
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex);
            }
        }
        public CommonResponse FillItemReports(StockRegistration stockRegistration)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var locationId = stockRegistration.LocationID.Id != null ? stockRegistration.LocationID.Id.ToString(): "NULL " ;
                var itemId = stockRegistration.ItemID.Id != null ? stockRegistration.ItemID.Id.ToString() : "NULL";
                var accountID = stockRegistration.AccountID.Id != null ? stockRegistration.AccountID.Id.ToString() : "NULL";
                var branchID = stockRegistration.BranchID.Id != null ? stockRegistration.BranchID.Id.ToString() : "NULL";
                var isItemWise = stockRegistration.IsItemwise == 0 ? false : true;
                var barcode = stockRegistration.Barcode.Id != null ? stockRegistration.Barcode.Id.ToString() : "NULL";
                var commityId = stockRegistration.CommodityID.Id != null ? stockRegistration.CommodityID.Id.ToString() : "NULL";
                var orginId = stockRegistration.OriginID.Id != null ? stockRegistration.OriginID.Id.ToString() : "NULL";
                var brandId = stockRegistration.BrandID.Id != null ? stockRegistration.BrandID.Id.ToString() : "NULL";
                var colorId = stockRegistration.ColorID.Id != null ? stockRegistration.ColorID.Id.ToString() : "NULL";
                var batchNo = stockRegistration.BatchNo != null ? stockRegistration.BatchNo.ToString() : "NULL";
                var supplierId = stockRegistration.SupplierID.Id != null ? stockRegistration.SupplierID.Id.ToString() : "NULL";
                var customerId = stockRegistration.CustomerID.Id != null ? stockRegistration.CustomerID.Id.ToString() : "NULL";
                var userId = _authService.GetId();
                var categoryId = stockRegistration.CategoryTypeID.Id != null ? stockRegistration.CategoryTypeID.Id.ToString() : "NULL";
                cmd.CommandText = $"Exec InventoryItemsReportSP @Criteria='InventoryItemWiseReport',@LocationID={locationId},@ItemID={itemId}," +
                    $"@BranchID={branchID},@ToDate='{stockRegistration.ToDate}',@IsItemwise={isItemWise},@Barcode={barcode},@OriginID={orginId},@BrandID={brandId}," +
                    $"@CategoryTypeID={categoryId},@CommodityID={commityId},@ColorID={colorId},@AccountID={accountID},@BatchNo={batchNo},@SupplierID={supplierId}," +
                    $"@CustomerID={customerId},@AddedBy={userId}";
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
                return CommonResponse.Ok(ex);
            }
        }


        //************************************************************************************************************************
        //---------------------------------------------ItemStock Register-----------------------------------------
        ///Aswathy
        //*************************************************************************************************************************


        private CommonResponse GetVoucherList()
        {
            var vouchers = (from voucher in _context.FiMaVouchers
                            join page in _context.MaPageMenus on voucher.Id equals page.VoucherId
                            where page.Active == true && page.ModuleId == 1
                            select new
                            {
                                Code = voucher.Alias,
                                voucher.Name,
                                voucher.Id
                            }).ToList();
            return CommonResponse.Ok(vouchers);
        }
        public CommonResponse GetItemStockLoadData()
        {
            try
            {
                var voucher = GetVoucherList().Data;
                var itemList = GetItem().Data;
                _logger.LogInformation("Sucessfully Load GetData");
                return CommonResponse.Ok(new { Voucher = voucher, Items = itemList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }

        public CommonResponse FillStockItemRegister(ItemStockRegisterRpt itemStockRegister)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var itemId = itemStockRegister.ItemID.Id != null ? itemStockRegister.ItemID.Id.ToString() : "NULL";
                var branchID = itemStockRegister.BranchID.Id != null ? itemStockRegister.BranchID.Id.ToString() : "NULL";
                //var batchNo = itemStockRegister.BatchNo != "" ? itemStockRegister.BatchNo.ToString() : "NULL";
                if (itemStockRegister.BatchNo != null)
                    cmd.CommandText = $"Exec StockRegisterSP @FromDate='{itemStockRegister.FromDate}',@ItemID={itemId},@BranchID={branchID},@ToDate='{itemStockRegister.ToDate}',@BatchNo='{itemStockRegister.BatchNo}'";
                else
                    cmd.CommandText = $"Exec StockRegisterSP @FromDate='{itemStockRegister.FromDate}',@ItemID={itemId},@BranchID={branchID},@ToDate='{itemStockRegister.ToDate}'";
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

        //**************************ItemDetails*********************

        private CommonResponse FillAllBranch()
        {
            try
            {
                string criteria = "FillCompanyMaster";
                var result = _context.SpFillAllBranch.FromSqlRaw($"EXEC spCompany @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetItemDetailsLoadData()
        {
            try
            {

                var branches = FillAllBranch().Data;
                var itemList = GetItem().Data;
                _logger.LogInformation("Sucessfully Load GetData");
                return CommonResponse.Ok(new { Branches = branches, Items = itemList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillStockItemDetails(ItemDetailsRpt itemDetails)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var itemId = itemDetails.ItemId.Id != null ? itemDetails.ItemId.Id.ToString() : "NULL";
                var branchID = itemDetails.BranchId.Id != null ? itemDetails.BranchId.Id.ToString() : "NULL";
                cmd.CommandText = $"Exec GoldPriceSP @Criteria='GoldStock',@StockType='Quality',@FromDate='{itemDetails.FromDate}',@ItemID={itemId},@BranchID={branchID},@ToDate='{itemDetails.ToDate}'";
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
        //************************************ Warehouse Stock **************************************
        public CommonResponse GetWarehouseStockLoadData()
        {
            try
            {
                var locations = FillVoucherLocations().Data;
                _logger.LogInformation("Sucessfully Load GetWarehouseStockLoadData");
                return CommonResponse.Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillWarehouseStockDetails(WarehouseStockRegRpt warehouseStock)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var locationId = warehouseStock.LocationId.Id != null ? warehouseStock.LocationId.Id.ToString() : "NULL";
                var branchID = _authService.GetBranchId().Value;
                cmd.CommandText = $"Exec StockRegisterSP @Criteria='Locationwise',@FromDate='{warehouseStock.FromDate}',@LocationID={locationId},@BranchID={branchID},@ToDate='{warehouseStock.ToDate}'";
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

        //************************CommodityWiseRegister******************************

        public CommonResponse GetCommudityLoadData()
        {
            try
            {
                var locations = FillVoucherLocations().Data;
                var type = FillTypeOfWoodForReports().Data;
                _logger.LogInformation("Sucessfully Load GetWarehouseStockLoadData");
                return CommonResponse.Ok(new { Location = locations, Type = type });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillStockRegisterCommoditywise(CommodityStockRegRpt commodityStockReg)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var locationId = commodityStockReg.LocationId.Id != null ? commodityStockReg.LocationId.Id.ToString() : "NULL";
                var branchID = _authService.GetBranchId().Value;
                var typeId = commodityStockReg.TyoeOfWood.Id != null ? commodityStockReg.TyoeOfWood.Id.ToString() : "NULL";
                cmd.CommandText = $"Exec StockRegisterSP @Criteria='Commoditywise',@FromDate='{commodityStockReg.FromDate}',@LocationID={locationId},@BranchID={branchID},@ToDate='{commodityStockReg.ToDate}',@TypeOfWoodID={typeId}";
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

        //*********************** Stock Receipt Issue ******************************

        public CommonResponse GetStockReceiptLoadData()
        {
            try
            {
                var branches = FillAllBranch().Data;
                var locations = FillVoucherLocations().Data;
                var item = GetItem().Data;
                _logger.LogInformation("Sucessfully Load GetWarehouseStockLoadData");
                return CommonResponse.Ok(new { Location = locations, Item = item, Branches = branches });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillStockReceiptIssue(StockReceiptIssueRpt stockReceiptIssue)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var locationId = stockReceiptIssue.LocationId.Id != null ? stockReceiptIssue.LocationId.Id.ToString() : "NULL";
                var Items = stockReceiptIssue.ItemId.Id != null ? stockReceiptIssue.ItemId.Id.ToString() : "NULL";
                var fromBranch = stockReceiptIssue.FromBranch.Id != null ? stockReceiptIssue.FromBranch.Id.ToString() : "NULL";
                var toBranch = stockReceiptIssue.ToBranch.Id != null ? stockReceiptIssue.ToBranch.Id.ToString() : "NULL";
                var vType = stockReceiptIssue.VType.Value != null ? stockReceiptIssue.VType.Value.ToString() : "All";
                cmd.CommandText = $"Exec StockIssueReceiptSP @DateFrom='{stockReceiptIssue.FromDate}',@DateUpto='{stockReceiptIssue.ToDate}',@VType='{vType}',@LocationID={locationId},@FromBranchID={fromBranch},@ToBranchID={toBranch},@ItemID={Items}";
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

        //*********************** Itemwise Stock ******************************

        public CommonResponse GetUnitwiseStockLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var commditti = GetCommodity().Data;
                var orgin = GetOrginBrandColor("Item Origin").Data;
                var brand = GetOrginBrandColor("Item Brand").Data;
                var unit = GetUnit().Data;
                var barcode = GetBarCode().Data;
                var location = FillVoucherLocations().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item,
                    Commodity = commditti,
                    Orgin = orgin,
                    Brand = brand,
                    BarCode = barcode,
                    Location = location
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillUnitwiseStock(UnitwiseStock unitwiseStock)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var locationId = unitwiseStock.LocationID.Id != null ? unitwiseStock.LocationID.Id.ToString() : "NULL";
                var Items = unitwiseStock.ItemID.Id != null ? unitwiseStock.ItemID.Id.ToString() : "NULL";
                var brandid = unitwiseStock.BrandId.Id != null ? unitwiseStock.BrandId.Id.ToString() : "NULL";
                var commodity = unitwiseStock.CommodityID.Id != null ? unitwiseStock.CommodityID.Id.ToString() : "NULL";
                var orgin = unitwiseStock.OriginID.Id != null ? unitwiseStock.OriginID.Id.ToString() : "NULL";
                //var barcode = unitwiseStock.Barcode.Code != null ? unitwiseStock.Barcode.Code.ToString() : "NULL";
                var branchId = _authService.GetBranchId().Value;
                if (unitwiseStock.Barcode.Code != "" && unitwiseStock.Barcode.Code != null)
                    cmd.CommandText = $"Exec UnitwiseStockSP @DateUpto='{unitwiseStock.ToDate}',@BranchID={branchId},@WarehouseID={locationId},@ItemID={Items},@CommodityID={commodity},@BrandID={brandid},@OriginID={orgin},@Barcode='{unitwiseStock.Barcode.Code}'";
                else
                    cmd.CommandText = $"Exec UnitwiseStockSP @DateUpto='{unitwiseStock.ToDate}',@BranchID={branchId},@WarehouseID={locationId},@ItemID={Items},@CommodityID={commodity},@BrandID={brandid},@OriginID={orgin}";

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



        //*********************** Itemwise Stock ******************************

        public CommonResponse GetItemwiseStockLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var commditti = GetCommodity().Data;
                var unit = GetUnit().Data;
                var categories = GetCategoryType().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Items = item,
                    Commodities = commditti,
                    Categories = categories,
                    Units = unit
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillItemwiseStock(ItemsCatalogue itemsCatalogue)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var warehouseId = itemsCatalogue.WarehouseID.Id != null ? itemsCatalogue.WarehouseID.Id.ToString() : "NULL";
                var branchId = _authService.GetBranchId().Value;
                if (itemsCatalogue.Less == null)
                    itemsCatalogue.Less = false;
                if (itemsCatalogue.Date != null)
                    cmd.CommandText = $"Exec ItemCatalogueSP @BranchID={branchId},@WarehouseID={warehouseId},@Less={itemsCatalogue.Less}";
                else
                    cmd.CommandText = $"Exec ItemCatalogueSP @BranchID={branchId},@WarehouseID={warehouseId},@Less={itemsCatalogue.Less},@Date='{itemsCatalogue.Date}'";

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

        //*********************** Unitwise Stock ******************************
        private CommonResponse GetPrimaryVoucher()
        {
            string[] vName = { "Purchase", "Sales Invoice" };
            var vouchers = _context.FiMaVouchers.Where(p => vName.Contains(p.Name)).Select(p => new { Id = p.PrimaryVoucherId, p.Name }).ToList();
            return CommonResponse.Ok(vouchers);
        }
        private CommonResponse GetPartyCategory()
        {
            var partyCategory = _context.DropDownViewValue.FromSqlRaw("Exec DropDownListSP @Criteria='PartyCategory'").ToList();
            return CommonResponse.Ok(partyCategory);
        }
        private CommonResponse GetCommodityTypy()
        {
            var branchId = _authService.GetBranchId().Value;
            var commodityType = _context.DropDownViewName.FromSqlRaw($"Exec DropDownListSP @Criteria='FillTypeOfWoodForReports',@IntParam={branchId}").ToList();
            return CommonResponse.Ok(commodityType);
        }
        private CommonResponse GetSalesMan()
        {
            var salesMan = _context.FiMaAccounts.Where(a => a.AccountCategory == 3 && a.Active == true).Select(a => new { Code = a.Alias, a.Name, a.Id }).ToList();
            return CommonResponse.Ok(salesMan);
        }
        private CommonResponse GetPartys()
        {
            var branchId = _authService.GetBranchId().Value;
            List<int> accountCategories = new List<int> { 1, 2 };
            var result = from A in _context.FiMaAccounts
                         join B in _context.FiMaBranchAccounts on A.Id equals B.AccountId
                         join P in _context.Parties on A.Id equals P.AccountId into joinedParties
                         from P in joinedParties.DefaultIfEmpty() // Left join
                         where A.IsGroup == false
                            && A.Active == true
                             && accountCategories.Contains((int)A.AccountCategory)
                            && A.AccountCategory == 1 || A.AccountCategory == 2
                            && B.BranchId == branchId
                         select new
                         {
                             AccountCode = A.Alias,
                             AccountName = A.Name,
                             Address = (P.AddressLineOne == null || P.AddressLineOne.Trim() == "" ? "" :
                                        (P.AddressLineTwo != null && P.City != null && P.Pobox != null ? P.AddressLineOne + ", " : P.AddressLineOne + ". ")) +
                                       (P.AddressLineTwo == null || P.AddressLineTwo.Trim() == "" ? "" :
                                        (P.City != null && P.Pobox != null ? P.AddressLineTwo + ", " : P.AddressLineTwo + ". ")) +
                                       (P.City == null || P.City.Trim() == "" ? "" :
                                        (P.Pobox != null ? P.City + ", " : P.City + ". ")) +
                                       (P.Pobox == null || P.Pobox.Trim() == "" ? "" : P.Pobox + "."),
                             ID = A.Id
                         };

            var resultList = result.ToList();
            return CommonResponse.Ok(resultList);
        }
        public CommonResponse GetMonthwiseStockLoadData()
        {
            try
            {
                var parties = GetParties().Data;
                var vochers = GetPrimaryVoucher().Data;
                var item = GetItem().Data;
                var commdittiType = GetCommodityTypy().Data;
                var commodity = GetCommodity().Data;
                var partyCategory = GetPartyCategory().Data;
                var salesman = GetSalesMan().Data;
                var branches = FillAllBranch().Data;
                var categoriType = GetCategoryType().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Parties = parties,
                    Items = item,
                    Commodities = commodity,
                    SalesMan = salesman,
                    Vouchers = vochers,
                    Branches = branches,
                    PartiCategory = partyCategory,
                    CommoditiType = commdittiType,
                    CategoriType = categoriType
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillMonthwiseStock(MonthwiseStockRpt monthwiseStk)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var partyId = monthwiseStk.PartyId.Id != null ? monthwiseStk.PartyId.Id.ToString() : "NULL";
                var voucher = monthwiseStk.VoucherId.Id != null ? monthwiseStk.VoucherId.Id.ToString() : "NULL";
                var partiCategory = monthwiseStk.PartyCategoryId.Id != null ? monthwiseStk.PartyCategoryId.Id.ToString() : "NULL";
                var categoryType = monthwiseStk.CategoryType.Id != null ? monthwiseStk.CategoryType.Id.ToString() : "NULL";
                var commpdity = monthwiseStk.Commodity.Id != null ? monthwiseStk.Commodity.Id.ToString() : "NULL";
                var item = monthwiseStk.ItemId.Id != null ? monthwiseStk.ItemId.Id.ToString() : "NULL";

                var branchId = _authService.GetBranchId().Value;
                if (monthwiseStk.SalesMan.Id != null)
                    cmd.CommandText = $"Exec ConsolidatedMonthwiseInventorySP @Criteria='Stock',@BranchID={branchId},@DateFrom='{monthwiseStk.FromDate}',@DateUpto='{monthwiseStk.ToDate}',@AccountID={partyId},@VoucherID={voucher},@DrCr='{monthwiseStk.DrCr}',@PartyCategoryID={partiCategory},@CategoryType={categoryType},@Commodity={commpdity},@ItemID={item},@SalesManID={monthwiseStk.SalesMan.Id},@ViewBy='{monthwiseStk.ViewBy}'";
                else
                    cmd.CommandText = $"Exec ConsolidatedMonthwiseInventorySP @Criteria='Stock',@BranchID={branchId},@DateFrom='{monthwiseStk.FromDate}',@DateUpto='{monthwiseStk.ToDate}',@AccountID={partyId},@VoucherID={voucher},@DrCr='{monthwiseStk.DrCr}',@PartyCategoryID={partiCategory},@CategoryType={categoryType},@Commodity={commpdity},@ItemID={item},@ViewBy='{monthwiseStk.ViewBy}'";

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

        //*********************** Warehousewise Stock ******************************

        private CommonResponse GetCompany()
        {
            var companies = _authContext.Companies.Where(c => c.Active == true).ToList();
            return CommonResponse.Ok(companies);
        }

        public CommonResponse GetWarehousewiseStockLoadData()
        {
            try
            {
                var companies = GetCompany().Data;
                var branch = FillAllBranch().Data; ///*********************** companiwise branch not working ***************************               
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Companies = companies,
                    Branch = branch,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillWarehousewiseStock(string item, int branchId)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var branch = branchId != null ? branchId.ToString() : "NULL";
                cmd.CommandText = $"Exec ItemSearchItemSP @Value={item},@BranchID={branchId},@Criteria='WarehouseStock'";
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

        //
        //*********************** Warehousewise Inventory ******************************

        private CommonResponse GetBasicVType()
        {
            List<int> moduleType = new List<int> { 2, 3 };
            var basicVType = (from PV in _context.FiPrimaryVouchers
                              join V in _context.FiMaVouchers on PV.Id equals V.PrimaryVoucherId
                              join PM in _context.MaPageMenus on V.Id equals PM.VoucherId
                              where moduleType.Contains((int)V.ModuleType) && V.Active == true && PM.Active == true
                              select new
                              {
                                  Id = PV.Id,
                                  Name = PV.Name,
                              }).ToList();
            return CommonResponse.Ok(basicVType.Distinct());
        }
        private CommonResponse GetArea()
        {
            var areas=_context.MaArea.Where(a=>a.Active==true).Select(a=>new { a.Code, a.Name,a.Id}).ToList();
            return CommonResponse.Ok(areas);
        }
        private CommonResponse GetManufacturer()
        {
            var manufacturer=_context.ItemMaster.Where(m=>m.Manufacturer != null && m.Manufacturer!="").Select(m=>m.Manufacturer).ToList();
            return CommonResponse.Ok(manufacturer.Distinct());
        }
        public CommonResponse GetWarehousewiseInventoryLoadData()
        {
            try
            {
                
                var branch = FillAllBranch().Data; ///*********************** companiwise branch not working ***************************    
               var basicVTypes=GetBasicVType().Data;
                var locations = FillVoucherLocations(_authService.GetBranchId().Value);
                var staffs = GetSalesMan().Data;
                var parties=GetPartys().Data;  
                var areas=GetArea().Data;
                var items=GetItem().Data;
                var origin = GetOrginBrandColor("Item Origin");
                var brand = GetOrginBrandColor("Item Brand");
                var colors = GetOrginBrandColor("Item Color");
                var categoryType=GetCategoryType().Data;
                var commodities=GetCommodity().Data;
                var manufactures=GetManufacturer().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    BasicType=basicVTypes,
                    Branch = branch,
                    Locations=locations,
                    Staffs=staffs,
                    Parties=parties,
                    Areas=areas,
                    Items=items,
                    Origin=origin,
                    Brand=brand,
                    Colors=colors,
                    CategoryType=categoryType,
                    Commodities=commodities,
                    Manufactures=manufactures,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillWarehousewiseInventory(WarehouseWiseInventoryRpt warehouseWiseInventory)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var branchId = warehouseWiseInventory.BranchID.Id != null ? warehouseWiseInventory.BranchID.Id.ToString() : "NULL";
                string dateFrom = $"'{warehouseWiseInventory.FromDate.ToString("yyyy-MM-dd")}'";
                string dateUpto =  $"'{warehouseWiseInventory.ToDate.ToString("yyyy-MM-dd")}'" ;
                var vno = warehouseWiseInventory.VoucherNo != null ? $"'{warehouseWiseInventory.VoucherNo}'" : "NULL";
                var basicTypeId = warehouseWiseInventory.BasicType.Id != null ? warehouseWiseInventory.BasicType.Id.ToString() : "NULL";
                var accountId = warehouseWiseInventory.PartyId.Id != null ? warehouseWiseInventory.PartyId.Id.ToString() : "NULL";
                var brandId = warehouseWiseInventory.BrandID.Id != null ? warehouseWiseInventory.BrandID.Id.ToString() : "NULL";
                var orginId = warehouseWiseInventory.OriginID.Id != null ? warehouseWiseInventory.OriginID.Id.ToString() : "NULL";
                var colorId = warehouseWiseInventory.ColorID.Id != null ? warehouseWiseInventory.ColorID.Id.ToString() : "NULL";
                var catTypeId = warehouseWiseInventory.CategoryTypeID.Id != null ? warehouseWiseInventory.CategoryTypeID.Id.ToString() : "NULL";
                var commodityId = warehouseWiseInventory.CommodityID.Id != null ? warehouseWiseInventory.CommodityID.Id.ToString() : "NULL";
                var locationId = warehouseWiseInventory.WarehouseId.Id != null ? warehouseWiseInventory.WarehouseId.Id.ToString() : "NULL";
                var manufactuer = warehouseWiseInventory.Manufacture.Id != null ? warehouseWiseInventory.Manufacture.Id.ToString() : "NULL";
                var groupById = warehouseWiseInventory.GroupBy.Id != null ? $"'{warehouseWiseInventory.GroupBy.Id.ToString()}'" : "NULL";
                var itemId = warehouseWiseInventory.ItemID.Id != null ? warehouseWiseInventory.ItemID.Id.ToString() : "NULL";
                var salesManId = warehouseWiseInventory.SalesManId.Id != null ? warehouseWiseInventory.SalesManId.Id.ToString() : "NULL";
                var vTypeId = warehouseWiseInventory.VType.Id != null ? warehouseWiseInventory.VType.Id.ToString() : "NULL";
                var areaId = warehouseWiseInventory.Area.Id != null ? warehouseWiseInventory.Area.Id.ToString() : "NULL";

                cmd.CommandText = $"Exec WarehousewiseInventorySP @DateFrom={dateFrom}, @DateUpto={dateUpto},@BranchID={branchId},@BasicVTypeID={basicTypeId},@VTypeID={vTypeId}," +
                    $"@AccountID={accountId},@BrandID={brandId},@OriginID={orginId},@ColorID={colorId},@CategoryTypeID={catTypeId},@CommodityID={commodityId},@LocationID={locationId}," +
                    $"@Manufacturer={manufactuer},@GroupBy={groupById},@ItemID={itemId},@SalesManID={salesManId},@AreaID={areaId},@VoucherNo={vno}";
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
        //*********************** Batchwise Stock ******************************



        /// <summary>
        /// GetLoad Data  - same as  ===================== GetUnitwiseStockLoadData===============================
        /// </summary>
        /// <param name="item"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// 

        public CommonResponse GetBatchwiseStockLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var commditti = GetCommodity().Data;
                var orgin = GetOrginBrandColor("Item Origin").Data;
                var brand = GetOrginBrandColor("Item Brand").Data;
                var unit = GetUnit().Data;
                var barcode = GetBarCode().Data;
                var location = FillVoucherLocations().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item,
                    Commodity = commditti,
                    Orgin = orgin,
                    Brand = brand,
                    BarCode = barcode,
                    Location = location,
                    Unit = unit
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }

        public CommonResponse FillBatchwiseStock(BatchwiseStockRpt batchwiseStock) // Batch No front end filter
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                var locationId = batchwiseStock.WarehouseID.Id != null ? batchwiseStock.WarehouseID.Id.ToString() : "NULL";
                var Items = batchwiseStock.ItemID.Id != null ? batchwiseStock.ItemID.Id.ToString() : "NULL";
                var brandid = batchwiseStock.BrandId.Id != null ? batchwiseStock.BrandId.Id.ToString() : "NULL";
                var commodity = batchwiseStock.CommodityID.Id != null ? batchwiseStock.CommodityID.Id.ToString() : "NULL";
                var orgin = batchwiseStock.OriginID.Id != null ? batchwiseStock.OriginID.Id.ToString() : "NULL";
                //var UnitId = batchwiseStock.Unit.Name != null ? batchwiseStock.Unit.Code.ToString() : "NULL";
                var branchId = _authService.GetBranchId().Value;
                if (batchwiseStock.Barcode.Code != null && batchwiseStock.Unit.Name != null)
                    cmd.CommandText = $"Exec BatchwiseStockSP @DateUpto='{batchwiseStock.DateUpto}',@BranchID={branchId},@WarehouseID={locationId},@ItemID={Items},@CommodityID={commodity},@BrandID={brandid},@OriginID={orgin},@Unit='{batchwiseStock.Unit.Name}',@Barcode='{batchwiseStock.Barcode.Code}'";
                else if (batchwiseStock.Barcode.Code != null && batchwiseStock.Unit.Name == null)
                    cmd.CommandText = $"Exec BatchwiseStockSP @DateUpto='{batchwiseStock.DateUpto}',@BranchID={branchId},@WarehouseID={locationId},@ItemID={Items},@CommodityID={commodity},@BrandID={brandid},@OriginID={orgin},@Barcode='{batchwiseStock.Barcode.Code}'";
                else if (batchwiseStock.Barcode.Code == null && batchwiseStock.Unit.Name != null)
                    cmd.CommandText = $"Exec BatchwiseStockSP @DateUpto='{batchwiseStock.DateUpto}',@BranchID={branchId},@WarehouseID={locationId},@ItemID={Items},@CommodityID={commodity},@BrandID={brandid},@OriginID={orgin},@Unit='{batchwiseStock.Unit.Name}'";
                else
                    cmd.CommandText = $"Exec BatchwiseStockSP @DateUpto='{batchwiseStock.DateUpto}',@BranchID={branchId},@WarehouseID={locationId},@ItemID={Items},@CommodityID={commodity},@BrandID={brandid},@OriginID={orgin}";

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


        //*********************** Itemwise Register Stock ******************************



        /// <summary>
        /// GetLoad Data  - same as  ===================== GetUnitwiseStockLoadData===============================
        /// </summary>
        /// <param name="item"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// 

        public CommonResponse GetItemwiseRegStockLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var commditti = GetCommodity().Data;
                var type = FillTypeOfWoodForReports().Data;
                var branch = FillAllBranch().Data;

                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item,
                    Commodity = commditti,
                    Type = type,
                    Branch = branch
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }

        public CommonResponse FillItemwiseRegStock(ItemwiseRegRpt itemwiseReg) // Batch No front end filter
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                var locationId = itemwiseReg.LocationID.Id != null ? itemwiseReg.LocationID.Id.ToString() : "NULL";
                var Items = itemwiseReg.ItemID.Id != null ? itemwiseReg.ItemID.Id.ToString() : "NULL";
                var branchId = itemwiseReg.BranchID.Id != null ? itemwiseReg.BranchID.Id.ToString() : "NULL";
                var commodity = itemwiseReg.CommodityID.Id != null ? itemwiseReg.CommodityID.Id.ToString() : "NULL";
                var typeOfWoodId = itemwiseReg.TypeOfWoodID.Id != null ? itemwiseReg.TypeOfWoodID.Id.ToString() : "NULL";
                cmd.CommandText = $"Exec StockRegisterSP @Criteria='Itemwise',@FromDate='{itemwiseReg.FromDate}',@ToDate='{itemwiseReg.ToDate}',@BranchID={branchId},@LocationID={locationId},@ItemID={Items},@CommodityID={commodity},@TypeOfWoodID={typeOfWoodId}";
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

        //*********************** Itemwise Register Stock ******************************



        /// <summary>
        /// GetLoad Data  - same as  ===================== GetUnitwiseStockLoadData===============================
        /// </summary>
        /// <param name="item"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// 

        public CommonResponse GetItemStockRptLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var branch = FillAllBranch().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item,
                    Branch = branch
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }

        public CommonResponse FillItemStockRpt(ItemStockRpt itemStock) 
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                var locationId = itemStock.LocationID.Id != null ? itemStock.LocationID.Id.ToString() : "NULL";
                var Items = itemStock.ItemID.Id != null ? itemStock.ItemID.Id.ToString() : "NULL";
                var branchId = itemStock.BranchID.Id != null ? itemStock.BranchID.Id.ToString() : "NULL";
                cmd.CommandText = $"Exec ItemStockReportSP @Criteria='ItemStock',@Date='{itemStock.ToDate}',@BranchID={branchId},@LocationID={locationId},@ItemID={Items}";
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

        //*********************** Item Movement Analysis ******************************



        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// 

        public CommonResponse GetItemMovementAnalysisLoadData()
        {
            try
            {
                var item = GetItem().Data;
                var branch = FillAllBranch().Data;
                var commditti = GetCommodity().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item,
                    Branch = branch,
                    Commdity = commditti,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }

        public CommonResponse FillItemMovementAnalysis(ItemMovementAnalysis itemMovement)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                var locationId = itemMovement.LocationID.Id != null ? itemMovement.LocationID.Id.ToString() : "NULL";
                var Items = itemMovement.ItemID.Id != null ? itemMovement.ItemID.Id.ToString() : "NULL";
                var branchId = itemMovement.BranchID.Id != null ? itemMovement.BranchID.Id.ToString() : "NULL";
                var perc = itemMovement.Percentage != null ? itemMovement.Percentage.ToString() : "NULL";
                var commodity = itemMovement.CommodityID.Id != null ? itemMovement.CommodityID.Id.ToString() : "NULL";
                cmd.CommandText = $"Exec ItemMovementAnalysisSP @FromDate='{itemMovement.FromDate}',@ToDate='{itemMovement.ToDate}',@BranchID={branchId},@LocationID={locationId},@ItemID={Items},@Percentage={perc},@CommodityID={commodity}";
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

        //*********************** Unitwise Stock Report ******************************



        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// 

        public CommonResponse GetUnitwiseStkLoadData()
        {
            try
            {
                var item = GetItem().Data;
                _logger.LogInformation("Load Success");
                return CommonResponse.Ok(new
                {
                    Item = item
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }

        public CommonResponse FillUnitwiseStk(int itemId)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

               var Items = itemId != null ? itemId.ToString() : "NULL";
                var branchId = _authService.GetBranchId().Value;
                cmd.CommandText = $"Exec ItemMasterSP @Criteria='UnitwiseStock',@ID='{Items}',@BranchID={branchId}";
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
    }
}
