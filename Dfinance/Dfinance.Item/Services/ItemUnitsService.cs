using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Migrations;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Item.Services.Inventory
{
    public class ItemUnitsService : IItemUnitsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;        
        private readonly ISettingsService _settings;
        private readonly ILogger<ItemUnitsService> _logger;
        public ItemUnitsService(DFCoreContext context, IAuthService authService, ISettingsService settings, ILogger<ItemUnitsService> logger)
        {
            _context = context;
            _authService = authService;           
            _settings = settings;
            _logger= logger;
        }

        //fill itemunits by itemid
        public CommonResponse FillItemUnits(int ItemId, int BranchId)
        {

            var unitBranch = _context.ItemUnits.Where(u => u.BranchId == BranchId && u.ItemId == ItemId).Count();
            if (unitBranch == 0)
                return CommonResponse.Ok("Units are empty");
            string criteria = "FillInvItemUnitsWeb";
            var data = _context.FillItemUnitsView.FromSqlRaw("Exec ItemMasterSP @Criteria={0}, @ItemID={1}, @BranchID={2}",
                criteria, ItemId, BranchId).AsEnumerable().ToArray(); ;
            return CommonResponse.Ok(data);
        }

        //get itemunits for pop up in inventory item grid using getcommandtext
        public CommonResponse GetItemUnits(int itemId)
        {
            string criteria = "ItemUnitofItemID";
            object PrimaryVoucherID = null, ModeID = null, TransactionID = null, branchId = null, partyId = null, locId = null, voucherId = null, PageID = null, userId = null;
            bool IsSizeItem = false, IsMargin = false, ISTransitLoc = false, IsFinishedGood = false, IsRawMaterial = false;

            DateTime? VoucherDate = null;
            var result = _context.CommandTextView
            .FromSqlRaw($"select dbo.GetCommandText('{criteria}','{PrimaryVoucherID}','{branchId}','{partyId}','{locId}','{IsSizeItem}','{IsMargin}','{voucherId}','{itemId}','{ISTransitLoc}','{IsFinishedGood}','{IsRawMaterial}','{ModeID}','{PageID}','{VoucherDate}','{TransactionID}','{userId}')")
            .ToList();

            var res = result.FirstOrDefault();
            var data = _context.TransItemUnits.FromSqlRaw(res.commandText).ToList();
            return CommonResponse.Ok(data);
        }

        //saving itemunits
        public CommonResponse SaveItemUnits(List<ItemUnitsDto> unitList, List<int> branch, int ItemId)
        {
            SqlParameter newIdUnits = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            foreach (var b in branch)
            {
                foreach (var units in unitList)
                {
                    string criteria = "InsertInvItemUnits";
                    units.PurchaseRate = Math.Round(units.PurchaseRate.Value, 4);
                    units.SellingPrice = Math.Round(units.SellingPrice.Value, 4);
                    units.WholeSalePrice = Math.Round(units.WholeSalePrice.Value, 4);
                    units.RetailPrice = Math.Round(units.RetailPrice.Value, 4);
                    units.WholeSalePrice2 = Math.Round(units.WholeSalePrice2.Value, 4);
                    units.RetailPrice2 = Math.Round(units.RetailPrice2.Value, 4);
                    units.MRP = Math.Round(units.MRP.Value, 4);

                    var data = _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@ItemID={1},@Unit={2},@BasicUnit={3},@Factor={4},@Active={5}," +
                             "@SellingPrice={6},@PurchaseRate={7},@BarCode={8},@WholeSalePrice={9},@RetailPrice={10},@DiscountPrice={11},@OtherPrice={12}," +
                             "@LowestRate={13},@MRP={14},@BranchID={15},@NewID={16} OUTPUT",
                             criteria,
                             ItemId,
                             units.Unit.Unit,
                             units.BasicUnit,
                             units.Factor,
                             units.Active,
                             units.SellingPrice,
                             units.PurchaseRate,
                             units.BarCode,
                             units.WholeSalePrice,
                             units.RetailPrice,
                             units.WholeSalePrice2,
                             units.RetailPrice2,
                             units.LowestRate,
                             units.MRP,
                             b,
                             newIdUnits);
                    var newUnitId = (int)newIdUnits.Value;
                }
            }
            _logger.LogInformation("ItemUnits Created Successfully");
            return CommonResponse.Created("ItemUnits Created Successfully");
        }

        //updating itemunits
        public CommonResponse UpdateItemUnits(List<ItemUnitsDto> unitList, int ItemId, List<int> branch)
        {
            foreach (var b in branch)
            {
                foreach (var units in unitList)
                {
                    var check = _context.ItemUnits.Any(u => u.Id == units.UnitID);
                    if (check == false)
                        return CommonResponse.NotFound("Unit Not Found");
                    string criteria = "UpdateInvItemUnits";
                    units.PurchaseRate = Math.Round(units.PurchaseRate.Value, 4);
                    units.SellingPrice = Math.Round(units.SellingPrice.Value, 4);
                    units.WholeSalePrice = Math.Round(units.WholeSalePrice.Value, 4);
                    units.RetailPrice = Math.Round(units.RetailPrice.Value, 4);
                    units.WholeSalePrice2 = Math.Round(units.WholeSalePrice2.Value, 4);
                    units.RetailPrice2 = Math.Round(units.RetailPrice2.Value, 4);
                    units.MRP = Math.Round(units.MRP.Value, 4);

                    var data = _context.Database.ExecuteSqlRaw("Exec ItemMasterSP @Criteria={0},@ItemID={1},@Unit={2},@BasicUnit={3},@Factor={4},@Active={5}," +
                         "@SellingPrice={6},@PurchaseRate={7},@BarCode={8},@WholeSalePrice={9},@RetailPrice={10},@DiscountPrice={11},@OtherPrice={12}," +
                         "@LowestRate={13},@MRP={14},@BranchID={15},@ID={16}",
                         criteria,
                         ItemId,
                         units.Unit.Unit,
                         units.BasicUnit,
                         units.Factor,
                         units.Active,
                         units.SellingPrice,
                         units.PurchaseRate,
                         units.BarCode,
                         units.WholeSalePrice,
                         units.RetailPrice,
                         units.WholeSalePrice2,
                         units.RetailPrice2,
                         units.LowestRate,
                         units.MRP,
                         branch,
                         units.UnitID);
                    _logger.LogInformation(units.UnitID+" ItemUnits Updated Successfully");
                }
                
            }
           
            return CommonResponse.Created(" ItemUnits Updated");
        }

        //deleting itemunits
        public CommonResponse DeleteItemUnits(int UnitId)
        {
            var check = _context.ItemUnits.Any(i => i.Id == UnitId);
            if (check == false)
                return CommonResponse.NotFound("Unit Not Found");
            _context.Database.ExecuteSqlRaw($"Exec ItemMasterSP @Criteria='DeleteInvItemUnits',@ID='{UnitId}'");
            _logger.LogInformation(UnitId+" ItemUnits Deleted Successfully");
            return CommonResponse.Ok();
        }
    }
}
