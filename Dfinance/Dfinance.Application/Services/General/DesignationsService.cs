using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class DesignationsService : IDesignationsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public DesignationsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        //**************************Fill All Designation *****************************
        public CommonResponse FillAllDesignation()
        {

            var data = _context.SpFillDesignationMaster.FromSqlRaw("Exec spMaDesignations @Criteria = 'FillDesignationMaster'").ToList();

            return CommonResponse.Ok(data);
        }
        //**************************Fill DesignationById *****************************
        public CommonResponse FillDesignationById(int Id)
        {
            try
            {
                string msg = null;
                var desig = _context.MaDesignations.Where(i => i.Id == Id).
                    Select(i => i.Id).
                    SingleOrDefault();
                if (desig == null)
                {
                    msg = "Designation Not Found";
                    return CommonResponse.NotFound(msg);
                }
                string criteria = "FillDesignationWithID";
                var result = _context.SpDesignationMasterByIdG.FromSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@ID='{Id}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //**************************Save Designation *****************************
        public CommonResponse SaveDesignations(DesignationsDto designationsdto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;

                string criteria = "InsertMaDesignations";

                SqlParameter newIdUserRight = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                //var result = _context.Database.ExecuteSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@Name='{designationsdto.Name}',@CreatedBy='{CreatedBy}',@CreatedBranchID='{CreatedBranchId}',@CreatedOn='{CreatedOn}',@NewID='{newIdUserRight}' OUTPUT");
                var result = _context.Database.ExecuteSqlRaw("EXEC spMaDesignations " +
         "@Criteria={0}, @Name={1}, @CreatedBy={2}, @CreatedBranchID={3}, @CreatedOn={4}, @NewID={5} OUTPUT",
         criteria, designationsdto.Name, CreatedBy, CreatedBranchId, CreatedOn, newIdUserRight);

                int NewIdUserRighnt = (int)newIdUserRight.Value;
                return CommonResponse.Created(NewIdUserRighnt);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //**************************Update Designation *****************************
        public CommonResponse UpdateDesignation(DesignationsDto designationsdto, int Id)
        {
            try
            {
                string msg = null;
                var desig = _context.MaDesignations.Where(i => i.Id == Id).
                    Select(i => i.Id).
                    SingleOrDefault();
                if (desig == null)
                {
                    msg = "Designation Not Found";
                    return CommonResponse.NotFound(msg);
                }
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                if (Id == 0)
                    return CommonResponse.Error("Designation Not Found");
                else
                {

                    string criteria = "UpdateMaDesignations";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@ID='{Id}', @Name='{designationsdto.Name}',@CreatedBy='{CreatedBy}',@CreatedBranchID='{CreatedBranchId}',@CreatedOn='{CreatedOn}'");
                    return CommonResponse.Ok(result);

                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //**************************Delete Designation*****************************
        public CommonResponse DeleteDesignation(int Id)
        {

            try
            {
                string msg = null;
                var userDes = _context.MaEmployees.Any(m => m.DesignationId == Id);
                if (userDes)
                    return CommonResponse.Ok("Cannot Delete the Designation");
                var desig = _context.MaDesignations.Where(i => i.Id == Id).
                    Select(i => i.Name).
                    SingleOrDefault();
                if (desig == null)
                {
                    msg = "Designation Not Found";
                    return CommonResponse.NotFound(msg);
                }
                int Mode = 3;
                msg = "Designation " +desig+ " is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRaw($"EXEC spMaDesignations @Mode='{Mode}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}


