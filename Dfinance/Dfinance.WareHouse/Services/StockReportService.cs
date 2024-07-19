using Dfinance.Application.Services.General;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Warehouse.Services
{
    public class StockReportService : IStockReportService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<StockReportService> _logger;
        public StockReportService(DFCoreContext context, IAuthService authService, IHostEnvironment environment, ILogger<StockReportService> logger)
        {
            _authService = authService;
            _environment = environment;
            _logger = logger;
            _context = context;
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
       
        private CommonResponse FillTypeOfWoodForReports()
        {
            var branchId=_authService.GetBranchId();
            var location =(from tw in _context.CategoryType 
                           join c in _context.MaBranches on tw.CreatedBranchId equals c.Id
                           where tw.ActiveFlag==1 && c.BranchCompanyId ==branchId
                           orderby tw.Description
                           select new
                           {
                               Name = tw.Description,
                               Id=tw.Id
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
            var barcode = _context.ItemMaster.Where(i => i.BarCode != null && i.Active == true).Select(u => new { u.Id, Code=u.BarCode,Name=u.ItemName }).Distinct().ToList();
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
                var locationId = stockRegistration.LocationID == null ? "NULL " : stockRegistration.LocationID;
                var itemId = stockRegistration.ItemID != null ? stockRegistration.ItemID.ToString() : "NULL";
                var accountID = stockRegistration.AccountID != null ? stockRegistration.AccountID.ToString() : "NULL";
                var branchID = stockRegistration.BranchID != null ? stockRegistration.BranchID.ToString() : "NULL";
                var isItemWise = stockRegistration.IsItemwise == 0 ? false : true;
                var barcode = stockRegistration.Barcode != null ? stockRegistration.Barcode.ToString() : "NULL";
                var commityId = stockRegistration.CommodityID != null ? stockRegistration.CommodityID.ToString() : "NULL";
                var orginId = stockRegistration.OriginID != null ? stockRegistration.OriginID.ToString() : "NULL";
                var brandId = stockRegistration.BrandID != null ? stockRegistration.BrandID.ToString() : "NULL";
                var colorId = stockRegistration.ColorID != null ? stockRegistration.ColorID.ToString() : "NULL";
                var batchNo = stockRegistration.BatchNo != null ? stockRegistration.BatchNo.ToString() : "NULL";
                var supplierId = stockRegistration.SupplierID != null ? stockRegistration.SupplierID.ToString() : "NULL";
                var customerId = stockRegistration.CustomerID != null ? stockRegistration.CustomerID.ToString() : "NULL";
                var userId = _authService.GetId();
                var categoryId = stockRegistration.CategoryTypeID != null ? stockRegistration.CategoryTypeID.ToString() : "NULL";
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
                var type=FillTypeOfWoodForReports().Data;
                _logger.LogInformation("Sucessfully Load GetWarehouseStockLoadData");
                return CommonResponse.Ok(new { Location = locations ,Type= type});
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
                var branches=FillAllBranch().Data;
                var locations = FillVoucherLocations().Data;
                var item = GetItem().Data;
                _logger.LogInformation("Sucessfully Load GetWarehouseStockLoadData");
                return CommonResponse.Ok(new { Location = locations, Item = item, Branches=branches });
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
                var vType=stockReceiptIssue.VType.Value != null ? stockReceiptIssue.VType.Value.ToString() : "All";
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
                if(unitwiseStock.Barcode.Code!="" && unitwiseStock.Barcode.Code!=null)
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



        //*********************** Unitwise Stock ******************************

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
                    Units=unit
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
    }
}
