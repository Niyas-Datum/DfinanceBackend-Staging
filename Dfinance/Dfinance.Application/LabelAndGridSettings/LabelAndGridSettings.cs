using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public CommonResponse UpdateLabel(List<LabelDto> labelDto, string password)
        {
            try
            {


                var existLabels = _context.FormLabelSettings
                                   .Where(v => labelDto.Select(dto => dto.Id).Contains(v.Id))
                                   .ToList();
                if (existLabels.Count != labelDto.Count)
                {
                    return CommonResponse.NotFound("Id Not Found");
                }
                else
                {
                    var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                    if ((bool)passwordCheckResponse.Data != true)
                    {
                        return CommonResponse.Ok("Invalid password");
                    }

                    foreach (var label in labelDto)
                    {
                        var labelToUpdate = existLabels.FirstOrDefault(v => v.Id == label.Id);

                        if (labelToUpdate != null)
                        {
                            string criteria = "UpdateFormLabelSettings";
                            _context.Database.ExecuteSqlRaw("Exec LabelSettingsSP @Criteria ={0},@FormName={1},@LabelName={2},@OriginalCaption={3},@NewCaption={4},@Visible={5},@PageID={6},@Enable={7},@ArabicCaption={8},@ID={9}",
                                criteria, label.FormName.Name, label.LabelName, label.OriginalCaption, label.NewCaption, label.Visible, label.PageId, label.Enable, label.ArabicCaption, label.Id);
                        }
                    }
                    _logger.LogInformation("Label updated Successfully");
                    return CommonResponse.Ok("Updated Successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse UpdateGrid(List<GridDto> gridDto, string password)
        {
            try
            {
                var existGrids = _context.FormGridSettings
                                   .Where(v => gridDto.Select(dto => dto.Id).Contains(v.Id))
                                   .ToList();
                if (existGrids.Count != gridDto.Count)
                {
                    return CommonResponse.NotFound("Id Not Found");
                }
                else
                {
                    var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                    if ((bool)passwordCheckResponse.Data != true)
                    {
                        return CommonResponse.Ok("Invalid password");
                    }
                    foreach (var grid in gridDto)
                    {
                        var gridToUpdate = existGrids.FirstOrDefault(v => v.Id == grid.Id);

                        if (gridToUpdate != null)
                        {
                            string criteria = "UpdateFormGridSettings";
                            _context.Database.ExecuteSqlRaw("Exec LabelSettingsSP @Criteria ={0},@FormName={1},@PageID={2},@GridName={3},@ColumnName={4},@OriginalCaption={5},@NewCaption={6},@Visible={7},@ArabicCaption={8},@ID={9}",
                               criteria, grid.FormName.Name, grid.PageId, grid.GridName, grid.ColumnName, grid.OriginalCaption, grid.NewCaption, grid.Visible, grid.ArabicCaption, grid.Id);
                        }
                    }
                    _logger.LogInformation("Grid updated Successfully");
                    return CommonResponse.Ok("Updated Successfully");
                }
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
    }
}

