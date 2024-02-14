using Dfinance.Application.Dto;
using Dfinance.Application.Dto.General;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

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

   

      

        //*****************************UpdateDepartment****************************************************
       
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


        public CommonResponse AddDepartment(DepartmentTypeDto departmentDto)
        {
            try
            {
                
                // Department exist or not
                var departmentExists = _context.ReDepartmentTypes.Any(i => i.Department == departmentDto.Department);
                if (departmentExists)
                {
                    return CommonResponse.Error("Department already exists");
                }

                if (departmentDto.Id == 0)
                {
                    if (departmentDto.DepId == 0)
                    {
                        var newId = SaveDepartmentTypes(departmentDto.Department);
                        if (newId > 0)
                        {
                            foreach (var branchId in departmentDto.Branch.Select(b => b.Id))
                            {
                                var result = SaveDepartment(newId, branchId);
                            }
                        }
                    }
                }
                else
                {

                    var updatedeptype = UpationDepartmentTypes(departmentDto.Department, departmentDto.Id);
                    foreach (var branchId in departmentDto.Branch.Select(b => b.Id))
                    {
                        var updatedept = UpdationDepartment(departmentDto.DepId, branchId, departmentDto.Id);
                    }

                    return CommonResponse.Ok(departmentDto);

                    
                }
                
                return CommonResponse.Ok("Inserted Sucessfully");
            
             
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
 //**********************************************************************************************************************************
        private int SaveDepartment(int DepId, int compid)
        {
            try
            {
                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;

                string criteria = "InsertMaDepartments";

                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC spMaDepartments @Criteria={0},@DepartmentTypeID={1},@CreatedBy={2},@CompanyID={3},@CreatedOn={4},@NewID={5} OUTPUT",
                                                criteria, DepId, createdBy, compid, createdOn, newId);

                int newdepId = (int)newId.Value;

                return newdepId;
            }



            catch (Exception ex)
            {
                return 0;
            }
        }

        private int SaveDepartmentTypes(string departmentName)
        {
            try
            {
                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;



                string criteria = "InsertReDepartmentTypes";

                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC spDepartmentTypes @Criteria={0},@Department={1},@CreatedBy={2},@BranchID={3},@CreatedOn={4},@NewID={5} OUTPUT",
                                                criteria, departmentName, createdBy, createdBranchId, createdOn, newId);

                int newIdValue = (int)newId.Value;

                return newIdValue;
            }


            catch (Exception ex)
            {
                return 0;
            }
        }

        //*****************************UpdateDepartment****************************************************
        private int UpdationDepartment(int DepId, int compid, int Id)
        {
            try
            {
                var dept = _context.MaDepartments.Find(Id);
                if (dept == null)
                {

                    return 0;
                }
                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;

                string criteria = "UpdateMaDepartments";
                var result = _context.Database.ExecuteSqlRaw("EXEC spMaDepartments @Criteria={0},@DepartmentTypeID={1},@CreatedBy={2},@CompanyID={3},@CreatedOn={4},@ID={5}",
                                                 criteria, DepId, createdBy, compid, createdOn, Id);

                return result;



            }
            catch (Exception ex)
            {
                return 0;
            }
        }
     //**********************************************************************************************************************************
        private int UpationDepartmentTypes(string departmentName, int Id)
        {
            try
            {
                var dept = _context.ReDepartmentTypes.Find(Id);
                if (dept == null)
                {

                    return 0;
                }

                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;


                string criteria = "UpdateReDepartmentTypes";
                var result = _context.Database.ExecuteSqlRaw($"EXEC spDepartmentTypes @Criteria='{criteria}',@ID='{Id}', @Department='{departmentName}',@CreatedBy='{createdBy}',@BranchID='{createdBranchId}',@CreatedOn='{createdOn}'");
                return result;


            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
