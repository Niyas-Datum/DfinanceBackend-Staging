using Azure;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Dfinance.Application.Services.Finance
{
    public class BranchAccountsService:IBranchAccounts
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public BranchAccountsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
       //fill the accounts and branches
        public CommonResponse FillBranchAccounts()
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec AccountsSp @Criteria='GetBranchAccounts'";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tb = new DataTable();
                        tb.Load(reader);

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

        //for updating branch accounts
        public CommonResponse UpdateBranchAccounts(BranchAccountsDto branchAccountsDto)
        {
            try
            {

                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                var remove=_context.FiMaBranchAccounts.Where(i=>i.AccountId==branchAccountsDto.AccountID).ToList();
                _context.RemoveRange(remove);
                _context.SaveChanges();
                string criteria = "UpdateBranchAccounts";
                if (branchAccountsDto.BranchID != null)
                {
                    foreach (var b in branchAccountsDto.BranchID)
                    {
                        var result = _context.Database.ExecuteSqlRaw("Exec AccountsSp @Criteria={0},@ID={1},@BranchID={2},@IsBit={3} ", criteria, branchAccountsDto.AccountID, b, branchAccountsDto.IsBit);

                    }
                }
                
                return CommonResponse.Created("Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        
        }
}
