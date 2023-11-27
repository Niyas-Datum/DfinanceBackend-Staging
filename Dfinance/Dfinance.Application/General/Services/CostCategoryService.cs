using Dfinance.Application.Dto.General;
using Dfinance.Application.General.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Dfinance.Application.General.Services
{
    public class CostCategoryService:ICostCategoryService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public CostCategoryService(DFCoreContext context,IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public CommonResponse FillCostCategory()
        {
            try
            {
                string criteria = "FillCostCategoryMaster";
                var result = _context.SpFillCostCategoryG.FromSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}'").ToList();               
                return CommonResponse.Ok(result);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillCostCategoryById(int Id)
        {
            try
            {
                string criteria = "FillCostCategoryWithID";
                var result=_context.SpFillCostCategoryByIdG.FromSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}',@CostCategoryID='{Id}'").ToList();                
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveCostCategory(CostCategoryDto costCategoryDto)
        {
            try
            {
                int CreatedBy =  _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn= DateTime.Now;               
                SqlParameter newIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                string criteria = "InsertCostCategory";
                
                _context.Database.ExecuteSqlRaw("EXEC CostCentreSP @Criteria={0}, @Name={1}, @Description={2}, @AllocateRevenue={3},@AllocateNonRevenue={4}, @Active={5}, @CreatedBranchID={6}, @CreatedBy={7},@CreatedOn={8}, @NewID={9} OUTPUT",
                    criteria, costCategoryDto.Name, costCategoryDto.Description, costCategoryDto.AllocateRevenue, costCategoryDto.AllocateNonRevenue, costCategoryDto.Active, 
                    CreatedBranchId,CreatedBy,CreatedOn,newIdParam);

                var NewId = newIdParam.Value;
                return CommonResponse.Ok("CostCategory "+costCategoryDto.Name+" Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse UpdateCostCategory(CostCategoryDto costCategoryDto,int Id)
        {
            try
            {                
                if (Id == 0)
                    return CommonResponse.NotFound("Cost Category Not Found");               
                string criteria = "UpdateCostCategory";
                var result = _context.Database.ExecuteSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}',@ID='{Id}',@Name='{costCategoryDto.Name}',@Description='{costCategoryDto.Description}',@AllocateRevenue='{costCategoryDto.AllocateRevenue}',@AllocateNonRevenue='{costCategoryDto.AllocateNonRevenue}',@Active='{costCategoryDto.Active}'");
                return CommonResponse.Ok("CostCategory Updated Successfully");
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse DeleteCostCategory(int Id)
        {
            try
            {
                if (Id == 0)
                    return CommonResponse.NotFound();
                string msg = null;
                var name=_context.CostCategory.Where(i=>i.Id == Id).
                    Select(i=>i.Name).
                    SingleOrDefault();
                if (name == null)
                {
                    msg = "This CostCategory is Not Found";
                }
                else
                {
                    string criteria = "DeleteCostCategory";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}',@ID='{Id}'");
                    msg = name + " Is Deleted Successfully";
                }
                
                return CommonResponse.Ok(msg);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse FillCostCategoryDropDown()
        {
            try
            {
                string criteria = "FillCostCategory";
                var data = _context.SpDropDownCommon.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
