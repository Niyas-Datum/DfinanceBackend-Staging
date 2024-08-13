using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Item.Services
{
    public class PriceCategoryService :IPriceCategoryService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<PriceCategoryService> _logger;
        private readonly IUserTrackService _userTrackService;
        public PriceCategoryService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment,ILogger<PriceCategoryService> logger,IUserTrackService userTrack) 
        {
            _authService = authService;
            _environment = hostEnvironment;
            _context = context;
            _logger = logger;
            _userTrackService = userTrack;
        }
        public CommonResponse FillMaster()
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            var branchId = _authService.GetBranchId().Value;
            cmd.CommandText = $"Exec MaPriceCategorySP @Criteria='FillMaster'";
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
                return CommonResponse.NoContent();
            }
        }
        public CommonResponse FillPriceCategoryById(int Id)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            var branchId = _authService.GetBranchId().Value;
            cmd.CommandText = $"Exec MaPriceCategorySP @Criteria='FillPriceCategory',@ID={Id}";
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
                return CommonResponse.NoContent();
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
        public CommonResponse SavePriceCategory(PriceCategoryDto priceCategory,int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("Save PriceCategory");
                }
                var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                string criteria = "InsertMaPriceCategory";
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC MaPriceCategorySP " +
                    "@Criteria={0}, @Name={1}, @Perc={2}, @Note={3}, @CreatedBy={4}, " +
                    "@CreatedOn={5}, @CreatedBranchID={6}, @Active={7},@NewID={8} OUTPUT",
                    criteria,priceCategory.CategoryName, priceCategory.SellingPrice, priceCategory.Description,createdBy,DateTime.Now,branchId,priceCategory.Active, newId);

                var NewId = (int)newId.Value;
                _userTrackService.AddUserActivity(priceCategory.CategoryName, NewId, 0, priceCategory.Description, "PriceCategory", moduleName, 0, null);
                _logger.LogInformation("Successfully Created");
                return CommonResponse.Ok(NewId);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse UpdatePriceCategory(PriceCategoryDto priceCategory, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("Update PriceCategory");
                }
                var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                string criteria = "UpdateMaPriceCategory";
                _context.Database.ExecuteSqlRaw("EXEC MaPriceCategorySP " +
                    "@Criteria={0}, @Name={1}, @Perc={2}, @Note={3}, @CreatedBy={4}, " +
                    "@CreatedOn={5}, @CreatedBranchID={6}, @Active={7},@ID={8}",
                    criteria, priceCategory.CategoryName, priceCategory.SellingPrice, priceCategory.Description, createdBy, DateTime.Now, branchId, priceCategory.Active, priceCategory.Id);

                _userTrackService.AddUserActivity(priceCategory.CategoryName, priceCategory.Id, 1, priceCategory.Description, "PriceCategory", moduleName, 0, null);
                _logger.LogInformation("Successfully Created");
                return CommonResponse.Ok(priceCategory.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse DeletePriceCategory(int id, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("Delete PriceCatgory");
                }
                var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();               
                string criteria = "DeleteMaPriceCategory";
                _context.Database.ExecuteSqlRaw("EXEC MaPriceCategorySP " +
                    "@Criteria={0}, @ID={1}",
                    criteria, id);

                _userTrackService.AddUserActivity(criteria, id, 2, "Deleted", "PriceCategory", moduleName, 0, null);
                _logger.LogInformation("Successfully Deleted");
                return CommonResponse.Ok("Successfully Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}
