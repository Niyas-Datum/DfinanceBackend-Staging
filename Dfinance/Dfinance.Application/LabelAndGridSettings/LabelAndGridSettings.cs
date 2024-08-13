using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.AuthApplication.Services;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.General;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

namespace Dfinance.Application.LabelAndGridSettings
{
    public class LabelAndGridSettings : ILabelAndGridSettings
    {

        private readonly IAuthService _authService;
        private readonly DFCoreContext _context;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<LabelAndGridSettings> _logger;
        public LabelAndGridSettings(IAuthService authService, DFCoreContext dFCoreContext, IPasswordService passwordService, ILogger<LabelAndGridSettings> logger)
        {

            _authService = authService;
            _context = dFCoreContext;
            _passwordService = passwordService;
            _logger = logger;
        }
        public CommonResponse FillGridSettings()
        {
            try
            {
                int BranchId = _authService.GetBranchId().Value;
                string criteria = "FillGridSettings";
                var data = _context.FormGridView.FromSqlRaw($"Exec LabelSettingsSP @Criteria='{criteria}'").ToList();
                _logger.LogInformation("Successfully filled grid");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillFormLabelSettings()
        {
            try
            {
                int BranchId = _authService.GetBranchId().Value;
                string criteria = "FillFormLabelSettings";
                var data = _context.FormLabelView.FromSqlRaw($"Exec LabelSettingsSP @Criteria='{criteria}'").ToList();
                _logger.LogInformation("Successfully filled label");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FormNamePopup()
        {
            try
            {
                
                var formNames = _context.MaPageMenus
            .Where(m => m.FormName != null)
            .AsEnumerable() 
            .Select(m =>
            {
                var index = m.FormName.IndexOf('.');
                return index >= 0
                    ? m.FormName.Substring(index + 1)
                    : m.FormName; 
            })
            .Distinct()
            .ToList();
                return CommonResponse.Ok(formNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        //page popup in formgridsettings
        public CommonResponse PagePopUp()
        {
            try
            {
                var pages = _context.MaPageMenus
            .Where(p => p.IsPage == true) 
            .Select(p => new  
            {
                ID = p.Id,
                MenuText = p.MenuText,
                MenuValue = p.MenuValue,
                FormName = p.FormName,
                AssemblyName = p.AssemblyName,
                PageTitle = p.PageTitle,
                VoucherID = p.VoucherId
            })
            .ToList(); 
                
            return CommonResponse.Ok(pages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse SaveAndUpdateLabel(List<LabelDto> labelDto, string password)
        {
            try
            {
                var criteria = "";
                var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                if ((bool)passwordCheckResponse.Data != true)
                {
                    return CommonResponse.Ok("Invalid password");
                }
                foreach (var label in labelDto)
                {
                    if (label.Id == 0 || label == null)
                    {

                        criteria = "InsertFormLabelSettings";
                        SqlParameter newIdItem = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var result = _context.Database.ExecuteSqlRaw("EXEC LabelSettingsSP @Criteria={0},@FormName={1},@LabelName={2},@OriginalCaption={3},@NewCaption={4},@Visible={5},@ArabicCaption={6},@PageID={7},@NewID={8} OUTPUT",
                            criteria, label.FormName.Name, label.LabelName, label.OriginalCaption, label.NewCaption, label.Visible, label.ArabicCaption, null, newIdItem);
                        var NewItemId = (int)newIdItem.Value;
                        _logger.LogInformation("Label.Inserted with ID: {Id}", NewItemId);
                    }
                    else
                    {
                        var check = _context.FormLabelSettings.Any(x => x.Id == label.Id);
                        if (!check) { return CommonResponse.NotFound("Id not found"); }
                        criteria = "UpdateFormLabelSettings";
                        var res = _context.Database.ExecuteSqlRaw("Exec LabelSettingsSP @Criteria ={0},@FormName={1},@LabelName={2},@OriginalCaption={3},@NewCaption={4},@Visible={5},@PageID={6},@ArabicCaption={7},@ID={8}",
                            criteria, label.FormName.Name, label.LabelName, label.OriginalCaption, label.NewCaption, label.Visible, null,label.ArabicCaption, label.Id);
                        _logger.LogInformation("Label.Updated with ID: {Id}", label.Id);
                    }
                }
                return CommonResponse.Ok("Processed successfully!");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }


        public CommonResponse SaveAndUpdateGrid(List<GridDto> gridDto, string password)
        {
            try
            {
                var criteria = "";
                var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                if ((bool)passwordCheckResponse.Data != true)
                {
                    return CommonResponse.Ok("Invalid password");
                }
                foreach (var grid in gridDto)
                {
                    if (grid.Id == 0 || grid == null)
                    {
                        criteria = "InsertFormGridSettings";
                        SqlParameter newIdItem = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var result = _context.Database.ExecuteSqlRaw("EXEC LabelSettingsSP @Criteria={0},@FormName={1},@PageId={2},@GridName={3},@ColumnName={4},@OriginalCaption={5},@NewCaption={6},@Visible={7},@ArabicCaption={8},@NewID={9} OUTPUT",
                            criteria, grid.FormName.Name, grid.PageId, grid.GridName, grid.ColumnName, grid.OriginalCaption, grid.NewCaption, grid.Visible, grid.ArabicCaption, newIdItem);
                        var NewItemId = (int)newIdItem.Value;
                        _logger.LogInformation("grid.Inserted with ID: {Id}", NewItemId);
                    }
                    else
                    {
                        var check = _context.FormGridSettings.Any(x => x.Id == grid.Id);
                        if (!check) { return CommonResponse.NotFound("Id not found"); }
                        var res = criteria = "UpdateFormGridSettings";
                        _context.Database.ExecuteSqlRaw("Exec LabelSettingsSP @Criteria ={0},@FormName={1},@PageID={2},@GridName={3},@ColumnName={4},@OriginalCaption={5},@NewCaption={6},@Visible={7},@ArabicCaption={8},@ID={9}",
                           criteria, grid.FormName.Name, grid.PageId, grid.GridName, grid.ColumnName, grid.OriginalCaption, grid.NewCaption, grid.Visible, grid.ArabicCaption, grid.Id);
                        _logger.LogInformation("grid.Updated with ID: {Id}", grid.Id);
                    }
                }
                return CommonResponse.Ok("Processed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }



        public CommonResponse labelGridpopup()
        {
            var labelgrid = _context.FormGridSettings
                            .Join(_context.MaPageMenus,
                            md => md.PageId,
                            rd => rd.Id,
                            (md, rd) => new
                            {
                                Id = md.PageId,
                                MenuText = rd.MenuText,
                                MenuValue = rd.MenuValue,
                                FormName = rd.FormName,
                                AssemblyName = rd.AssemblyName,
                                PageTitle = rd.PageTitle,
                                VoucherId = rd.VoucherId
                            })
                           .ToList();

            return CommonResponse.Ok(labelgrid);
        }

        public CommonResponse GetGridByPageId(int pageId)
        {
            var check = _context.FormGridSettings.Any(x => x.PageId == pageId);
            if (!check) { return CommonResponse.NotFound("Id not found"); }
            var grid = _context.FormGridSettings.Where(i=>i.PageId == pageId && i.Visible==true).ToList();
            return CommonResponse.Ok(grid);
        }




    }
}




