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

namespace Dfinance.Item.Services
{
    public class InternationalBarCodeService : IInternationalBarCodeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<InternationalBarCodeService> _logger;
        public InternationalBarCodeService(DFCoreContext context, IAuthService authService, ILogger<InternationalBarCodeService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        /// <summary>
        /// Fill InternationalBarcode
        /// </summary>
        /// <returns></returns>
        public CommonResponse FillInternBarCode()
        {
            try
            {
                string criteria = "FillBarcode";
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec InvBarcodeMasterSP  @Criteria='{criteria}'";
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
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse SaveUpdateIntBarcCode(List<IntnBarCodeDto> intnBarCodeDto)
        {
            try
            {
                var criteria = "";
                foreach (var invBarCode in intnBarCodeDto)
                {
                    if (invBarCode.Id == 0 || intnBarCodeDto == null)
                    {
                        criteria = "InsertInvBarcodeMaster";
                        SqlParameter newIdItem = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var result = _context.Database.ExecuteSqlRaw("EXEC InvBarcodeMasterSP @Criteria={0},@Barcode={1},@Active={2},@NewID={3} OUTPUT", criteria, invBarCode.BarCode, invBarCode.Active, newIdItem);
                        var NewItemId = (int)newIdItem.Value;
                        _logger.LogInformation("InvBarCode.Inserted with ID: {Id}", NewItemId);
                        
                    }

                    else
                    {
                        var check = _context.InvBarcodeMasters.Any(x => x.Id == invBarCode.Id);
                        if (!check) { return CommonResponse.NotFound("Id not found"); }
                        criteria = "UpdateInvBarcodeMaster";
                        var result = _context.Database.ExecuteSqlRaw("EXEC InvBarcodeMasterSP @Criteria={0},@Barcode={1},@Active={2},@ID={3}", criteria, invBarCode.BarCode, invBarCode.Active, invBarCode.Id);
                        _logger.LogInformation("InvBarCode.Updated for ID: {Id}", invBarCode.Id);
                      
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
