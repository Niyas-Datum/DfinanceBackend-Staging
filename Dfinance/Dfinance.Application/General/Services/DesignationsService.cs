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
    public class DesignationsService : IDesignationsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public DesignationsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public CommonResponse FillAllDesignation()
        {
            
            var data = _context.SpFillDesignationMaster.FromSqlRaw("Exec spMaDesignations @Criteria = 'FillDesignationMaster'").ToList();

            return CommonResponse.Ok(data);
        }
        public CommonResponse FillDesignationById(int Id)
        {
                try
                {
                    string criteria = "FillDesignationWithID";
                    var result = _context.SpDesignationMasterByIdG.FromSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@ID='{Id}'").ToList();
               
            return CommonResponse.Ok(result);
                }
         catch (Exception ex)
          {
             return CommonResponse.Error(ex);
           }
}
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
        public CommonResponse UpdateDesignation(DesignationsDto designationsdto, int Id)
        {
            try
            {
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
            public CommonResponse DeleteDesignation(int Id)
            {

                try
                {
                    int Mode = 3;
                    string msg = "Designation is Suspended";
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
    

