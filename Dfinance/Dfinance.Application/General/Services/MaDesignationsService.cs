using Dfinance.Application.Dto.General;
using Dfinance.Application.General.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Application.General.Services
{
    public class MaDesignationsService : IMaDesignationsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public MaDesignationsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public CommonResponse GetAllDesignation()
        {
            
            var data = _context.SpFillDesignationMaster.FromSqlRaw("Exec spMaDesignations @Criteria = 'FillDesignationMaster'").ToList();

           
            var maDesignation= data.Select(item => new SpDesignationMasterG
            {
                ID = item.ID,
                Name = item.Name
            }).ToList();

            
            return CommonResponse.Ok(maDesignation);
        }
        public CommonResponse GetAllDesignationById(int Id)
        {
                try
                {
                    string criteria = "FillDesignationWithID";
                    var result = _context.SpDesignationMasterByIdG.FromSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@ID='{Id}'").ToList();
                var res = result.Select(i => new SpDesignationMasterByIdG
                {
                    ID = i.ID,
                    Name = i.Name,
                    Company = i.Company,
                    CreatedBranchID = i.CreatedBranchID,
                    CreatedBy = i.CreatedBy,
                    CreatedOn = i.CreatedOn,
                    ActiveFlag = i.ActiveFlag,
                 }).ToList();
            return CommonResponse.Ok(res);
                }
         catch (Exception ex)
          {
             return CommonResponse.Error(ex);
           }
}
public CommonResponse AddDesignations(MaDesignationsDto designationsdto)
        {
            try
            {
                designationsdto.CreatedBy = _authService.GetId().Value;
                designationsdto.CreatedBranchID = _authService.GetBranchId().Value;
                MaDesignation madesignations = new MaDesignation
                {
                    Name = designationsdto.Name,
                    CreatedBy = designationsdto.CreatedBy,
                    CreatedBranchId = designationsdto.CreatedBranchID,
                    CreatedOn = designationsdto.CreatedOn,
                };
                string criteria = "InsertMaDesignations";
                var result = _context.spMaDesignationsC.FromSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@Name='{madesignations.Name}',@CreatedBy='{madesignations.CreatedBy}',@CreatedBranchID='{madesignations.CreatedBranchId}',@CreatedOn='{madesignations.CreatedOn}'").ToList();
                var NewId = result.Select(i => new spMaDesignationsC
                {
                    NewID = i.NewID
                }).ToList();
                return CommonResponse.Created(NewId);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse UpdateDesignation(MaDesignationsDto designationsdto, int Id)
        {
            try
            {
                designationsdto.CreatedBy = _authService.GetId().Value;
                designationsdto.CreatedBranchID= _authService.GetBranchId().Value;
                if (Id == 0)
                    return CommonResponse.Error("Designation Not Found");
                else
                {
                    MaDesignation madesignations = new MaDesignation
                    {
                        Name = designationsdto.Name,
                        CreatedBy = designationsdto.CreatedBy,
                        CreatedBranchId = designationsdto.CreatedBranchID,
                        CreatedOn = designationsdto.CreatedOn,
                    };
                    string criteria = "UpdateMaDesignations";
                    var result = _context.spMaDesignationsC.FromSqlRaw($"EXEC spMaDesignations @Criteria='{criteria}',@ID='{Id}', @Name='{madesignations.Name}',@CreatedBy='{madesignations.CreatedBy}',@CreatedBranchID='{madesignations.CreatedBranchId}',@CreatedOn='{madesignations.CreatedOn}'").ToList();
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
                    var result = _context.spMaDesignationsC.FromSqlRaw($"EXEC spMaDesignations @Mode='{Mode}',@ID='{Id}'").ToList();
                    return CommonResponse.Ok(msg);
                }
                catch (Exception ex)
                {
                    return CommonResponse.Error(ex);
                }
            }
            }
    }
    

