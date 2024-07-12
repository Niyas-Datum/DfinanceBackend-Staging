using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class PageMenuService:IPageMenuService
    {
        private readonly DFCoreContext _context;
        private readonly ILogger<IPageMenuService> _logger;
        public PageMenuService(DFCoreContext context, ILogger<IPageMenuService> logger)
        {
            _context = context;
            _logger = logger;
        }
        //fills the first level of menu(without input parentId)
        //fills the menu under parent(providing parentId)
        //fills complete pages from PageMenu
        public CommonResponse FillMenu(bool AllPages,int? parentId = null)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                if (parentId == null && !AllPages)
                    cmd.CommandText = $"Exec MaPageMenuSP @Criteria='FillFirstLevelMenu'";
                else if(parentId!=null && !AllPages)
                    cmd.CommandText = $"Exec MaPageMenuSP @Criteria='FillMenuByParent',@ParentID={parentId}";
                else if(AllPages)
                    cmd.CommandText = $"Exec MaPageMenuSP @Criteria='FillMaPageMenu'";

                _context.Database.GetDbConnection().Open();

                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);

                    if (tb.Rows.Count > 0)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        foreach (DataRow dr in tb.Rows)
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                            rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);
                    }
                    else
                    {
                        return CommonResponse.NoContent("No Data");
                    }
                }
            }
        }
       
        //fills the dropdown for Group and Modules in pagemenu
        public CommonResponse FillGroupAndModules()
        {
            var groups = _context.MenuGroupView.FromSqlRaw("exec DropDownListSP @Criteria='FillMenuGroups'").ToList();
            var modules = _context.DropDownViewName.FromSqlRaw("exec DropDownListSP @Criteria='FillModules'").ToList();
            return CommonResponse.Ok(new {Groups= groups, Modules = modules});
        }

        //save or update pagemenu
        public CommonResponse SavePageMenu(PageMenuDto pageMenuDto,int? parentId=null)
        {
            try
            {
                if (pageMenuDto == null)
                    return CommonResponse.NoContent();
                string criteria = "";
                int? refPageId = null, helpId = null;
                //insertion of pagemenu
                if (pageMenuDto.Id == 0 || pageMenuDto.Id == null)
                {
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    criteria = "InsertMaPageMenu";
                    _context.Database.ExecuteSqlRaw("Exec MaPageMenuSP @Criteria={0},@MenuText={1},@MenuValue={2},@UrlID={3},@Url={4},@IsFinanceRef={5},@RefPageID={6},@MenuOrder={7},@VoucherID={8}," +
                        "@Active={9},@ModuleID={10},@ParentID={11},@IsPage={12},@MenuLevel={13},@MenuPermission={14},@PageTitle={15},@HelpID={16},@AssemblyName={17},@FormName={18}," +
                        "@IsMaximized={19},@MDIParent={20},@FrequentlyUsed={21},@ShortcutKey={22},@ArabicName={23},@ProcedureName={24},@NewID={25} OUTPUT",
                        criteria, pageMenuDto.MenuText, pageMenuDto.MenuValue, pageMenuDto.UrlID, pageMenuDto.Url, pageMenuDto.IsFinanceRef, refPageId, pageMenuDto.MenuOrder,
                        pageMenuDto.Voucher.Id, pageMenuDto.Active, pageMenuDto.Module.Id, parentId, pageMenuDto.IsPage, pageMenuDto.MenuLevel, pageMenuDto.MenuPermission.Key,
                        pageMenuDto.PageTitle, helpId, pageMenuDto.AssemblyName, pageMenuDto.FormName, pageMenuDto.IsMaximized, pageMenuDto.MDIParent, pageMenuDto.FrequentlyUsed,
                        pageMenuDto.ShortcutKey, pageMenuDto.ArabicName, pageMenuDto.ProcedureName, newId);
                }
                //updating pagemenu
                else
                {
                    criteria = "UpdateMaPageMenu";
                    _context.Database.ExecuteSqlRaw("Exec MaPageMenuSP @Criteria={0},@MenuText={1},@MenuValue={2},@UrlID={3},@Url={4},@IsFinanceRef={5},@RefPageID={6},@MenuOrder={7},@VoucherID={8}," +
                       "@Active={9},@ModuleID={10},@ParentID={11},@IsPage={12},@MenuLevel={13},@MenuPermission={14},@PageTitle={15},@HelpID={16},@AssemblyName={17},@FormName={18}," +
                       "@IsMaximized={19},@MDIParent={20},@FrequentlyUsed={21},@ShortcutKey={22},@ArabicName={23},@ProcedureName={24},@ID={25}",
                       criteria, pageMenuDto.MenuText, pageMenuDto.MenuValue, pageMenuDto.UrlID, pageMenuDto.Url, pageMenuDto.IsFinanceRef, refPageId, pageMenuDto.MenuOrder,
                       pageMenuDto.Voucher.Id, pageMenuDto.Active, pageMenuDto.Module.Id, parentId, pageMenuDto.IsPage, pageMenuDto.MenuLevel, pageMenuDto.MenuPermission.Key,
                       pageMenuDto.PageTitle, helpId, pageMenuDto.AssemblyName, pageMenuDto.FormName, pageMenuDto.IsMaximized, pageMenuDto.MDIParent, pageMenuDto.FrequentlyUsed,
                       pageMenuDto.ShortcutKey, pageMenuDto.ArabicName, pageMenuDto.ProcedureName, pageMenuDto.Id);
                }
                _logger.LogInformation("PageMenu Saved Successfully");
                return CommonResponse.Ok("PageMenu Saved Successfully");
            }
            catch
            {
                _logger.LogError("PageMenu Not Saved");
                return CommonResponse.Error("PageMenu Not Saved");
            }
        }
        public CommonResponse DeletePageMenu(int Id)
        {
            try
            {
                string criteria = "DeleteMaPageMenu";
                _context.Database.ExecuteSqlRaw("Exec MaPageMenuSP @Criteria={0},@ID={1}", criteria, Id);
                _logger.LogInformation("PageMenu Deleted Successfully");
                return CommonResponse.Ok("PageMenu Deleted Successfully");

            }
            catch
            {
                _logger.LogError("PageMenu Cannot Deleted");
                return CommonResponse.Error("PageMenu Cannot Deleted");
            }
        }
        public CommonResponse UpdateActive(List<PageActiveDto> pageActiveDto)
        {
            try
            {               
                foreach(var page in  pageActiveDto)
                {
                    _context.Database.ExecuteSqlRaw($"Exec MaPageMenuSP @Criteria='UpdateActiveForWeb',@ID={page.PageId},@Active='{page.Active}',@FrequentlyUsed='{page.FrequentlyUsed}'");
                }
                return CommonResponse.Ok("Activated Successfully");
            }
            catch
            {
                _logger.LogError("PageMenu Cannot Updated");
                return CommonResponse.Error("PageMenu Cannot Updated");
            }
        }
    }
}
