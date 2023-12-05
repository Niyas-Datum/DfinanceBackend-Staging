using Dfinance.Application.Dto.Inventory;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Data;

namespace Dfinance.Application.Services.Inventory
{

    public class CategoryService : ICategoryService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public CategoryService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /******************** Fill All Category  ***********************/
        public CommonResponse FillCategory()
        {
            try
            {
                string criteria = "FillCommodityMaster";
                var result = _context.SpFillCategoryG.FromSqlRaw($"EXEC spCommodity @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        /******************** Fill Category By Id  ***********************/
        public CommonResponse FillCategoryById(int Id)
        {
            try
            {
                var id = _context.Commodity.Where(i => i.Id == Id).
                  Select(i => i.Id).
                  SingleOrDefault();
                if (id == null)
                {
                    return CommonResponse.NotFound("Category Not Found");
                }
                string criteria = "FillCommodityByID";
                var result = _context.SpFillCategoryByIdG.FromSqlRaw($"EXEC spCommodity @Criteria='{criteria}',@ID='{Id}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        
        /******************** Save Category  ***********************/
        public CommonResponse SaveCategory(CategoryDto categoryDto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                string criteria = "InsertCommodity";
                SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC spCommodity @Criteria={0},@Description={1},@TypeofWoodID={2},@Category={3}," +
                    "@CreatedBy={4},@CreatedOn={5},@ActiveFlag={6},@Code={7},@MType={8},@MinQty={9},@MaxQty={10},@BranchID={11},@FloorRate={12}," +
                    "@MinusStock={13},@StartDate={14},@EndDate={15},@Discount={16},@NewID={17} OUTPUT",
                    criteria,
                    categoryDto.CategoryName,
                    categoryDto.CategoryType.Id,
                    categoryDto.Category,
                    CreatedBy,
                    CreatedOn,
                    categoryDto.ActiveFlag,
                    categoryDto.CategoryCode,
                    categoryDto.MeasurementType,
                    categoryDto.MinimumQuantity,
                    categoryDto.MaximumQuantity,
                    CreatedBranchId,
                    categoryDto.FloorRate,
                    categoryDto.MinusStock,
                    categoryDto.StartDate,
                    categoryDto.EndDate,
                    categoryDto.DiscountPerc,
                    newIdParameter);
                var newId = newIdParameter.Value;
                return CommonResponse.Created("Category " + categoryDto.CategoryName + " Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /******************** Update Category  ***********************/
        public CommonResponse UpdateCategory(CategoryDto categoryDto, int Id)
        {
            try
            {
                var id = _context.Commodity.Where(i => i.Id == Id).
                  Select(i => i.Id).
                  SingleOrDefault();
                if (id == null)
                {
                   return CommonResponse.NotFound("Category Not Found");
                }
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;              
                string criteria = "UpdateCommodity";
                var result = _context.Database.ExecuteSqlRaw($"EXEC spCommodity @ID='{Id}',@Criteria='{criteria}',@Description='{categoryDto.CategoryName}',@TypeofWoodID='{categoryDto.CategoryType.Id}',@Category='{categoryDto.Category}',@CreatedBy='{CreatedBy}',@CreatedOn='{CreatedOn}',@Code='{categoryDto.CategoryCode}',@MType='{categoryDto.MeasurementType}',@MinQty='{categoryDto.MinimumQuantity}',@MaxQty='{categoryDto.MaximumQuantity}',@FloorRate='{categoryDto.FloorRate}',@MinusStock='{categoryDto.MinusStock}',@BranchID='{CreatedBranchId}',@ActiveFlag='{categoryDto.ActiveFlag}',@StartDate='{categoryDto.StartDate}',@EndDate='{categoryDto.EndDate}',@Discount='{categoryDto.DiscountPerc}'");
                return CommonResponse.Ok("Category Updated Successfully");               
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }


        /******************** Delete Category  ***********************/
        public CommonResponse DeleteCategory(int Id)
        {
            try
            {                
                string msg = null;
                var name = _context.Commodity.Where(i => i.Id == Id).
                   Select(i => i.Description).
                   SingleOrDefault();
                if (name == null)
                {
                    msg = "This Category is Not Found";
                }
                else
                {
                    int Mode = 3;
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spCommodity @Mode='{Mode}',@ID='{Id}'");
                    msg = name + " Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
      
    }
}
