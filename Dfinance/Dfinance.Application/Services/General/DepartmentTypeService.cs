using Dfinance.Application.Dto;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class DepartmentTypeService: IDepartmentTypeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public DepartmentTypeService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        /*******************Department***********************************/
        //***********************DropDownDepartment***********************
        public CommonResponse DepartmentDropdown()
        {
            var data = _context.SpReDepartmentTypeFillAllDepartment.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillDepartmentTypes'").ToList();

            return CommonResponse.Ok(data);
        }
        //**********************Fill********************************************************
       public  CommonResponse FillDepartment()
        {
            try
            {
                string criteria = "FillDepartmentMaster";
                var data = _context.spMaDepartmentsFillAllDepartment
                   .FromSqlRaw($"Exec spMaDepartments @Criteria='{criteria}'")
                   .ToList();

                return CommonResponse.Ok(data);    
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);

            }
        }
        public CommonResponse FillDepartmentById(int Id)
        {
            try
            {
                string Criteria = "FillDepartmentWithID";
                var data = _context.spMaDepartmentsFillDepartmentById.FromSqlRaw($"Exec spMaDepartments @Criteria='{Criteria}', @ID='{Id}'")
                        .ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Department by ID", ex);
            }
        }

    //****************************************************************************
    //**********************FillDepartmentTypesById********************************************************
    public CommonResponse FillDepartmentTypes()
        {
            try
            {
                int company = _authService.GetBranchId().Value;
                int mod = 10;
                var data = _context.spDepartmentTypesFillAllDepartmentTypes
                    .FromSqlRaw($"Exec spDepartmentTypes @Mode='{mod}', @CompanyID='{company}'")
                    .ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Departementtype by ID", ex);
            }
        }
        public CommonResponse FillDepartmentTypesById(int Id)
        {
            try
            {
                var dept = _context.ReDepartmentTypes.Where(i => i.Id == Id).
                   Select(i => i.Id).
                   SingleOrDefault();
                if (dept == null)
                {
                    return CommonResponse.NotFound("Department Not Found");
                }
                int mod = 9;
                var data = _context.spDepartmentTypesGetById
                    .FromSqlRaw($"Exec spDepartmentTypes @Mode='{mod}', @ID='{Id}'")
                    .ToList();
                
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        
        //************************AddDepartmentType****************************************************
        public CommonResponse SaveDepartmentTypes(DepartmentTypeDto departmentTypeDto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;

                string criteria = "InsertReDepartmentTypes";

                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC spDepartmentTypes  @Criteria ={0},@Department={1},@CreatedBy={2},@BranchID={3},@CreatedOn={4},@NewID={5} OUTPUT",
                                                criteria, departmentTypeDto.Department, CreatedBy, CreatedBranchId, CreatedOn, newId);

                int newIdValue = (int)newId.Value;

                return CommonResponse.Created(newIdValue);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //*****************************UpdateDepartment****************************************************
        public CommonResponse UpdateDepartmentTypes(DepartmentTypeDto departmentTypeDto,int Id)
        {
            try
            {
                var dept = _context.ReDepartmentTypes.Where(i => i.Id == Id).
                    Select(i => i.Id).
                    SingleOrDefault();
                if(dept==null)
                {
                    return CommonResponse.NotFound("Department Not Found");
                }

                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                if (Id == 0)
                    return CommonResponse.Error("Branch Not Found");

                else
                {
                    
                    string criteria = "UpdateReDepartmentTypes";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spDepartmentTypes @Criteria='{criteria}',@ID='{Id}', @Department='{departmentTypeDto.Department}',@CreatedBy='{CreatedBy}',@BranchID='{CreatedBranchId}',@CreatedOn='{CreatedOn}'");
                return CommonResponse.Ok(result);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //**************************Delete*****************************
        public CommonResponse DeleteDepartmentTypes(int Id)
        {
            try
            {
                string msg = null;
                var dept = _context.ReDepartmentTypes.Where(i => i.Id == Id).
                    Select(i => i.Department).
                    SingleOrDefault();
                if (dept == null)
                {
                    msg = "Department Not Found";
                    return CommonResponse.NotFound(msg);
                }
                    
                int Mode = 3;
                msg = "DepartmentType "+dept+" is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRawAsync($"EXEC spDepartmentTypes @Mode='{Mode}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        
        
    }
}
