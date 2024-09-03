using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Dfinance.Application.Services.Inventory
{
    public class TaxTypeService : ITaxTypeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<ITaxTypeService> _logger;

        public TaxTypeService(DFCoreContext context, IAuthService authService, ILogger<ITaxTypeService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Saves or updates a TaxType.
        /// </summary>
        /// <param name="taxTypeDto">The DTO containing TaxType data.</param>
        /// <returns>A response indicating the outcome.</returns>
        public CommonResponse SaveTaxType(TaxTypeDto taxTypeDto)
        {
            try
            {
                string TaxAccount = null;
                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;

                if (taxTypeDto.Id == 0)
                {
                    return InsertTaxType(taxTypeDto, TaxAccount);
                }
                else
                {
                    return UpdateTaxType(taxTypeDto, TaxAccount);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("An error occurred while saving the TaxType: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserts a new TaxType.
        /// </summary>
        /// <param name="taxTypeDto">The DTO containing TaxType data.</param>
        /// <param name="TaxAccount">The tax account string.</param>
        /// <returns>A response indicating the outcome.</returns>
        private CommonResponse InsertTaxType(TaxTypeDto taxTypeDto, string TaxAccount)
        {
            string criteria = "InsertMaTaxType";
            SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC MaTaxSP @Criteria={0}, @Name={1}, @Type={2}, @SalePurchaseModeID={3}, " +
                "@ReceivableAccountID={4}, @PayableAccountID={5}, @TaxAccountID={6}, @SalesPerc={7}, " +
                "@PurchasePerc={8}, @Active={9}, @TaxMiscID={10}, @Description={11}, " +
                "@SGSTReceivableAccountID={12}, @SGSTPayableAccountID={13}, @CGSTReceivableAccountID={14}, " +
                "@CGSTPayableAccountID={15}, @CessPerc={16}, @CessPayable={17}, @CessReceivable={18}, " +
                "@IsDefault={19}, @NewID={20} OUTPUT",
                criteria,
                taxTypeDto.Name,
                taxTypeDto.Type.Id,
                taxTypeDto.Sales_Pur_Mode.Id,
                taxTypeDto.ReceivableAccount.Id,
                taxTypeDto.PayableAccount.Id,
                TaxAccount,
                taxTypeDto.SalesPerc,
                taxTypeDto.PurchasePerc,
                taxTypeDto.Active,
                taxTypeDto.TaxType.Id,
                taxTypeDto.Description,
                taxTypeDto.SGSTReceivable.Id,
                taxTypeDto.SGSTPayable.Id,
                taxTypeDto.CGSTReceivable.Id,
                taxTypeDto.CGSTPayable.Id,
                taxTypeDto.Cess_Perc,
                taxTypeDto.CessPayable.Id,
                taxTypeDto.CessReceivable.Id,
                taxTypeDto.Default,
                newIdParameter);

            return CommonResponse.Created($"TaxType {taxTypeDto.Name} created successfully with ID {newIdParameter.Value}");
        }

        /// <summary>
        /// Updates an existing TaxType.
        /// </summary>
        /// <param name="taxTypeDto">The DTO containing TaxType data.</param>
        /// <param name="TaxAccount">The tax account string.</param>
        /// <returns>A response indicating the outcome.</returns>
        private CommonResponse UpdateTaxType(TaxTypeDto taxTypeDto, string TaxAccount)
        {
            string criteria = "UpdateMaTaxType";

            _context.Database.ExecuteSqlRaw(
                "EXEC MaTaxSP @Criteria={0}, @Name={1}, @Type={2}, @SalePurchaseModeID={3}, " +
                "@ReceivableAccountID={4}, @PayableAccountID={5}, @TaxAccountID={6}, @SalesPerc={7}, " +
                "@PurchasePerc={8}, @Active={9}, @TaxMiscID={10}, @Description={11}, " +
                "@SGSTReceivableAccountID={12}, @SGSTPayableAccountID={13}, @CGSTReceivableAccountID={14}, " +
                "@CGSTPayableAccountID={15}, @CessPerc={16}, @CessPayable={17}, @CessReceivable={18}, " +
                "@IsDefault={19}, @ID={20}",
                criteria,
                taxTypeDto.Name,
                taxTypeDto.Type.Id,
                taxTypeDto.Sales_Pur_Mode.Id,
                taxTypeDto.ReceivableAccount.Id,
                taxTypeDto.PayableAccount.Id,
                TaxAccount,
                taxTypeDto.SalesPerc,
                taxTypeDto.PurchasePerc,
                taxTypeDto.Active,
                taxTypeDto.TaxType.Id,
                taxTypeDto.Description,
                taxTypeDto.SGSTReceivable.Id,
                taxTypeDto.SGSTPayable.Id,
                taxTypeDto.CGSTReceivable.Id,
                taxTypeDto.CGSTPayable.Id,
                taxTypeDto.Cess_Perc,
                taxTypeDto.CessPayable.Id,
                taxTypeDto.CessReceivable.Id,
                taxTypeDto.Default,
                taxTypeDto.Id);

            return CommonResponse.Ok($"TaxType {taxTypeDto.Name} updated successfully");
        }

        /// <summary>
        /// DeleteTaxType
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CommonResponse DeleteTaxType(int Id)
        {
            try
            {
                string msg = null;
                var name = _context.TaxType.Where(i => i.Id == Id).
                   Select(i => i.Name).
                   SingleOrDefault();
                if (name == null)
                {
                    msg = "This Category is Not Found";
                }
                else
                {
                    string Criteria = "DeleteMaTaxType";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC MaTaxSP @Criteria='{Criteria}',@ID='{Id}'");
                    msg = name + " Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        private CommonResponse FillDropDownValues(string StrParam)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"Exec DropDownListSP @Criteria='FillMaMisc',@StrParam='{StrParam}',@IntParam=null";
            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
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
        private CommonResponse AccountsByList(string Category)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            var category = Category != null ? Category.ToString() : "NULL";
            var branchId=_authService.GetBranchId().Value;
            cmd.CommandText = $"Exec AccountsListSP @Criteria='AccountsByList',@Category='{category}',@BranchID={branchId}";
            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
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
        public CommonResponse GetLoadData()
        {
            try
            {
                var salePurchaseMode = FillDropDownValues("SalePurchaseMode").Data;
                var taxType = FillDropDownValues("TaxTypes").Data;
                var receivableAccount = AccountsByList("VAT RECEIVABLE ACCOUNTS").Data;
                var payableAccount = AccountsByList("VAT PAYABLE ACCOUNTS").Data;
                var cGSTPayable = AccountsByList("GST Payable Accounts").Data;
                var cGSTReceivable = AccountsByList("GST Receivable Accounts").Data;
                var cessPayables = AccountsByList("CessPayable").Data;
                var cessReceivable = AccountsByList("CessReceivable").Data;
                return CommonResponse.Ok(new { SalePurchaseMode= salePurchaseMode, TaxTypes=taxType, ReceivableAccount= receivableAccount, PayableAccount = payableAccount,
                    CGSTPayable = cGSTPayable,SGSTPayable = cGSTPayable,CGSTReceivable= cGSTReceivable,SGSTReceivable = cGSTReceivable,
                    CessPayables= cessPayables,CessReceivable= cessReceivable
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Ok(ex.Message);
            }
        }
        public CommonResponse FillTaxTypeMaster()
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"Exec MaTaxSP @Criteria='FillTaxTypeMaster'";
            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
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
        public CommonResponse FillTaxTypeById(int Id)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"Exec MaTaxSP @Criteria='FillMaTaxType',@ID={Id}";
            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
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
    }
}
