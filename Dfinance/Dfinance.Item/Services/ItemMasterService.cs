﻿using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory;
using Dfinance.Core.Views.Item;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using JsonDiffPatchDotNet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.Json;
using static Dfinance.Shared.Routes.InvRoute;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Dfinance.Item.Services.Inventory
{
    public class ItemMasterService : IItemMasterService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        IItemUnitsService _itemunitService;
        private static int reqCount = 0;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settings;
        private readonly ILogger<ItemMasterService> _logger;
        private readonly IUserTrackService _userTrack;
        private string uploadPath;
        public ItemMasterService(DFCoreContext context, IAuthService authService, IItemUnitsService itemunitService, IHostEnvironment environment,
           IConfiguration configuration, ISettingsService settings, ILogger<ItemMasterService> logger, IUserTrackService userTrack)
        {
            _context = context;
            _authService = authService;
            _itemunitService = itemunitService;
            _environment = environment;
            _configuration = configuration;
            _settings = settings;
            _logger = logger;
            _userTrack = userTrack;
            // Get upload path from app settings
            uploadPath = _configuration["AppSettings:UploadPath"];

        }

        private string base64RemoveData = "data:image/png;base64,";
        //called by ItemMasterController/FillItemMaster
        /******************* Fill Item Master (Left side Table) ********************/
        public CommonResponse FillItemMaster(int[]? catId, int[]? brandId, string search = null, int pageNo = 0, int limit = 0)
        {
            int branchId = _authService.GetBranchId().Value;
            if (catId.Length == 0 && brandId.Length == 0 && search == null && pageNo == 0)
            {
                var result = _context.SpFillItemMaster.FromSqlRaw($"Exec ItemMasterSP @Criteria='FillMasterWeb',@BranchID='{branchId}'").ToList();
                return CommonResponse.Ok(result);
            }
            string catIdString = (catId.Length > 0) ? string.Join(",", catId) : null;
            string bransIdString = brandId.Length > 0 ? string.Join(",", brandId) : null;
            string critera = "FillItemMasterWithBrandIdWeb";
            var data = _context.SpFillItemMaster.FromSqlRaw("Exec ItemMasterSP @Criteria={0},@BranchID={1},@SelectedBrandID={2},@SelectedCommodityID={3}," +
                "@Search={4},@PageNo={5},@Limit={6}",
               critera, branchId, bransIdString, catIdString, search, pageNo, limit).ToList();
            return CommonResponse.Ok(data);

        }

        /********************* Fill Items by Id **************************/
        public CommonResponse FillItemByID(int pageId, int Id, int BranchId = 0)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Items");
            }
            var itemId = _context.ItemMaster.Any(i => i.Id == Id);
            if (!itemId)
            {
                _logger.LogInformation(Id + " Item Not Found");
                return CommonResponse.NoContent("Item Not Exists");
            }

            string criteria = "FillItemByID";
            var itemdata = _context.SpFillItemMasterById.FromSqlRaw($"Exec ItemMasterSP @Criteria='{criteria}',@ID='{Id}'").AsEnumerable().FirstOrDefault();
            string? imagePath = null;
            if (itemdata.Imagepath != null)
                imagePath = uploadPath + itemdata?.Imagepath;

            string imageBase64 = null;

            if (!string.IsNullOrEmpty(imagePath) || File.Exists(imagePath))
            {
                // Read image data
                byte[] imageData = File.ReadAllBytes(imagePath);
                // Convert  to Base64 string
                imageBase64 = Convert.ToBase64String(imageData);
                imageBase64 = base64RemoveData + imageBase64;
            }
            var s = _settings.GetSettings("BranchwiseItem").Data;

            if (!Convert.ToBoolean(s) || BranchId == 0)
            {
                BranchId = _authService.GetBranchId().Value;
            }
            var unitdata = _itemunitService.FillItemUnits(Id, BranchId);
            var itemhistory = FillItemHistory(Id, BranchId);
            var itemstock = GetCurrentStock(Id, BranchId);
            _logger.LogInformation("Item Details loaded Successfully");
            return CommonResponse.Ok(new { item = itemdata, img = imageBase64, unit = unitdata, history = itemhistory, stock = itemstock });

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

        //called by ItemMasterController/GetNextItemCode
        //Generate next ItemCode
        public CommonResponse GetNextItemCode()
        {

            return CommonResponse.Ok(GenerateCode());
        }

        private string GenerateCode()
        {
            string criteria = "GetNextItemCode";
            return _context.ItemNextCode.FromSqlRaw($"Exec ItemMasterSP @Criteria ='{criteria}'")
                .AsEnumerable()
                .Select(x => x.Result)
                .FirstOrDefault();
        }


        //called by ItemMasterController/ParentItemPopup
        //PopUp for Parent Item
        public CommonResponse ParentItemPopup()
        {
            var data = _context.ItemMaster
                .Where(i => i.IsGroup == true && i.Active == true)
                .Select(i => new ParentItemPoupView
                {
                    ID = i.Id,
                    ItemCode = i.ItemCode,
                    ItemName = i.ItemName
                }).ToList();
            return CommonResponse.Ok(data);
        }

        //Branchwise TaxType Dropdown
        public CommonResponse TaxDropDown()
        {
            int branchId = _authService.GetBranchId().Value;
            string criteria = "FillBranchWiseTax";
            var data = _context.TaxDropDownView.FromSqlRaw($"Exec DropDownListSP @Criteria='{criteria}',@BranchID='{branchId}'").ToList();
            return CommonResponse.Ok(data);
        }

        //Generate Barcode
        public CommonResponse GenerateBarCode()
        {
            var Barcode = _context.BarcodeView.FromSqlRaw($"Exec ItemMasterSP @Criteria='BarCodeFromInvItemUnits'").ToList();
            return CommonResponse.Ok(Barcode);
        }
        // //checking whether the itemcode is valid or not
        private bool CheckItemCode(string itemcode)
        {
            var nextcode = GenerateCode();
            if (nextcode != itemcode)
            {
                if (!(itemcode.Any(char.IsLetter)))
                {
                    var diff = Math.Abs(itemcode.Length - nextcode.Length);
                    if (diff > 2)
                    {
                        if (reqCount == 1)
                            reqCount = 0;
                        else
                        {
                            reqCount = 1;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private string UploadImage(string base64EncodedData, string itemCode)
        {
            //  base64EncodedData = base64EncodedData.Replace(" ", "+");
            // Assuming base64RemoveData is defined somewhere in your code
            base64EncodedData = base64EncodedData.Replace(base64RemoveData, "");
            byte[] imageData = Convert.FromBase64String(base64EncodedData);


            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Construct image paths
            string imagePath = Path.Combine(uploadPath, $"{itemCode}.jpg");
            string imagePathDb = itemCode + ".jpg";

            // Write image data to file
            File.WriteAllBytes(imagePath, imageData);
            _logger.LogInformation("Item Image uploaded Successfully");
            return imagePathDb;
        }

        //save itemmaster,itemunits
        //called by ItemMasterController/SaveItemMaster
        public CommonResponse SaveItemMaster(ItemMasterDto itemDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Items");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                int CreatedBy = _authService.GetId().Value;
                int BranchId = _authService.GetBranchId().Value;
                //int pageId = 55;
                int method = 8;
                //get the Settings value for CommodityMandatory to check whether it true or false
                var catMandSetting = _settings.GetSettings("CommodityMandatory").Data;

                if (catMandSetting == "True" || catMandSetting == "0")
                {
                    //if the Category is null show mandatory message
                    if (itemDto.Category.ID == 0)
                    {
                        _logger.LogInformation("Commodity field is mandatory, but it not entered");
                        return CommonResponse.Error("Commodity is Mandatory");
                    }

                }
                //check dropdowns and popups are null or not
                var catId = _context.Category.Any(t => t.Id == itemDto.Category.ID);
                var taxId = _context.TaxType.Any(t => t.Id == itemDto.TaxType.Id);
                var parentId = _context.ItemMaster.Any(i => i.Id == itemDto.Parent.ID);
                var qualityId = _context.MaMisc.Any(m => m.Id == itemDto.Quality.Id);
                var colorId = _context.MaMisc.Any(m => m.Id == itemDto.Color.Id);
                var countryId = _context.MaMisc.Any(m => m.Id == itemDto.CountryOfOrigin.Id);
                var brandId = _context.MaMisc.Any(m => m.Id == itemDto.Brand.Id);
                var sellunit = _context.UnitMaster.Any(u => u.Unit == itemDto.SellingUnit.Unit);
                var purunit = _context.UnitMaster.Any(u => u.Unit == itemDto.PurchaseUnit.Unit);
                var invacc = _context.FiMaAccounts.Any(c => c.Id == itemDto.InvAccount.Id);
                var costacc = _context.FiMaAccounts.Any(c => c.Id == itemDto.CostAccount.Id);
                var puracc = _context.FiMaAccounts.Any(c => c.Id == itemDto.PurchaseAccount.Id);
                var salacc = _context.FiMaAccounts.Any(c => c.Id == itemDto.SalesAccount.Id);

                string msg = null;

                //check whether the item already exists
                var obj = _context.ItemMaster.Any(i => i.ItemCode == itemDto.ItemCode);
                if (obj)
                {
                    _logger.LogInformation("ItemCode Already exists");
                    msg = "Item Code Already Exists";
                    return CommonResponse.Error(msg);
                }

                //check itemcode valid or not
                bool checkitemcode = CheckItemCode(itemDto.ItemCode);
                if (checkitemcode == false)
                    return CommonResponse.Error("Warning:Do you want to change ItemCode??");

                //upload image

                string? path = null;
                if (itemDto.ImageFile != null)
                {
                    path = UploadImage(itemDto.ImageFile, itemDto.ItemCode);
                }

                //path = "";


                //saving itemmaster table
                string criteria = "Insert";
                SqlParameter newIdItem = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var data = _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@ItemCode={1},@ItemName={2},@SellingPrice={3},@OEMNo={4},@PartNo={5}," +
                    "@CategoryID={6},@Manufacturer={7},@BarCode={8},@ModelNo={9},@Unit={10},@ROL={11},@Remarks={12},@IsGroup={13},@StockItem={14},@Active={15}," +
                    "@InvAccountID={16},@CostAccountID={17},@PurchaseAccountID={18},@SalesAccountID={19},@Stock={20},@InvoicedStock={21},@AvgCost={22}," +
                    "@LastCost={23},@CreatedDate ={24},@ModifiedDate={25},@CreatedUserID={26},@ModifiedUserID={27},@BranchID={28},@Location={29}," +
                    "@CashPrice={30},@CreditPrice={31},@ROQ={32},@CommodityID={33},@ShipMark={34},@PaintMark={35},@QualityID={36},@Weight={37},@ParentID={38}," +
                    "@PurchaseRate={39},@Margin={40},@ImagePath={41},@TaxTypeID={42},@SizeItem={43},@ColorID={44},@BrandID={45},@OriginID={46},@IsExpiry={47}," +
                    "@ExpiryPeriod={48},@BarcodeDesignID={49},@IsFinishedGood= {50},@IsRawMaterial={51},@IsUniqueItem={52},@PurchaseUnit={53}," +
                    "@SellingUnit={54},@MarginValue={55},@ArabicName={56},@HSN={57},@ItemDisc={58},@MRP={59},@NewID={60} OUTPUT",
                    criteria,
                    itemDto.ItemCode,
                    itemDto.ItemName,
                    (itemDto.SellingPrice != null) ? Math.Round(itemDto.SellingPrice.Value, 4) : null,
                    itemDto.OEMNo,
                    itemDto.PartNo,
                    null,//CategoryId,   
                    itemDto.Manufacturer,
                    itemDto.BarCode,
                    itemDto.ModelNo,
                    itemDto.Unit.Unit,
                    (itemDto.ROL != null) ? Math.Round(itemDto.ROL.Value, 4) : null,
                    itemDto.Remarks,
                    itemDto.IsGroup,
                    itemDto.StockItem,
                    itemDto.Active,
                    invacc ? itemDto.InvAccount.Id : null,
                    costacc ? itemDto.CostAccount.Id : null,
                    puracc ? itemDto.PurchaseAccount.Id : null,
                    salacc ? itemDto.SalesAccount.Id : null,
                    null, null, null, null, //stock,invoicedstock,avgcost,lastcost                 
                    DateTime.Now,
                    DateTime.Now,
                    CreatedBy,
                    CreatedBy,
                    BranchId,
                    itemDto.Location,
                    null, null,//cashprice,creditprice
                    (itemDto.ROQ != null) ? Math.Round(itemDto.ROQ.Value, 4) : null,
                    itemDto.Category.ID,
                    itemDto.ShipMark,
                    itemDto.PaintMark,
                    qualityId ? itemDto.Quality.Id : null,
                    (itemDto.Weight != null) ? Math.Round(itemDto.Weight.Value, 4) : null,
                    parentId ? itemDto.Parent.ID : null,
                    (itemDto.CostPrice != null) ? Math.Round(itemDto.CostPrice.Value, 4) : null,
                    (itemDto.Margin != null) ? Math.Round(itemDto.Margin.Value, 4) : null,
                    path,//imagepath         
                    taxId ? itemDto.TaxType.Id : null,
                    null,//sizeitem          
                    colorId ? itemDto.Color.Id : null,
                    brandId ? itemDto.Brand.Id : null,
                    countryId ? itemDto.CountryOfOrigin.Id : null,
                    itemDto.IsExpiry,
                    itemDto.ExpiryPeriod,
                    null,//barcodedesignId,  
                    itemDto.IsFinishedGood,
                    itemDto.IsRawMaterial,
                    itemDto.IsUniqueItem,
                    purunit ? itemDto.PurchaseUnit.Unit : null,
                    sellunit ? itemDto.SellingUnit.Unit : null,
                    (itemDto.MarginValue != null) ? Math.Round(itemDto.MarginValue.Value, 4) : null,
                    itemDto.ArabicName,
                    itemDto.HSN,
                    (itemDto.ItemDisc != null) ? Math.Round(itemDto.ItemDisc.Value, 4) : null,
                    (itemDto.MRP != null) ? Math.Round(itemDto.MRP.Value, 4) : null,
                    newIdItem
                    );
                var NewItemId = (int)newIdItem.Value;
                var multirate = SaveItemMultiRate(NewItemId);
                var branch = new List<int>();


                var ItemBranchSettings = (bool)_settings.GetSettings("BranchwiseItem").Data;
                bool HigherApprove = _authService.UserPermCheck(pageId, method);


                if (ItemBranchSettings && HigherApprove)
                {
                    if (itemDto.Branch.Count > 0)
                        foreach (var b in itemDto.Branch)
                            branch.Add(b.Id.Value);
                }
                else
                    branch.Add(BranchId);

                foreach (var b in branch)
                {
                    var branchitemsdata = SaveBranchItems(b, NewItemId, itemDto); //saving branchitems
                }
                if (itemDto.ItemUnit != null && itemDto.ItemUnit.Count > 0)
                {
                    var unisdata = _itemunitService.SaveItemUnits(itemDto.ItemUnit, branch, NewItemId);
                }
                transaction.Commit();
                var jsonItemSave = JsonSerializer.Serialize(itemDto);
                _userTrack.AddUserActivity(itemDto.ItemCode, NewItemId, 0, "Added", "InvItemMaster", "Item Master", 0, jsonItemSave);
                _logger.LogInformation("Item Saved Successfully");
                return CommonResponse.Created(new { msg = "Item " + itemDto.ItemName + " Created Successfully", data = 0 });
            }
        }

        //saving BranchItems//called by SaveItemMaster() 
        private CommonResponse SaveBranchItems(int branch, int ItemId, ItemMasterDto itemDto)
        {
            bool active = true;
            int BranchId = _authService.GetBranchId().Value;
            SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            if (branch == BranchId)
                active = true;

            string criteria = "InsertInvBranchItems";
            var data = _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@BranchID={1},@ItemID={2},@Active={3},@ROL={4},@ROQ={5},@TaxTypeID={6},@Location={7},@PurchaseRate={8},@SellingPrice={9},@NewID={10} OUTPUT",
                 criteria,
                 branch,
                 ItemId,
                 active,
                 itemDto.ROL,
                 itemDto.ROQ,
                 itemDto.TaxType.Id,
                 itemDto.Location,
                 itemDto.CostPrice,
                 itemDto.SellingPrice,
                 newId);
            _logger.LogError("BranchItem Saved");
            return CommonResponse.Created(data);
        }


        //update itemmaster,itemunits
        //called by ItemMasterController/UpdateItemMaster
        public CommonResponse UpdateItemMaster(ItemMasterDto itemDto, int ItemId, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Edit Items");
            }
            var beforeUpdate = _context.ItemMaster.AsNoTracking().FirstOrDefault(i => i.Id == ItemId);
            var jsonBeforeUpdate = JsonSerializer.Serialize(beforeUpdate);
            using (var transaction = _context.Database.BeginTransaction())
            {

                //check dropdowns and popups are null or not
                var taxId = _context.TaxType.Any(t => t.Id == itemDto.TaxType.Id);
                var parentId = _context.ItemMaster.Any(i => i.Id == itemDto.Parent.ID);
                var qualityId = _context.MaMisc.Any(m => m.Id == itemDto.Quality.Id);
                var colorId = _context.MaMisc.Any(m => m.Id == itemDto.Color.Id);
                var countryId = _context.MaMisc.Any(m => m.Id == itemDto.CountryOfOrigin.Id);
                var brandId = _context.MaMisc.Any(m => m.Id == itemDto.Brand.Id);
                var sellunit = _context.UnitMaster.Any(u => u.Unit == itemDto.SellingUnit.Unit);
                var purunit = _context.UnitMaster.Any(u => u.Unit == itemDto.PurchaseUnit.Unit);
                var invacc = _context.FiMaAccounts.Any(c => c.Id == itemDto.InvAccount.Id);
                var costacc = _context.FiMaAccounts.Any(c => c.Id == itemDto.CostAccount.Id);
                var puracc = _context.FiMaAccounts.Any(c => c.Id == itemDto.PurchaseAccount.Id);
                var salacc = _context.FiMaAccounts.Any(c => c.Id == itemDto.SalesAccount.Id);
                itemDto.StockItem = true;


                var item = _context.ItemMaster.Any(item => item.Id == ItemId);
                if (item == false)
                    return CommonResponse.NotFound("Item Not Exists");

                var obj = _context.ItemMaster.
                    FirstOrDefault(i => i.ItemCode == itemDto.ItemCode && i.Id != ItemId);
                if (obj != null)
                {
                    _logger.LogInformation("ItemCode Already Exists");
                    return CommonResponse.Error("Item Code Already Exists");
                }
                var catMandSetting = _settings.GetSettings("CommodityMandatory").Data;

                if (catMandSetting == "True" || catMandSetting == "0")
                {
                    //if the Category is null show mandatory message
                    if (itemDto.Category.ID == 0)
                    {
                        _logger.LogInformation("Commodity field is mandatory, but it not entered");
                        return CommonResponse.Error("Commodity is Mandatory");
                    }

                }

                int CreatedBy = _authService.GetId().Value;
                int BranchId = _authService.GetBranchId().Value;

                //check itemcode valid or not
                bool checkitemcode = CheckItemCode(itemDto.ItemCode);
                if (checkitemcode == false)
                    return CommonResponse.Error("Warning:Do you want to change ItemCode??");

                //upload image
                string? path = null;
                if (itemDto.ImageFile != null)
                {
                    path = UploadImage(itemDto.ImageFile, itemDto.ItemCode);
                }

                //update itemmaster
                string criteria = "Update";
                _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@ItemCode={1},@ItemName={2},@SellingPrice={3},@OEMNo={4},@PartNo={5}," +
                  "@CategoryID={6},@Manufacturer={7},@BarCode={8},@ModelNo={9},@Unit={10},@ROL={11},@Remarks={12},@IsGroup={13},@StockItem={14},@Active={15}," +
                  "@InvAccountID={16},@CostAccountID={17},@PurchaseAccountID={18},@SalesAccountID={19},@Stock={20},@InvoicedStock={21},@AvgCost={22}," +
                  "@LastCost={23},@CreatedDate ={24},@ModifiedDate={25},@CreatedUserID={26},@ModifiedUserID={27},@BranchID={28},@Location={29}," +
                  "@CashPrice={30},@CreditPrice={31},@ROQ={32},@CommodityID={33},@ShipMark={34},@PaintMark={35},@QualityID={36},@Weight={37},@ParentID={38}," +
                  "@PurchaseRate={39},@Margin={40},@ImagePath={41},@TaxTypeID={42},@SizeItem={43},@ColorID={44},@BrandID={45},@OriginID={46},@IsExpiry={47}," +
                  "@ExpiryPeriod={48},@BarcodeDesignID={49},@IsFinishedGood= {50},@IsRawMaterial={51},@IsUniqueItem={52},@PurchaseUnit={53}," +
                  "@SellingUnit={54},@MarginValue={55},@ArabicName={56},@HSN={57},@ItemDisc={58},@MRP={59},@ID={60}",
                   criteria,
                        itemDto.ItemCode,
                        itemDto.ItemName,
                        (itemDto.SellingPrice != null) ? Math.Round(itemDto.SellingPrice.Value, 4) : null,
                        itemDto.OEMNo,
                        itemDto.PartNo,
                        null,//CategoryId,   
                        itemDto.Manufacturer,
                        itemDto.BarCode,
                        itemDto.ModelNo,
                        itemDto.Unit.Unit,
                        (itemDto.ROL != null) ? Math.Round(itemDto.ROL.Value, 4) : null,
                        itemDto.Remarks,
                        (itemDto.IsGroup == null) ? 0 : 1,
                        (itemDto.StockItem == null) ? 0 : 1,
                        (itemDto.Active == null) ? 0 : 1,
                        invacc ? itemDto.InvAccount.Id : null,
                        costacc ? itemDto.CostAccount.Id : null,
                        puracc ? itemDto.PurchaseAccount.Id : null,
                        salacc ? itemDto.SalesAccount.Id : null,
                        null, null, null, null, //stock,invoicedstock,avgcost,lastcost                 
                        DateTime.Now,
                        DateTime.Now,
                        CreatedBy,
                        CreatedBy,
                        BranchId,
                        itemDto.Location,
                        null, null,//cashprice,creditprice
                        (itemDto.ROQ != null) ? Math.Round(itemDto.ROQ.Value, 4) : null,
                        itemDto.Category.ID,
                        itemDto.ShipMark,
                        itemDto.PaintMark,
                        qualityId ? itemDto.Quality.Id : null,
                        (itemDto.Weight != null) ? Math.Round(itemDto.Weight.Value, 4) : null,
                        parentId ? itemDto.Parent.ID : null,
                        (itemDto.CostPrice != null) ? Math.Round(itemDto.CostPrice.Value, 4) : null,
                        (itemDto.Margin != null) ? Math.Round(itemDto.Margin.Value, 4) : null,
                        path,//imagepath         
                        taxId ? itemDto.TaxType.Id : null,
                        null,//sizeitem          
                        colorId ? itemDto.Color.Id : null,
                        brandId ? itemDto.Brand.Id : null,
                        countryId ? itemDto.CountryOfOrigin.Id : null,
                        itemDto.IsExpiry,
                        itemDto.ExpiryPeriod,
                        null,//barcodedesignId,  
                        itemDto.IsFinishedGood,
                        itemDto.IsRawMaterial,
                        itemDto.IsUniqueItem,
                        purunit ? itemDto.PurchaseUnit.Unit : null,
                        sellunit ? itemDto.SellingUnit.Unit : null,
                        (itemDto.MarginValue != null) ? Math.Round(itemDto.MarginValue.Value, 4) : null,
                        itemDto.ArabicName,
                        itemDto.HSN,
                        (itemDto.ItemDisc != null) ? Math.Round(itemDto.ItemDisc.Value, 4) : null,
                        (itemDto.MRP != null) ? Math.Round(itemDto.MRP.Value, 4) : null,
                   ItemId

                  );

                var branch = new List<int>();
                var ItemBranchSettings = (bool)_settings.GetSettings("BranchwiseItem").Data;
                object result;

                bool HigherApprove = _authService.UserPermCheck(pageId, 8);
                // Check if ItemBranchSettings is true or 1
                if (ItemBranchSettings && HigherApprove)
                {
                    if (itemDto.Branch != null)
                        foreach (var b in itemDto.Branch)
                            branch.Add(b.Id.Value);
                }
                else
                    branch.Add(BranchId);

                var bi = _context.BranchItems.Where(b => b.ItemId == ItemId).ToList();//updating branchItems
                if (bi != null)
                {
                    _context.BranchItems.RemoveRange(bi);
                    _context.SaveChanges();
                    foreach (var b in branch)
                    {
                        var branchitemsdata = SaveBranchItems(b, ItemId, itemDto);
                    }
                }


                if (itemDto.ItemUnit != null && itemDto.ItemUnit.Count > 0)
                {
                    var unitsToRemove = _context.ItemUnits.Where(u => u.ItemId == ItemId).ToList();
                    _context.ItemUnits.RemoveRange(unitsToRemove);
                    _context.SaveChanges();
                    var unisdata = _itemunitService.SaveItemUnits(itemDto.ItemUnit, branch, ItemId);
                }

                transaction.Commit();

                var afterUpdate = _context.ItemMaster.AsNoTracking().FirstOrDefault(i => i.Id == ItemId);
                var jsonAfterUpdate = JsonSerializer.Serialize(afterUpdate);
                var diffObj = new JsonDiffPatch();
                var jsonDifference = diffObj.Diff(jsonBeforeUpdate, jsonAfterUpdate);
                _userTrack.AddUserActivity(itemDto.ItemCode, ItemId, 1, "Modified", "InvItemMaster", "Item Master", 0, jsonDifference);
                _logger.LogInformation("Item Updated successfully");
                return CommonResponse.Ok("Item Updated successfully");
            }
        }

        //called by SaveItemMaster()
        private CommonResponse SaveItemMultiRate(int ItemId)
        {
            var PriceCatId = _context.MaPriceCategory
                            .Select(c => new { c.Id, c.Perc })
                            .ToList();
            string Criteria = "InsertInvItemMultiRate";
            foreach (var p in PriceCatId)
            {
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@ItemID={1},@PriceCategoryID={2},@SalesPerc={3},@SalesDiscountPerc={4}," +
               "@PurchaseRate={5},@SalePrice={6},@NewID={7} OUTPUT",
               Criteria,
               ItemId,
               p.Id,
               p.Perc,
               null, null, null,
               newId
               );
            }
            _logger.LogInformation("ItemMultiRate Saved Successfully");
            return CommonResponse.Ok();
        }

        //delete itemmaster and itemunits
        public CommonResponse DeleteItem(int ItemId, int pageId)
        {

            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 5))
            {
                return PermissionDenied("Delete Items");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                var check = _context.ItemMaster.Any(i => i.Id == ItemId);
                if (check == false)
                {
                    _logger.LogInformation(ItemId + " Item Not Exists");
                    return CommonResponse.NotFound("Item Not Exists");
                }

                var unitId = _context.ItemUnits.Where(i => i.ItemId == ItemId).Select(i => i.Id).ToList();
                var branchitems = _context.BranchItems.Where(i => i.ItemId == ItemId).Select(i => i.Id).ToList();
                var itemData = _context.ItemMaster.Where(i => i.Id == ItemId).Select(i => new { i.ItemCode, i.ItemName }).FirstOrDefault();
                //check whether the imagepath exists
                string imagepath = _context.ItemMaster.Where(i => i.Id == ItemId).Select(i => i.ImagePath).FirstOrDefault();
                if (!string.IsNullOrEmpty(imagepath) && System.IO.File.Exists(imagepath))
                    System.IO.File.Delete(imagepath);//delete imagepath from the folder

                var del = _context.Database.ExecuteSqlRaw($"Exec ItemMasterSP @Criteria='Delete',@ID='{ItemId}'");

                //for deleting corresponding itemunits                   
                foreach (var u in unitId)
                {
                    _itemunitService.DeleteItemUnits(u);
                }

                //for deleting corresponding branchitems 
                foreach (var b in branchitems)
                {
                    _context.Database.ExecuteSqlRaw($"Exec ItemMasterSP @Criteria='DeleteInvBranchItems',@ID='{b}'");
                }
                transaction.Commit();

                _userTrack.AddUserActivity(itemData.ItemCode, ItemId, 1, "Deleted", "InvItemMaster", "Item Master", 0, itemData.ItemName + " Deleted");
                _logger.LogInformation(ItemId + " Item Deleted");
                return CommonResponse.Ok(new { msg = "Item Details Deleted successfully", data = 0 });
            }
        }

        //fill ItemHistory//called by FillItemHistory in ItemMasterController
        private CommonResponse FillItemHistory(int ItemId, int BranchId)
        {
            //int BranchId = _authService.GetBranchId().Value;
            string criteria = "FillItemHistory";
            var result = _context.ItemHistoryView.FromSqlRaw($"Exec ItemMasterSP @Criteria='{criteria}',@BranchID='{BranchId}',@ItemID='{ItemId}'").ToList();
            return CommonResponse.Ok(result);
        }

        private CommonResponse GetCurrentStock(int ItemId, int BranchId)
        {
            //var BranchId= _authService.GetBranchId().Value;
            // StockQtyOnDate SQL function calling 
            var result = _context.CurrentStockView.FromSqlRaw($"select dbo.StockQtyOnDate('{ItemId}','{BranchId}',null,null)").ToList();
            return CommonResponse.Ok(result);
        }
        class abc
        {
            public int id { get; set; }
            public decimal? Rate { get; set; }
            public string PriceCategory { get; set; }
            public decimal Perc { get; set; }
        }
        //for price category popup in item grid
        private object PriceCategoryPopup(int itemId)
        {//lin
            var result = _context.MaPriceCategory.Include(x => x.MultiRate)

                  .Where(pc => (bool)pc.Active) // Active price categories
           .Select(pc => new abc()
           {
               id = pc.Id,
               PriceCategory = pc.Name,
               Perc = pc.Perc,
               Rate = pc.MultiRate.Where(x => x.Id == itemId && x.PriceCategoryId == pc.Id).Select(x => x.PurchaseRate).FirstOrDefault()
           }).ToList();

            return result;
        }
        /// <summary>
        /// fill transaction items
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="PageID"></param>
        /// <param name="locId"></param>
        /// <returns></returns>
        /// //used in item grid in inventory transaction pages
        /// //return whether the item is Unique or not, expiry or not
        /// //return tool tip data for each item(in transaction pages)
        public CommonResponse FillTransItems(Object PartyId = null, Object PageID = null, Object LocationID = null, Object VoucherID = null,
            Object PrimaryVoucherID = null, Boolean? IsSizeItem = null, Boolean IsMargin = false, Object ItemID = null, Boolean ISTransitLoc = false,
           Boolean IsFinishedGood = false, Boolean IsRawMaterial = false, Object ModeID = null, DateTime? VoucherDate = null,
            Object TransactionID = null, string Criteria = null)
        {

            int? userId = _authService.GetId();
            if (Criteria == null)
                Criteria = "ItemCommandText";
            int branchId = _authService.GetBranchId().Value;
            object uniqueExpiry = 0;
            object units = 0;
            var finishedGood = IsFinishedGood ? "1" : "null";
            var rawMaterial = IsRawMaterial ? "1" : "null";

            var primaryVoucherIDStr = PrimaryVoucherID != null ? PrimaryVoucherID.ToString() : "NULL";
            var partyIDStr = PartyId != null ? PartyId.ToString() : "NULL";
            var LocIDStr = LocationID != null ? LocationID.ToString() : "NULL";
            var VoucherIDStr = VoucherID != null ? VoucherID.ToString() : "NULL";
            var ItemIDStr = ItemID != null ? ItemID.ToString() : "NULL";
            var ModIDStr = ModeID != null ? ModeID.ToString() : "NULL";
            var PageIDStr = PageID != null ? PageID.ToString() : "NULL";
            var TransIDStr = TransactionID != null ? TransactionID.ToString() : "NULL";
            var branchID=branchId!=null? branchId.ToString() : "NULL";

            if (VoucherDate == null)
                VoucherDate = DateTime.Now;
            string voucherDate=VoucherDate.Value.ToString("yyyy-MM-dd");
            var result = _context.CommandTextView
                 .FromSqlRaw($"select dbo.GetCommandText('{Criteria}',{primaryVoucherIDStr},{branchID},{partyIDStr},{LocIDStr},'{IsSizeItem}','{IsMargin}',{VoucherIDStr},{ItemIDStr},'{ISTransitLoc}',{finishedGood},{rawMaterial},{ModIDStr},{PageIDStr},'{voucherDate}',{TransIDStr},{userId})")
                 .ToList();

            var res = result.FirstOrDefault();
            var data = _context.TransItemsView.FromSqlRaw(res.commandText).ToList();
            var itemsWithExpiry = new List<object>();
            string criteria1 = "GetLastItemRate";
            object prevTransData;
            bool? uniqueItem = false, expireItem = false;
            object expiryItem = null;
            var uniqueNo = (bool)_settings.GetSettings("SetUniqueNo").Data;
            var expiry = (bool)_settings.GetSettings("IsExpiryDate").Data;

            foreach (var item in data)
            {
                if (uniqueNo)
                    uniqueItem = _context.ItemMaster.Where(i => i.Id == item.ID).Select(i => i.IsUniqueItem).FirstOrDefault();//returns whether the item is uniqueItem 
                if (expiry)
                    expiryItem = _context.ItemMaster.Where(i => i.Id == item.ID).Select(i => new { i.ExpiryPeriod, i.IsExpiry }).FirstOrDefault();
                //returns the Isexpiry , expiry period of item

                units = _itemunitService.GetItemUnits(item.ID).Data;//for unit popup in itemgrid

              
                prevTransData = GetPreviousItemData(item.ID, PartyId, VoucherID).Data;
               // prevTransData = _context.ItemTransaction.FromSqlRaw($"Exec VoucherAdditionalsSP @Criteria='{criteria1}',@BranchID='{branchId}',@ItemID='{item.ID}',@AccountID='{PartyId}',@VoucherID='{VoucherID}'").ToList();
                var updatePrice = _itemunitService.FillItemUnits(item.ID, branchId).Data;
                var PriceCat = PriceCategoryPopup(item.ID);
                itemsWithExpiry.Add(new
                {
                    Item = item,
                    UnitPopup = units,
                    UniqueItem = uniqueItem,
                    ExpiryItem = expiryItem,
                    PreviousTransData = prevTransData,
                    UpdatePrice = updatePrice,
                    PriceCategory = PriceCat
                });
            }
            return CommonResponse.Ok(new { Items = itemsWithExpiry });
        }


        /// <summary>
        /// inv=>Report=>ItemSearch
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public CommonResponse GetItemSearch(int? itemId, string? value, string? criteria)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;

                var query = new StringBuilder();
                query.Append("Exec ItemSearchItemSP ");
                query.AppendFormat("@BranchID = {0}, ", branchId);
                query.AppendFormat("@Value = '{0}'", value ?? string.Empty);

                if (itemId.HasValue && itemId != 0)
                {
                    query.AppendFormat(", @ItemID = {0}", itemId.Value);
                }

                if (!string.IsNullOrEmpty(criteria))
                {
                    query.AppendFormat(", @Criteria = '{0}'", criteria);
                }
                if (value == "" || value==null)
                {
                    var result = _context.ItemSearchView.FromSqlRaw(query.ToString()).ToList();
                    return CommonResponse.Ok(result);
                }
                else
                {
                    var result = _context.RelatedItemSearchView.FromSqlRaw(query.ToString()).ToList();
                    return CommonResponse.Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        /// <summary>
        /// inv=>Report=>ItemRegister
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="WarehouseID"></param>
        /// <param name="Less"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public CommonResponse GetItemRegister(int? branchId, int? warehouseId, bool less = false, DateTime? date = null)
        {
            try
            {
                object result = null;
                var query = new StringBuilder();
                query.Append("Exec ItemCatalogueSP ");
                query.AppendFormat("@BranchID = {0}, ", branchId ?? 0);
                query.AppendFormat("@WarehouseID = {0}, ", warehouseId ?? 0);
                query.AppendFormat("@Less = {0}", less ? 1 : 0);

                if (date.HasValue)
                {
                    query.AppendFormat(", @Date = '{0}'", date.Value.ToString("yyyy-MM-dd"));
                }


                if (less == true)
                {
                    result = _context.ItemCatalogueViews.FromSqlRaw(query.ToString()).ToList();
                }
                else
                {
                    result = _context.ItemCatalogueView.FromSqlRaw(query.ToString()).ToList();
                }

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="BranchID"></param>
        /// <param name="OpeningBalance"></param>
        /// <param name="VoucherID"></param>
        /// <param name="UserID"></param>
        /// <param name="Nature"></param>
        /// <returns></returns>
        public CommonResponse GetInventoryAgeing(int? AccountID, DateTime? FromDate, DateTime? ToDate, bool? OpeningBalance, int? VoucherID, string? Nature)
        {
            try
            {
                int userId = _authService.GetId().Value;
                int? branchId = _authService.GetBranchId().Value;
                var result = _context.InventoryAgeingView.FromSqlRaw($"Exec AccountStatementAgingSP @DateFrom='{FromDate}',@BranchID='{branchId}',@AccountID='{AccountID}',@UserID='{userId}',@Nature='{Nature}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// GetItemExpiryReport
        /// </summary>
        /// <param name="itemExpiryReportDto"></param>
        /// <returns></returns>
        public CommonResponse GetItemExpiryReport(ItemExpiryReportDto itemExpiryReportDto)
        {
            try
            {
                int? branchid = _authService.GetBranchId().Value;
                var query = new StringBuilder();
                query.Append("Exec ItemExpiryReportSP ");

                query.AppendFormat("@BranchID = {0}, ", branchid ?? 0);

                if (itemExpiryReportDto.StartDate != null && itemExpiryReportDto.StartDate != DateTime.MinValue)
                {
                    query.AppendFormat("@DateFrom = '{0}', ", itemExpiryReportDto.StartDate.ToString("yyyy-MM-dd"));
                }

                if (itemExpiryReportDto.EndDate != null && itemExpiryReportDto.EndDate != DateTime.MinValue)
                {
                    query.AppendFormat("@DateUpto = '{0}', ", itemExpiryReportDto.EndDate.ToString("yyyy-MM-dd"));
                }

                if (itemExpiryReportDto.ExpiryDate != null && itemExpiryReportDto.ExpiryDate != DateTime.MinValue)
                {
                    query.AppendFormat("@ExpiryDate = '{0}', ", itemExpiryReportDto.ExpiryDate.ToString("yyyy-MM-dd"));
                }

                if (itemExpiryReportDto.Item.Id != 0)
                {
                    query.AppendFormat("@ItemID = {0}, ", itemExpiryReportDto.Item.Id);
                }

                if (itemExpiryReportDto.Barcode.Id != 0)
                {
                    query.AppendFormat("@Barcode = '{0}', ", itemExpiryReportDto.Barcode?.Id);
                }

                if (itemExpiryReportDto.Origin.Id != 0)
                {
                    query.AppendFormat("@OriginID = {0}, ", itemExpiryReportDto.Origin.Id);
                }

                if (itemExpiryReportDto.Brand.Id != 0)
                {
                    query.AppendFormat("@BrandID = {0}, ", itemExpiryReportDto.Brand.Id);
                }

                if (itemExpiryReportDto.Commodity.Id != 0)
                {
                    query.AppendFormat("@CommodityID = {0}, ", itemExpiryReportDto.Commodity.Id);
                }

                if (itemExpiryReportDto.Color.Id != 0)
                {
                    query.AppendFormat("@ColorID = {0}, ", itemExpiryReportDto.Color.Id);
                }

                if (itemExpiryReportDto.ExpiryDays != 0)
                {
                    query.AppendFormat("@Days = {0}, ", itemExpiryReportDto.ExpiryDays);
                }

                if (query.ToString().EndsWith(", "))
                {
                    query.Length -= 2;
                }

                var result = _context.ItemExpiryReportView.FromSqlRaw(query.ToString()).ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// GetInventoryProfitSP
        /// </summary>
        /// <param name="ViewBy"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Customer"></param>
        /// <param name="Detailed"></param>
        /// <param name="Item"></param>
        /// <param name="Salesman"></param>
        /// <returns></returns>
        public CommonResponse GetInventoryProfitSP(string? ViewBy, DateTime StartDate, DateTime EndDate, int? Customer, bool? Detailed, int Item, string? Salesman, int? AccountId)
        {
            try
            {
                object result = null;
                int? branchid = _authService.GetBranchId().Value;
                var query = new StringBuilder();
                query.Append("Exec InventoryProfitSP ");
                query.AppendFormat("@Criteria = {0}, ", ViewBy);
                query.AppendFormat("@BranchID = {0}, ", branchid ?? 0);
                query.AppendFormat("@DateFrom = '{0}', ", StartDate.ToString("yyyy-MM-dd"));
                query.AppendFormat("@DateUpto = '{0}', ", EndDate.ToString("yyyy-MM-dd"));
                query.AppendFormat("@Detailed = {0}, ", Detailed.HasValue ? (Detailed.Value ? 1 : 0) : 1);

                if (Item != 0)
                {
                    query.AppendFormat("@ItemID = {0}, ", Item);
                }

                if (Customer.HasValue && Customer.Value != 0)
                {
                    query.AppendFormat("@SalesManID = {0}, ", Customer.Value);
                }

                if (AccountId.HasValue && AccountId.Value != 0)
                {
                    query.AppendFormat("@AccountID = {0}, ", AccountId.Value);
                }

                if (query.ToString().EndsWith(", "))
                {
                    query.Length -= 2;
                }

                if (ViewBy == "Item")
                {
                    result = _context.InventoryProfitItemView.FromSqlRaw(query.ToString()).ToList();
                }
                else if (ViewBy == "Voucher")
                {
                    if (Detailed == false)
                    {
                        result = _context.InventoryProfitVoucherViews.FromSqlRaw(query.ToString()).ToList();
                    }
                    else
                    {
                        result = _context.InventoryProfitVoucherView.FromSqlRaw(query.ToString()).ToList();

                    }

                }
                else if (ViewBy == "Party")
                {
                    if (Detailed == false)
                    {
                        result = _context.InventoryProfitPartyViews.FromSqlRaw(query.ToString()).ToList();
                    }
                    result = _context.InventoryProfitPartyView.FromSqlRaw(query.ToString()).ToList();
                }
                else
                {
                    return CommonResponse.Error("Invalid ViewBy parameter.");
                }

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// report ItemHistory
        /// </summary>
        /// <param name="viewby"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="warehouse"></param>
        /// <param name="customersupplier"></param>
        /// <param name="item"></param>
        /// <param name="unit"></param>
        /// <param name="barcode"></param>
        /// <param name="orgin"></param>
        /// <param name="brand"></param>
        /// <param name="commodity"></param>
        /// <param name="branch"></param>
        /// <param name="vouchertype"></param>
        /// <param name="serialno"></param>
        /// <returns></returns>
        public CommonResponse GetItemHistory(string? viewby, DateTime startdate, DateTime enddate, int? warehouse, int? customersupplier, int? item, int? unit, string? barcode, int orgin, int? brand, int? commodity, int? branch, int? vouchertype, string? serialno)
        {
            try
            {
                object result = null;
                int? branchid = _authService.GetBranchId().Value;
                var query = new StringBuilder();
                query.Append("Exec ItemsHistorySP ");

                // Add a conditionally appended parameters to a list
                var parameters = new List<string>
        {
            $"@BranchID = {branchid ?? 0}",
            $"@DateFrom = '{startdate:yyyy-MM-dd}'",
            $"@DateUpto = '{enddate:yyyy-MM-dd}'"
        };

                if (viewby != null)
                {
                    parameters.Add($"@Nature = '{viewby}'");
                }
                if (item.HasValue && item.Value != 0)
                {
                    parameters.Add($"@ItemID = {item}");
                }
                if (customersupplier.HasValue && customersupplier.Value != 0)
                {
                    parameters.Add($"@AccountID = {customersupplier}");
                }
                if (!string.IsNullOrEmpty(barcode))
                {
                    parameters.Add($"@Barcode = '{barcode}'");
                }
                if (orgin != 0)
                {
                    parameters.Add($"@OriginID = {orgin}");
                }
                if (brand.HasValue && brand.Value != 0)
                {
                    parameters.Add($"@BrandID = {brand}");
                }
                if (commodity.HasValue && commodity.Value != 0)
                {
                    parameters.Add($"@CommodityID = {commodity}");
                }
                if (unit.HasValue && unit.Value != 0)
                {
                    parameters.Add($"@Unit = {unit}");
                }
                if (warehouse.HasValue && warehouse.Value != 0)
                {
                    parameters.Add($"@WarehouseID = {warehouse}");
                }
                if (vouchertype.HasValue && vouchertype.Value != 0)
                {
                    parameters.Add($"@VoucherID = {vouchertype}");
                }
                if (!string.IsNullOrEmpty(serialno))
                {
                    parameters.Add($"@UniqueID = '{serialno}'");
                }

                // Join parameters with commas
                query.Append(string.Join(", ", parameters));

                result = _context.ItemsHistoryReportView.FromSqlRaw(query.ToString()).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// Report=>ROL
        /// </summary>
        /// <param name="warehouse"></param>
        /// <param name="type"></param>
        /// <param name="commodity"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public CommonResponse GetROLReport(int? warehouse, int? type, int? commodity, int? item)
        {
            try
            {
                object result = null;
                int? branchid = _authService.GetBranchId().Value;
                string Criteria = "FillROLInfo";
                var query = new StringBuilder();
                query.Append("Exec ItemMasterSP ");
                query.AppendFormat("@Criteria = {0}, ", Criteria);
                query.AppendFormat("@BranchID = {0}, ", branchid ?? 0);

                if (item != 0 && item != null)
                {
                    query.AppendFormat(", @ID = {0}", item);
                }
                if (warehouse != 0 && warehouse != null)
                {
                    query.AppendFormat(", @LocID = {0}", warehouse);
                }
                if (type != 0 && type != null)
                {
                    query.AppendFormat(", @TypeofWoodID = {0}", type);
                }
                if (commodity != 0 && commodity != null)
                {
                    query.AppendFormat(", @CommodityID = {0}", commodity);
                }
                if (query.ToString().EndsWith(", "))
                {
                    query.Length -= 2;
                }
                result = _context.ROLView.FromSqlRaw(query.ToString()).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VoucherId"></param>
        /// <param name="VoucherNo"></param>
        /// <returns></returns>
        public CommonResponse GetQuotationStatusReport(int? VoucherId, string? VoucherNo)
        {
            try
            {
                object result = null;
                string cri = "ReferenceReport";
                result = _context.QuotationStatusReportView
                   .FromSql($"exec VoucherAdditionalsSP @Criteria='{cri}', @VoucherID={VoucherId}, @ReferenceNo='{VoucherNo}'")
                   .ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// QuotationComparison
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateUpto"></param>
        /// <param name="BranchID"></param>
        /// <param name="TransactionNo"></param>
        /// <param name="AccountID"></param>
        /// <param name="ItemID"></param>
        /// <param name="VoucherID"></param>
        /// <returns></returns>
        public CommonResponse GetQuotationComparisonView(DateTime DateFrom, DateTime DateUpto, int BranchID, string? TransactionNo, int? AccountID, int? ItemID, int? VoucherID)
        {
            try
            {
                object result = null;
                string Criteria = "QuotationDetails";
                var query = new StringBuilder();
                query.Append("Exec DeliveryReportSP ");
                query.AppendFormat("@Criteria = {0}, ", Criteria);
                query.AppendFormat("@DateFrom = '{0}', ", DateFrom.ToString("yyyy-MM-dd"));
                query.AppendFormat("@DateUpto = '{0}', ", DateUpto.ToString("yyyy-MM-dd"));
                query.AppendFormat("@BranchID = {0}, ", BranchID);
                if (TransactionNo != null)
                {
                    query.AppendFormat(", @TransactionNo = {0}", TransactionNo);
                }
                if (AccountID != 0 && AccountID != null)
                {
                    query.AppendFormat(", @AccountID = {0}", AccountID);
                }
                if (ItemID != 0 && ItemID != null)
                {
                    query.AppendFormat(", @ItemID = {0}", ItemID);
                }
                if (VoucherID != 0 && VoucherID != null)
                {
                    query.AppendFormat(", @VTypeID = {0}", VoucherID);
                }
                if (query.ToString().EndsWith(", "))
                {
                    query.Length -= 2;
                }
                result = _context.QuotationComparisonView.FromSqlRaw(query.ToString()).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse GetPartialDelivery(DateTime DateFrom, DateTime DateUpto, int branchid,
           bool Detailed, int? customersupplier,
           int? item, int? voucher, string? Criteria)
        {
            try
            {
                object result = null;

                var query = new StringBuilder();
                query.Append("Exec InventoryRegisterSP ");

                query.AppendFormat("@DateFrom = '{0}', ", DateFrom.ToString("yyyy-MM-dd"));
                query.AppendFormat("@DateUpto = '{0}', ", DateUpto.ToString("yyyy-MM-dd"));
                query.AppendFormat("@BranchID = {0}, ", branchid);
                query.AppendFormat("@Detailed = {0}, ", Detailed);
                if (Criteria != null)
                {
                    query.AppendFormat("@Criteria = {0}, ", Criteria);
                }

                if (customersupplier != 0 && customersupplier != null)
                {
                    query.AppendFormat(", @AccountID = {0}", customersupplier);
                }
                if (item != 0 && item != null)
                {
                    query.AppendFormat(", @ItemID = {0}", item);
                }
                if (voucher != 0 && voucher != null)
                {
                    query.AppendFormat(", @VTypeID = {0}", voucher);
                }

                if (query.ToString().EndsWith(", "))
                {
                    query.Length -= 2;
                }
                result = _context.PurchaseReportView.FromSqlRaw(query.ToString()).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// GetMonthlyInventorySummary
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public CommonResponse GetMonthlyInventorySummary(DateTime? startdate, DateTime? enddate,int? accountid,int? voucherid ,int? drcr,int? partycategoryid,int? categorytypeid,int? commodity,int? item)
        {
            //if (!_authService.IsPageValid(pageId))
            //{
            //    return PageNotValid(pageId);
            //}
            //if (!_authService.UserPermCheck(pageId, 1))
            //{
            //    return PermissionDenied("Fill the data ");
            //}
            try
            {
                int branchId = _authService.GetBranchId().Value;

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec ConsolidatedMonthwiseInventorySP @DateFrom='{startdate}',@DateUpto='{enddate}',@BranchID={branchId},@AccountID='{accountid}'," +
                    $"@VoucherID='{voucherid}',@DrCr='{drcr}',@PartyCategoryID='{partycategoryid}',@CategoryType='{categorytypeid}',@Commodity='{commodity}',@ItemID='{item}'";
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
                    return CommonResponse.NoContent();
                }
            }
            catch
            {
                _logger.LogError("Failed to fill Account Breakup/CostCentre Breakup ");
                return CommonResponse.Error("Failed to fill the data ");
            }
        }


        public CommonResponse PriceCategoryReport(int? ItemId)
        {
            try
            {
                string? criteria = "FillInvItemUnits";
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                string commandText = $"Exec ItemMasterSP @Criteria='{criteria}',@ItemID={ItemId}";
                cmd.CommandText = commandText;
                _context.Database.GetDbConnection().Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var tb = new DataTable();
                        tb.Load(reader);

                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
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
                    return CommonResponse.NoContent();
                }
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex.Message);
            }
            finally
            {

                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    _context.Database.GetDbConnection().Close();
                }
            }
        }

        //ItemPopup used in PriceCategoryReport
        public CommonResponse ItemPopUp()
        {
            try
            {
                var items = _context.ItemMaster.Where(im => im.Active == true && im.IsGroup == false)
                    .Select(im => new
                    {
                        ItemCode = im.ItemCode,
                        ItemName = im.ItemName,
                        Unit = im.Unit,
                        BarCode = im.BarCode,
                        ID = im.Id
                    }).ToList();
                return CommonResponse.Ok(items);        
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        //used in sales invoice
        //while pressing Alt+Ctrl+C
        public CommonResponse GetPriceCatAndRate(int itemId,string unit)
        {
            string criteria = "PriceCategory";
            var data = _context.PriceCatAndRateView.FromSqlRaw($"Exec ItemMasterSP @Criteria='{criteria}',@ItemID={itemId},@Unit='{unit}'").ToList();
            return CommonResponse.Ok(data);
        }

        public CommonResponse GetPreviousItemData(int itemId,Object accountId,object voucherId)
        {
            int branchId=_authService.GetBranchId().Value;
            string criteria = "GetLastItemRate";
            var prevTransData = _context.ItemTransaction.FromSqlRaw("Exec VoucherAdditionalsSP @Criteria={0},@BranchID={1},@ItemID={2},@AccountID={3},@VoucherID={4}",criteria,branchId,itemId,accountId,voucherId).ToList();
            return CommonResponse.Ok(prevTransData);
        }

        //shortcut key Alt+Ctrl+U
        //fills the remarks from itemmaster
        public CommonResponse FillRemarks(int itemId)
        {
            var remarks = _context.ItemMaster.Where(i => i.Id == itemId).Select(i => i.Remarks).FirstOrDefault();
            return CommonResponse.Ok(remarks);                
        }

        public CommonResponse FillQtySettings()
        {
            var qtySet = _context.MaSettings.Where(s => s.Key == "QuantityDefaultValue").Select(s => s.Value).FirstOrDefault();
            bool ratewithtax = false;           
            var rateWithTax = _context.MaSettings.Where(s => s.Key == "RateWithTax").Select(s => s.Value).SingleOrDefault();
            rateWithTax = rateWithTax.Trim().ToLower();
            if (rateWithTax == "true" || rateWithTax == "yes" || rateWithTax == "1")
                ratewithtax = true;
            return CommonResponse.Ok(new { DefaultQuantity = qtySet, RateWithTax = ratewithtax });
        }      

    }
}




