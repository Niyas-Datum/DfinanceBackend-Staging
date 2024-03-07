using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Item.Services.Inventory
{
    public  class ItemUnitsService:IItemUnitsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public ItemUnitsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        //fill itemunits by itemid
        public CommonResponse FillItemUnits(int ItemId,int BranchId)
        {
            try
            {
                if (BranchId == null)
                    BranchId = _authService.GetBranchId().Value;
                string criteria = "FillInvItemUnitsWeb";
                var data = _context.FillItemUnitsView.FromSqlRaw($"Exec ItemMasterSP @Criteria='{criteria}',@ItemID='{ItemId}',@BranchID='{BranchId}'").ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        //saving itemunits
        public CommonResponse SaveItemUnits(ItemUnitsDto units, int branch, int ItemId)
        {
            try
            {
                SqlParameter newIdUnits = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
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
                         branch,
                         newIdUnits);
                var newUnitId = (int)newIdUnits.Value;

                return CommonResponse.Created(data);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        //updating itemunits
        public CommonResponse UpdateItemUnits(ItemUnitsDto units, int ItemId,int branch)
        {
            try
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
                return CommonResponse.Created(data);
            }
            catch (Exception ex)
            {               
                return CommonResponse.Error();
            }
        }

        //deleting itemunits
        public CommonResponse DeleteItemUnits(int UnitId)
        {
            try
            {
                var check = _context.ItemUnits.Any(i => i.Id == UnitId);
                if (check == false)
                    return CommonResponse.NotFound("Unit Not Found");
                _context.Database.ExecuteSqlRaw($"Exec ItemMasterSP @Criteria='DeleteInvItemUnits',@ID='{UnitId}'");
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
    }
}
