using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class DepartmentTypeService : IDepartmentTypeService
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
        public CommonResponse FillDepartment()
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
        public CommonResponse FillDepartmentById(int Id)
        {
            try
            {
                string Criteria = "FillDepartmentWithIDWeb";
                var data = _context.spMaDepartmentsFillDepartmentById.FromSqlRaw($"Exec spMaDepartments @Criteria='{Criteria}', @ID='{Id}'")
                        .ToList();
                return CommonResponse.Ok(data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Department by ID", ex);
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

                int Mode = 20;
                msg = "DepartmentType " + dept + " is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRaw($"EXEC spMaDepartments @Mode='{Mode}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("Cannot delete because it is referenced in department.");
            }
        } //**************************Delete From MaDepartment *****************************
        public CommonResponse DeleteUpdate(int Id)
        {
            try
            {
                int Mode = 21;
                var result = _context.Database.ExecuteSqlRaw($"EXEC spMaDepartments @Mode='{Mode}',@ID='{Id}'");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("Cannot delete because it is referenced.");
            }
        }


        public CommonResponse AddDepartment(DepartmentTypeDto departmentDto)
        {
            try
            {

                // Department exist or not

                if (departmentDto.DepId == 0)
                {
                    var departmentExists = _context.ReDepartmentTypes.Any(i => i.Department == departmentDto.Department);

                    if (departmentExists)
                    {
                        return CommonResponse.Error("Department already exists");
                    }
                    var newId = SaveDepartmentTypes(departmentDto.Department);
                    if (newId > 0)
                    {
                        foreach (var branchId in departmentDto.Branch.Select(b => b.Id))
                        {
                            var result = SaveDepartment(newId, branchId);
                        }

                    }
                   
                    return CommonResponse.Created(new { msg = "Department " + departmentDto.Department + " Created Successfully", data = 0 });
                }



                else
                {

                    var updatedeptype = UpationDepartmentTypes(departmentDto.Department, departmentDto.DepId);

                    // Load all departments where DepId
                    var allDepartments = _context.MaDepartments
                        .Where(dd => dd.DepartmentTypeId == departmentDto.DepId)
                        .ToList();

                    foreach (var branchDto in departmentDto.Branch)
                    {
                        // Branch DTO check with all departments
                        var existingDept = allDepartments.FirstOrDefault(dd => dd.CompanyId == branchDto.Id);

                        if (existingDept == null)
                        {
                            var result = SaveDepartment(departmentDto.DepId, branchDto.Id);
                        }
                        else
                        {
                            var updatedept = UpdationDepartment(departmentDto.DepId, branchDto.Id);
                            // Remove all other departments except the updated one
                            allDepartments.Remove(existingDept);
                        }
                        
                    }
                  
                    _context.MaDepartments.RemoveRange(allDepartments);
                    _context.SaveChanges();
                    return CommonResponse.Ok(new { msg = "Department " + departmentDto.Department + " Update Successfully", data = 0 });

                }
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

        //**********************************************************************UpdateDepartment************************************************************
        private int UpdationDepartment(int DepId, int compid)
        {
            try
            {
                var dept = _context.MaDepartments
                         .Where(x => x.DepartmentTypeId == DepId && x.CompanyId == compid)
                         .FirstOrDefault();
                if (dept == null)
                {

                    return 0;
                }
                int createdBy = _authService.GetId().Value;
                //int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;

                string criteria = "UpdateMaDepartments";
                var result = _context.Database.ExecuteSqlRaw("EXEC spMaDepartments @Criteria={0},@DepartmentTypeID={1},@CreatedBy={2},@CompanyID={3},@CreatedOn={4},@ID={5}",
                                                 criteria, DepId, createdBy, compid, createdOn, dept.Id);

                return result;



            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //*********************************************************************************************************************************************************************
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
