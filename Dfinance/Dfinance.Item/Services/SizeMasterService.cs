using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.InvRoute;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dfinance.Item.Services
{
    public class SizeMasterService : ISizeMasterService
    {

        private readonly DFCoreContext _context;
        private readonly ILogger<SizeMasterService> _logger;
        private readonly IAuthService _authService;
        public SizeMasterService(DFCoreContext context, ILogger<SizeMasterService> logger, IAuthService authService)
        {
            _context = context;
            _logger = logger;
            _authService = authService;
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        //leftside fill
        //take Id,Code,Name from table InvSizeMaster
        public CommonResponse FillMaster()
        {
            var sizeMasters = _context.InvSizeMaster.Select(s => new {s.Id, s.Code, s.Name,s.Active }).ToList();
            return CommonResponse.Ok(sizeMasters);
        }
        //fill by Id
        public CommonResponse FillById(int id)
        {
            var sizeMaster=_context.InvSizeMaster.Where(s=>s.Id == id).Select(s => new {s.Id,s.Code,s.Name,s.Active,s.Description}).FirstOrDefault();
            return CommonResponse.Ok(sizeMaster);
        }
        public CommonResponse SaveSizeMaster(SizeMasterDto sizeMasterDto,int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Size Master");
            }
            try
            {
                string criteria = "";
                if (sizeMasterDto.Id == 0 || sizeMasterDto.Id == null)
                {
                    criteria = "InsertInvSizeMaster";
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    _context.Database.ExecuteSqlRaw("Exec SizeMasterSP @Criteria={0},@Code={1},@Name={2},@Active={3},@Description={4},@NewID={5} OUTPUT",
                        criteria, sizeMasterDto.Code, sizeMasterDto.Name, sizeMasterDto.Active, sizeMasterDto.Description, newId);
                    _logger.LogInformation("Size Master Saved Successfully");
                    return CommonResponse.Ok("Saved Successfully");
                }
                else
                {
                    var existsId = _context.InvSizeMaster.Any(x => x.Id == sizeMasterDto.Id);
                    if (!existsId)
                        return CommonResponse.NotFound("Size Not exists");
                    criteria = "UpdateInvSizeMaster";
                    _context.Database.ExecuteSqlRaw("Exec SizeMasterSP @Criteria={0},@Code={1},@Name={2},@Active={3},@Description={4},@ID={5} ",
                       criteria, sizeMasterDto.Code, sizeMasterDto.Name, sizeMasterDto.Active, sizeMasterDto.Description, sizeMasterDto.Id);
                    _logger.LogInformation("Size Master Updated Successfully");
                    return CommonResponse.Ok("Updated Successfully");
                }
            }
            catch 
            {
                _logger.LogError("Saving SizeMaster Failed");
                return CommonResponse.Error("Saving SizeMaster Failed");
            }
        }
        public CommonResponse DeleteSizeMaster(int id,int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 5))
            {
                return PermissionDenied("Delete SizeMaster");
            }
            try
            {
                var existsId = _context.InvSizeMaster.Any(x => x.Id == id);
                if (!existsId)
                    return CommonResponse.NotFound("Size Not exists");
                string criteria = "DeleteInvSizeMaster";
                _context.Database.ExecuteSqlRaw("Exec SizeMasterSP @Criteria={0},@ID={1}", criteria, id);
                return CommonResponse.Ok("Size Deleted Successfully");
            }
            catch
            {
                _logger.LogError("Failed to delete SizeMaster");
                return CommonResponse.Error("Failed to delete SizeMaster");
            }
        }
    }

}
