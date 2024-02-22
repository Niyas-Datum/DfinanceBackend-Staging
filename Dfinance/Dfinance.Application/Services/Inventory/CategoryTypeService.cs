using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.Inventory
{
    public class CategoryTypeService:ICategoryTypeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
      
        public CategoryTypeService(DFCoreContext context,IAuthService authService) 
        {
            _context = context;
            _authService = authService;
           
        }
        //called by CategoryTypeController/FillCategoryType
        /******************* Fill All Category Type ******************/
        public CommonResponse FillCategoryType()
        {
            try
            {
                string criteria = "FillTypeOfCommodityMaster";
                var result = _context.ReadViewDesc.FromSqlRaw($"EXEC spTypeofWood @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        //called by CategoryTypeController/FillCategoryTypeById
        /********************** Fill CategoryType By Id ***************/
        public CommonResponse FillCategoryTypeById(int Id)
        {
            try
            {
                var id = _context.CategoryType.Where(i => i.Id == Id).
                  Select(i => i.Id).
                  SingleOrDefault();
                if (id == null)
                {
                    return CommonResponse.NotFound("CategoryType Not Found");
                }
                string criteria = "FillTypeOfCommodityByID";
                var result = _context.SpFillCategoryTypeById.FromSqlRaw($"EXEC spTypeofWood @Criteria='{criteria}',@ID='{Id}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        //called by CategoryTypeController/GetNextCode
        /******************** Get Next Code ************************/
        public CommonResponse GetNextCode()
        {
            try
             {            
                string criteria = "GetNextCode";
                var code = _context.NextCodeView.FromSqlRaw($"Exec spTypeofWood @Criteria ='{criteria}'")
                    .AsEnumerable()
                    .Select(x => x.Code)
                    .FirstOrDefault();
                return CommonResponse.Ok(code);
             }           
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //called by CategoryTypeController/SaveCategoryType
        /********************  Save CategoryType  ******************/
        public CommonResponse SaveCategoryType(CategoryTypeDto categoryTypeDto)
        {
            try
            {               
                int CreatedBy = _authService.GetId().Value;
                int BranchId = _authService.GetBranchId().Value;                
                DateTime CreatedOn = DateTime.Now;
                string criteria = "InsertTypeofWood";               
               
                SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("Exec spTypeofWood  @Criteria={0},@Code={1},@Description={2},@CreatedBy={3},@CreatedOn={4}," +
                            "@AvgStockQuantity={5},@BranchID={6},@NewID={7} OUTPUT",
                            criteria,
                            categoryTypeDto.Code,
                            categoryTypeDto.Description,
                            CreatedBy,
                            CreatedOn,
                            categoryTypeDto.AvgStockQuantity,
                            BranchId,
                            newIdParameter
                            );
                var newId = newIdParameter.Value;
                return CommonResponse.Created("CategoryType " + categoryTypeDto.Description + " Created Successfully");
            }
            catch(Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        //called by CategoryTypeController/UpdateCategoryType
        /********************* Update Category Type ***********************/
        public CommonResponse UpdateCategoryType(CategoryTypeDto categoryTypeDto, int Id)
        {
            try
            {
                var id = _context.CategoryType.Where(i => i.Id == Id).
                  Select(i => i.Id).
                  SingleOrDefault();
                if (id == null)
                {
                    return CommonResponse.NotFound("CategoryType Not Found");
                }
                int CreatedBy = _authService.GetId().Value;
                int BranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                string criteria = "UpdateTypeofWood";               
                var result = _context.Database.ExecuteSqlRaw($"EXEC spTypeofWood @ID='{Id}',@Criteria='{criteria}',@Description='{categoryTypeDto.Description}',@Code='{categoryTypeDto.Code}',@AvgStockQuantity='{categoryTypeDto.AvgStockQuantity}',@CreatedBy='{CreatedBy}',@CreatedOn='{CreatedOn}',@BranchID='{BranchId}'");
                return CommonResponse.Ok("Category Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //called by CategoryTypeController/DeleteCategoryType
        /********************* Delete Category Type ***********************/
        public CommonResponse DeleteCategoryType(int Id)
        {
            try
            {
                string msg = null;
                var description = _context.CategoryType.Where(i => i.Id == Id).
                   Select(i => i.Description).
                   SingleOrDefault();
                if (description == null)
                {
                    msg = "CategoryType Not Found";
                }
                else 
                {
                    int Mode = 3;
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spTypeofWood @Mode='{Mode}',@ID='{Id}'");
                    msg = "Type of Category "+description + " Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //called by CategoryTypeController/CategoryTypePopUp
        /******************** CategoryType PopUp *************************/
        public CommonResponse CategoryTypePopUp()
        {
            try
            {
                var data = _context.CategoryType
                     .Select(c => new ReadViewDesc
                     {
                         ID = c.Id,
                         Code = c.Code,
                         Description = c.Description
                     }).ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}
