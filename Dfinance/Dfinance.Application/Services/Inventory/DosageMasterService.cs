using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.Inventory
{
    public class DosageMasterService : IDosageMasterService
    {
        private ILogger<DosageMasterService> _logger;
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public DosageMasterService(ILogger<DosageMasterService> logger, DFCoreContext context, IAuthService authService)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
        }

        //FillMaster and FillByID
        public CommonResponse FillMasterAndById(int? Id)
        {
            try
            {
                string criteria = "";
                if (Id == null)
                {
                    //masterfill
                    criteria = "FillMaster";
                }
                else
                {
                    //FillById
                    var check = _context.InvDrugDosages.Any(x => x.Id == Id);
                    if (!check) { return CommonResponse.NotFound("Id not found"); }
                    criteria = "FillDosageByID";
                }
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var ID = Id?.ToString() ?? "NULL";
                string commandText = $"Exec DosageMasterSP @Criteria='{criteria}',@ID={ID}";
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

        //PageId=416
        //save
        public CommonResponse SaveUpdateDosage(DosageDto dosageDto,int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 3))
                {
                    return PermissionDenied("Update/Save Counters");
                }
                var criteria = "";
                if (dosageDto.Id == 0 || dosageDto.Id == null)
                {
                     criteria = "Insert";
                    SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var data = _context.Database.ExecuteSqlRaw("EXEC DosageMasterSP @Criteria ={0},@Dosage={1},@Remarks={2},@Active={3},@NewID={4} OUTPUT", criteria,
                                dosageDto.Dosage, dosageDto.Remarks, dosageDto.Active, newIdparam);
                    int NewIdUser = (int)newIdparam.Value;
                    _logger.LogInformation("Dosage.Inserted with ID: {Id}", NewIdUser);
                    return CommonResponse.Ok("Inserted successfully!");
                }
                else
                {
                    var check = _context.InvDrugDosages.Any(x => x.Id == dosageDto.Id);
                    if (!check) { return CommonResponse.NotFound("Id not found"); }
                    criteria = "Update";
                    var data = _context.Database.ExecuteSqlRaw("EXEC DosageMasterSP @Criteria ={0},@Dosage={1},@Remarks={2},@Active={3},@ID={4} ", criteria,
                                dosageDto.Dosage, dosageDto.Remarks, dosageDto.Active, dosageDto.Id);
                    _logger.LogInformation("Dosage.Updated for ID: {Id}", dosageDto.Id);
                    return CommonResponse.Ok("Updated successfully!");

                }

                //return CommonResponse.Ok("Processed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse DeleteDosage(int Id, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 5))
                {
                    return PermissionDenied("Delete Dosage");
                }
                var check = _context.InvDrugDosages.Any(x => x.Id == Id);
                if (!check) { return CommonResponse.NotFound("Id not found"); }
                string criteria = "Delete";
                var data = _context.Database.ExecuteSqlRaw("EXEC DosageMasterSP @Criteria ={0},@Id={1}", criteria, Id);
                _logger.LogInformation("Dosage deleted");
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
