using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using Microsoft.AspNetCore.Http;


namespace Dfinance.Item.Services.Inventory
{
    public class ItemMasterService : IItemMasterService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        IItemUnitsService _itemunitService;
        private static int reqCount = 0;
        private readonly IHostEnvironment _environment;

        public ItemMasterService(DFCoreContext context, IAuthService authService, IItemUnitsService itemunitService, IHostEnvironment environment)
        {
            _context = context;
            _authService = authService;
            _itemunitService = itemunitService;
            _environment = environment;
        }

        //called by ItemMasterController/FillItemMaster
        /******************* Fill Item Master (Left side Table) ********************/
        public CommonResponse FillItemMaster(int Id=0)
        {
            try
            {
                int BranchId = _authService.GetBranchId().Value;
                if (Id == 0)
                {
                    var result = _context.SpFillItemMaster.FromSqlRaw($"Exec ItemMasterSP @Criteria='FillMasterWeb',@BranchID='{BranchId}'").ToList();
                    return CommonResponse.Ok(result);
                }
                else
                {
                    var itemId = _context.ItemMaster.Any(i=>i.Id==Id);
                    if (itemId)
                    {
                        var result = FillItemByID(Id, BranchId);
                        return CommonResponse.Ok(result);
                    }                   
                    return CommonResponse.NoContent("Item Not Exists");
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        /********************* Fill Items by Id **************************/
        private CommonResponse FillItemByID(int Id, int BranchId)
        {
            try
            {
                string criteria = "FillItemByID";
                var itemdata = _context.SpFillItemMasterById.FromSqlRaw($"Exec ItemMasterSP @Criteria='{criteria}',@ID='{Id}'").ToList();
                var unitdata = _itemunitService.FillItemUnits(Id, BranchId);
                var itemhistory = FillItemHistory(Id);
                var itemstock = GetCurrentStock(Id, BranchId);

                return CommonResponse.Ok(new { item = itemdata, unit = unitdata, history = itemhistory, stock = itemstock });
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //called by ItemMasterController/GetNextItemCode
        //Generate next ItemCode
        public CommonResponse GetNextItemCode()
        {
            try
            {
                return CommonResponse.Ok(GenerateCode());
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
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
            try
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
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //Branchwise TaxType Dropdown
        public CommonResponse TaxDropDown()
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                string criteria = "FillBranchWiseTax";
                var data = _context.DropDownViewName.FromSqlRaw($"Exec DropDownListSP @Criteria='{criteria}',@BranchID='{branchId}'").ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //Generate Barcode
        public CommonResponse GenerateBarCode()
        {
            try
            {
                var Barcode = _context.BarcodeView.FromSqlRaw($"Exec ItemMasterSP @Criteria='BarCodeFromInvItemUnits'").ToList();
                return CommonResponse.Ok(Barcode);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("Barcode not generated");
            }
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

        private string UploadImage(string itemCode, IFormFile imageFile)
        {
            //// Read the file contents into a byte array
            //byte[] fileBytes = File.ReadAllBytes(imageFile1);

            //// Create a MemoryStream from the byte array
            //using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            //{
            //    // Create a FormFile instance using the MemoryStream, specifying the file name and the form field name
            //    var formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(imageFile1))
            //    {
            //        Headers = new HeaderDictionary(),
            //        ContentType = "image/jpeg" // Set the content type appropriately
            //    };
            //    FormFile imageFile = formFile;
            //    var path = "";
            //    var maxSizeInBytes = 5 * 1024 * 1024;
            //    if (itemCode != null && imageFile != null)
            //    {
            //        if (imageFile.Length > maxSizeInBytes)
            //        {
            //            throw new Exception("File size exceeds the maximum allowed size.");
            //        }
            //        path = Path.Combine(_environment.ContentRootPath, "BindData", "Images", "Items", itemCode + ".jpg");
            //        using (FileStream stream = new FileStream(path, FileMode.Create))
            //        {
            //            imageFile.CopyTo(stream);
            //            stream.Close();
            //        }
            //    }
            //    return path;
            //}
            var path = "";
            var maxSizeInBytes = 5 * 1024 * 1024;
            if (itemCode != null && imageFile != null)
            {
                if (imageFile.Length > maxSizeInBytes)
                {
                    throw new Exception("File size exceeds the maximum allowed size.");
                }
                path = Path.Combine(_environment.ContentRootPath, "BindData", "Images", "Items", itemCode + ".jpg");
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                    stream.Close();
                }
            }
            return path;
        }
            //save itemmaster,itemunits
            //called by ItemMasterController/SaveItemMaster
            public CommonResponse SaveItemMaster(ItemMasterDto itemDto)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        int CreatedBy = _authService.GetId().Value;
                        int BranchId =  _authService.GetBranchId().Value;

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

                        string msg = null;


                        //check whether the item already exists
                        var obj = _context.ItemMaster.Any(i => i.ItemCode == itemDto.ItemCode);
                        if (obj)
                        {
                            msg = "Item Code Already Exists";
                            return CommonResponse.Error(msg);
                        }

                        //check itemcode valid or not
                        bool checkitemcode = CheckItemCode(itemDto.ItemCode);
                        if (checkitemcode == false)
                            return CommonResponse.Error("Warning:Do you want to change ItemCode??");

                        //upload image

                        var path = "";
                        if (itemDto.ImageFile != null)
                           // path = UploadImage(itemDto.ItemCode,itemDto.ImageFile);
                            path = "";


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
                            Math.Round(itemDto.SellingPrice.Value, 4),
                            itemDto.OEMNo,
                            itemDto.PartNo,
                            null,//CategoryId,   
                            itemDto.Manufacturer,
                            itemDto.BarCode,
                            itemDto.ModelNo,
                            itemDto.Unit.Unit,
                            Math.Round(itemDto.ROL.Value, 4),
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
                            Math.Round(itemDto.ROQ.Value, 4),
                            itemDto.Category.ID,
                            itemDto.ShipMark,
                            itemDto.PaintMark,
                            qualityId ? itemDto.Quality.Id : null,
                            Math.Round(itemDto.Weight.Value, 4),
                            parentId ? itemDto.Parent.ID : null,
                            Math.Round(itemDto.PurchaseRate.Value, 4),
                            Math.Round(itemDto.Margin.Value, 4),
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
                            itemDto.MarginValue,
                            itemDto.ArabicName,
                            itemDto.HSN,
                            Math.Round(itemDto.ItemDisc.Value, 4),
                            Math.Round(itemDto.MRP.Value, 4),
                            newIdItem
                            );
                        var NewItemId = (int)newIdItem.Value;
                        var multirate = SaveItemMultiRate(NewItemId);
                            

                        var branch = _context.MaBranches
                           .Where(b => b.ActiveFlag == 1 && b.BranchCompanyId == 1)   //add company id
                           .Select(b => b.Id)
                           .ToList();

                        foreach (var b in branch)
                        {
                            var branchitemsdata = SaveBranchItems(b, NewItemId, itemDto); //saving branchitems
                        }
                        if (itemDto.ItemUnit != null && itemDto.ItemUnit.Count > 0)
                        {
                            foreach (var units in itemDto.ItemUnit)
                            {
                                foreach (var b in branch)
                                {
                                    var unisdata = _itemunitService.SaveItemUnits(units, b, NewItemId); //saving itemunits
                                }
                            }
                        }
                        transaction.Commit();
                        return CommonResponse.Created( new { msg = "Item " + itemDto.ItemName + " Created Successfully", data =0 } );

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return CommonResponse.Error("Item Not saved");
                    }
                }
            }

            //saving BranchItems//called by SaveItemMaster() 
            private CommonResponse SaveBranchItems(int branch, int ItemId, ItemMasterDto itemDto)
            {
                try
                {
                    bool active = false;
                    int BranchId = _authService.GetBranchId().Value;
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    if (branch == BranchId)
                        active = true;

                    string criteria = "InsertInvBranchItems";
                    var data = _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@BranchID={1},@ItemID={2},@Active={3},@ROL={4},@ROQ={5},@TaxTypeID={6},@Location={7},@NewID={8} OUTPUT",
                         criteria,
                         branch,
                         ItemId,
                         active,
                         itemDto.ROL,
                         itemDto.ROQ,
                         itemDto.TaxType.Id,
                         itemDto.Location,
                         newId);
                    return CommonResponse.Created(data);
                }

                catch (Exception ex)
                {
                    return CommonResponse.Error(ex);
                }
            }


            //update itemmaster,itemunits
            //called by ItemMasterController/UpdateItemMaster
            public CommonResponse UpdateItemMaster(ItemMasterDto itemDto, int ItemId)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
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

                        var item = _context.ItemMaster.Any(item => item.Id == ItemId);
                        if (item == false)
                            return CommonResponse.NotFound("Item Not Exists");

                        var obj = _context.ItemMaster.
                            FirstOrDefault(i => i.ItemCode == itemDto.ItemCode && i.Id != ItemId);
                        if (obj != null)
                        {
                            return CommonResponse.Error("Item Code Already Exists");
                        }

                        int CreatedBy = _authService.GetId().Value;
                        int BranchId = _authService.GetBranchId().Value;

                        //check itemcode valid or not
                        bool chechitemcode = CheckItemCode(itemDto.ItemCode);
                        if (chechitemcode == false)
                            return CommonResponse.Error("Invalid ItemCode");

                        //upload image
                        var path = "";
                        if (itemDto.ImageFile != null)
                            //path = UploadImage(itemDto.ItemCode, itemDto.ImageFile);
                            path = "";

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
                           Math.Round(itemDto.SellingPrice.Value, 4),
                           itemDto.OEMNo,
                           itemDto.PartNo,
                           null,//CategoryId,   
                           itemDto.Manufacturer,
                           itemDto.BarCode,
                           itemDto.ModelNo,
                           itemDto.Unit.Unit,
                           Math.Round(itemDto.ROL.Value, 4),
                           itemDto.Remarks,
                           itemDto.IsGroup,
                           itemDto.StockItem,
                           itemDto.Active,
                           null,// itemDto.InvAccount.Id,  
                           itemDto.CostAccount.Id,
                           itemDto.PurchaseAccount.Id,
                           itemDto.SalesAccount.Id,
                           null, null, null, null, //stock,invoicedstock,avgcost,lastcost                 
                           DateTime.Now,
                           DateTime.Now,
                           CreatedBy,
                           CreatedBy,
                           BranchId,
                           itemDto.Location,
                           null, null,//cashprice,creditprice
                           Math.Round(itemDto.ROQ.Value, 4),
                           itemDto.Category.ID,
                           itemDto.ShipMark,
                           itemDto.PaintMark,
                           qualityId ? itemDto.Quality.Id : null,
                           Math.Round(itemDto.Weight.Value, 4),
                           parentId ? itemDto.Parent.ID : null,
                           Math.Round(itemDto.PurchaseRate.Value, 4),
                           Math.Round(itemDto.Margin.Value, 4),
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
                           itemDto.MarginValue,
                           itemDto.ArabicName,
                           itemDto.HSN,
                           Math.Round(itemDto.ItemDisc.Value, 4),
                           Math.Round(itemDto.MRP.Value, 4),
                           ItemId

                          );

                        var branch = new List<int>();
                        var ItemBranchSettings = _context.MaSettings
                            .Where(b => b.Key == "BranchwiseItem")
                            .Select(b => b.Value)
                            .FirstOrDefault();
                        object result;



                        if (ItemBranchSettings == "True" || ItemBranchSettings == "1")
                        {
                            bool HigherApprove = _authService.UserPermCheck(55, 8);
                            if (HigherApprove == true)
                            {
                                if (itemDto.Branch != null)
                                {
                                    branch.Add(itemDto.Branch.Id.Value);
                                }
                            }
                            else
                            {
                                branch.Add(BranchId);
                            }
                        }
                        else
                        {
                            branch = _context.MaBranches
                             .Where(b => b.ActiveFlag == 1 && b.BranchCompanyId == BranchId)
                             .Select(b => b.Id)
                             .ToList();
                        }

                        if (itemDto.ItemUnit != null && itemDto.ItemUnit.Count > 0)
                        {
                            foreach (var units in itemDto.ItemUnit)
                            {
                                foreach (var b in branch)
                                {
                                    if (units.Status == 1 || units.UnitID == 0)//adding new unit
                                    {
                                        result = _itemunitService.SaveItemUnits(units, b, ItemId);
                                    }
                                    else if (units.Status == 2)//edit unit
                                    {
                                        result = _itemunitService.UpdateItemUnits(units, ItemId, b);
                                    }
                                    else if (units.Status == 3)//deleting unit
                                    {
                                        result = _itemunitService.DeleteItemUnits(units.UnitID);
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                        return CommonResponse.Ok("Item Updated successfully");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return CommonResponse.Error(ex.Message);
                    }
                }
            }

            //called by SaveItemMaster()
            private CommonResponse SaveItemMultiRate(int ItemId)
            {
                try
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
                    return CommonResponse.Ok();
                }
                catch (Exception ex)
                {
                    return CommonResponse.Error(ex.Message);
                }
            }

            //delete itemmaster and itemunits
            public CommonResponse DeleteItem(int ItemId)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var check = _context.ItemMaster.Any(i => i.Id == ItemId);
                        if (check == false)
                            return CommonResponse.NotFound("Item Not Exists");
                        var unitId = _context.ItemUnits.Where(i => i.ItemId == ItemId).Select(i => i.Id).ToList();
                        var branchitems = _context.BranchItems.Where(i => i.ItemId == ItemId).Select(i => i.Id).ToList();

                        //check whether the imagepath exists
                        string imagepath = _context.ItemMaster.Where(i => i.Id == ItemId).Select(i => i.ImagePath).FirstOrDefault();
                        if (!string.IsNullOrEmpty(imagepath) && System.IO.File.Exists(imagepath))
                            System.IO.File.Delete(imagepath);//delete imagepath from the folder

                        _context.Database.ExecuteSqlRaw($"Exec ItemMasterSP @Criteria='Delete',@ID='{ItemId}'");

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
                    return CommonResponse.Ok(new { msg = "Item Details Deleted successfully", data = 0 });

                }
                catch (Exception ex)
                    {
                        transaction.Rollback();
                        return CommonResponse.Error(ex.Message);
                    }
                }
            }

            //fill ItemHistory//called by FillItemHistory in ItemMasterController
            public CommonResponse FillItemHistory(int ItemId)
            {
                try
                {
                    int BranchId = _authService.GetBranchId().Value;
                    string criteria = "FillItemHistory";
                    var result = _context.ItemHistoryView.FromSqlRaw($"Exec ItemMasterSP @Criteria='{criteria}',@BranchID='{BranchId}',@ItemID='{ItemId}'").ToList();
                    return CommonResponse.Ok(result);
                }
                catch (Exception ex)
                {
                    return CommonResponse.Error(ex.Message);
                }
            }

            public CommonResponse GetCurrentStock(int ItemId, int BranchId)
            {
                try
                {
                    //var BranchId= _authService.GetBranchId().Value;
                    var result = _context.CurrentStockView.FromSqlRaw($"select dbo.StockQtyOnDate('{ItemId}','{BranchId}',null,null)").ToList();
                    return CommonResponse.Ok(result);
                }
                catch (Exception ex)
                {
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
    }

