using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.Inventory
{
    public class QualityTypeService : IQualityTypeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<QualityTypeService> _logger;
        public QualityTypeService(DFCoreContext context, IAuthService authService, ILogger<QualityTypeService> logger)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
        }

        //QualityinCaret Popup
        public CommonResponse QualityinCaretPopup()
        {
            try
            {
                var result = _context.MaMisc.Where(m => m.Key == "Quality Type" && m.Active == true).Select(m => new
                {
                    ID = m.Id,
                    Value = m.Value,
                    Description = m.Description
                }).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }

        }

        public CommonResponse Fill()
        {
            try
            {
                string criteria = "FillGoldRate";
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                string commandText = $"Exec GoldPriceSP @Criteria='{criteria}'";
                cmd.CommandText = commandText;

                // Open the connection
                _context.Database.GetDbConnection().Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var tb = new DataTable();
                        tb.Load(reader);

                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        foreach (DataRow dr in tb.Rows)
                        {
                            var row = new Dictionary<string, object>();
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
            finally
            {
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    _context.Database.GetDbConnection().Close();
                }
            }
        }

        public CommonResponse SaveEditQualityType(List<QualityTypeDto> qualityTypeDto)
        {
            try
            {
                var criteria = "";
                foreach (var quality in qualityTypeDto)
                {
                    if (quality.Id == 0 || quality.Id == null)
                    {
                        criteria = "Insert";
                        SqlParameter newIdItem = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var result = _context.Database.ExecuteSqlRaw("EXEC GoldPriceSP @Criteria={0},@QualityID={1},@Rate={2},@NewID={3} OUTPUT", criteria, quality.QualityinCarat.Id, quality.Rate, newIdItem);
                        var NewItemId = (int)newIdItem.Value;
                        _logger.LogInformation("QualityType.Inserted with ID: {Id}", NewItemId);
                    }
                    else
                    {
                        var check = _context.InvQualityPrices.Any(x => x.Id == quality.Id);
                        if (!check) { return CommonResponse.NotFound("Id not found"); }
                        criteria = "Update";
                        var result = _context.Database.ExecuteSqlRaw("EXEC GoldPriceSP @Criteria={0},@QualityID={1},@Rate={2},@ID={3}", criteria, quality.QualityinCarat.Id, quality.Rate, quality.Id);
                        _logger.LogInformation("QualityType.Updated with ID: {Id}", quality.Id);
                    }

                }
                return CommonResponse.Ok("Processed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }


    }
}
