using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class SubMastersService : ISubMastersService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<SubMastersService> _logger;
        public SubMastersService(DFCoreContext context, IAuthService authService, ILogger<SubMastersService> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        public CommonResponse KeyDropDown()
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "exec SPMaMisc @Criteria='FillDropDownList'";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count > 0)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in tb.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                            rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);
                    }
                }
                return CommonResponse.NotFound();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //LeftsideFill
        public CommonResponse FillMaster(string Key)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"exec SPMaMisc @Criteria='FillMaster',@key='{Key}'";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count > 0)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in tb.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                            rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);
                    }
                }
                return CommonResponse.NotFound();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse FillSubMasterById(int? Id)
        {
            try
            {
                string result = Id.HasValue ? Id.ToString() : "NULL";
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"exec SPMaMisc @Criteria='Fill',@ID={result}";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count > 0)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in tb.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                            rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);
                    }
                }
                return CommonResponse.NotFound();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse SaveSubMasters(SubMasterDto submasterDto,int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("Save SubMasters");
                }
                string criteria = "Insert";
                bool active = true;
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var result = _context.Database.ExecuteSqlRaw("EXEC SPMaMisc @Criteria={0},@Key={1},@Value={2},@Description={3},@Active={4},@Code={5},@NewID={6} OUTPUT",
                                criteria, submasterDto.Key.Value, submasterDto.Value, submasterDto.Description, active, submasterDto.Code, newIdparam);
                int NewIdUser = (int)newIdparam.Value;
                _logger.LogInformation("SavedSubMaster!");
                return CommonResponse.Ok("Saved Sucessfully!");

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse UpdateSubMasters(SubMasterDto submasterDto, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 3))
                {
                    return PermissionDenied("Update SubMasters");
                }
                string criteria = "Update";
                bool active = true;
                var subma = _context.MaMisc.Where(i => i.Id == submasterDto.Id).Select(i => i.Id).SingleOrDefault();
                if (subma == 0)
                {
                    return CommonResponse.NotFound("Id is not found!");
                }
                var result = _context.Database.ExecuteSqlRaw("EXEC SPMaMisc @Criteria={0},@Key={1},@Value={2},@Description={3},@Active={4},@Code={5},@ID={6} ",
                                criteria, submasterDto.Key.Value, submasterDto.Value, submasterDto.Description, active, submasterDto.Code, submasterDto.Id);
              
                _logger.LogInformation("UpdatedSubMaster!");
                return CommonResponse.Ok("Updated Sucessfully!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
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

        public CommonResponse DeleteCounter(int Id, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 5))
                {
                    return PermissionDenied("Delete SubMaster");
                }
                var counter = _context.MaMisc.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (counter == 0)
                {
                    return CommonResponse.NotFound("Id is not found!");
                }
                string criteria = "Delete";
                var data = _context.Database.ExecuteSqlRaw("EXEC SPMaMisc @Criteria ={0},@Id={1}", criteria, Id);
                _logger.LogInformation("SubMaster deleted");
                return CommonResponse.Ok("Delete successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }

        }
      



    }
}
