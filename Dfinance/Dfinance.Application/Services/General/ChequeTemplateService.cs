using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class ChequeTemplateService : IChequeTemplateService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<ChequeTemplateService> _logger;
        public ChequeTemplateService(DFCoreContext context, IAuthService authService, ILogger<ChequeTemplateService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        public CommonResponse FillMaster()
        {
           
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec ChequeTemplateSP @Criteria='FillMaster'";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count>0)
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

        public CommonResponse FillChqTemplate(int Id)
        {

            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec ChequeTemplateSP @Criteria='FIllChequeTemplate',@ID='{Id}'";
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

        public CommonResponse FillChqTemplateField(int CheqTempId)
        {

            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec ChequeTemplateSP @Criteria='FillChequeTemplateFields',@ChequeTemplateID='{CheqTempId}'";
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

        public CommonResponse SaveChequeTemplate(CheqTempDto cheqTempDto)
        {
            try
            {
                string criteria = "InsertChequeTemplate";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var data = _context.Database.ExecuteSqlRaw("EXEC ChequeTemplateSP @Criteria ={0},@Code={1},@Name={2},@AccountID={3},@Width={4},@Height={5},@DateFormat={6},@DateSeperator={7},@NewID={8} OUTPUT",
                            criteria, cheqTempDto.Code, cheqTempDto.Name, cheqTempDto.Bank.ID, cheqTempDto.Width, cheqTempDto.Height, cheqTempDto.DateFormat, cheqTempDto.DateSeperator, newIdparam);
                int NewId = (int)newIdparam.Value;

                SaveCheqTempFields(NewId, cheqTempDto.CheqTempFields);

                _logger.LogInformation("Inserterd sucessfully");
                return CommonResponse.Ok("ChequeTemplate Inserted sucessfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        private void SaveCheqTempFields(int Id, List<CheqTempFields> cheqTempDtos)
        {
            string criteria = "InsertChequeTemplateFields";
            SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            foreach (var cheq in cheqTempDtos)
            {
                if (cheq.FieldId == "Account Payee") cheq.FieldId = "lblAccountPayee";
                else if(cheq.FieldId == "Party Name") cheq.FieldId = "lblPartyName";
                else if (cheq.FieldId == "Amount In Words1") cheq.FieldId = "lblAmountInWords1";
                else if (cheq.FieldId == "Amount In Words2") cheq.FieldId = "lblAmountInWords2";
                else if (cheq.FieldId == "Cheque Date") cheq.FieldId = "lblChequeDate";
                else if (cheq.FieldId == "Amount") cheq.FieldId = "lblAmount";

                _context.Database.ExecuteSqlRaw("EXEC ChequeTemplateSP @Criteria ={0},@ChequeTemplateID={1},@FieldID={2},@Width={3},@Height={4},@Left={5},@Top={6},@Font={7},@FontSize={8},@FontStyle={9},@Casing={10},@Visible={11},@NewID={12} OUTPUT",
                             criteria, Id,cheq.FieldId,cheq.Width,cheq.Height,cheq.Left,cheq.Top,cheq.Font,cheq.FontSize,cheq.FontStyle,cheq.Casing,cheq.Visible,newIdparam);

            }
        }

        public CommonResponse UpdateCheqTemp(CheqTempDto cheqTempDto)
        {
            try
            {
                string criteria = "UpdateChequeTemplate";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var data = _context.Database.ExecuteSqlRaw("EXEC ChequeTemplateSP @Criteria ={0},@Code={1},@Name={2},@AccountID={3},@Width={4},@Height={5},@DateFormat={6},@DateSeperator={7},@ID={8}",
                            criteria, cheqTempDto.Code, cheqTempDto.Name, cheqTempDto.Bank.ID, cheqTempDto.Width, cheqTempDto.Height, cheqTempDto.DateFormat, cheqTempDto.DateSeperator,cheqTempDto.Id);
                var cheqtempfields = _context.ChequeTemplateFields.Where(d => d.ChequeTemplateId == cheqTempDto.Id).ToList();
                _context.ChequeTemplateFields.RemoveRange(cheqtempfields);
                _context.SaveChanges();
                SaveCheqTempFields(cheqTempDto.Id, cheqTempDto.CheqTempFields);
                _logger.LogInformation("Updated sucessfully");
                return CommonResponse.Ok("ChequeTemplate Updated sucessfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse DeleteCheqTemp(int CheqTempId)
        {
            try
            {
                var CheqTemId = _context.ChequeTemplate.Where(i => i.Id == CheqTempId).Select(i => i.Id).SingleOrDefault();
                if (CheqTemId == 0)
                {
                    return CommonResponse.Error("ChequeTemplateId not found");
                }
                var cheqtempfields = _context.ChequeTemplateFields.Where(d => d.ChequeTemplateId == CheqTempId).ToList();
                _context.ChequeTemplateFields.RemoveRange(cheqtempfields);
                _context.SaveChanges();
                string criteria = "DeleteChequeTemplate";
                var result = _context.Database.ExecuteSqlRaw($"EXEC ChequeTemplateSP @Criteria='{criteria}', @ID='{CheqTempId}'");
                _logger.LogInformation("ChequeTemplate deleted");
                return CommonResponse.Ok("Deleted Sucessfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse BankPopup()
        {
            try
            {
                var accounts =   (from a in _context.FiMaAccounts where a.IsGroup == false
                                  select new
                                  { ID = a.Id, 
                                    AccountName = a.Name,
                                    AccountCode = a.Alias
                                  }).ToList();
                return CommonResponse.Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}
