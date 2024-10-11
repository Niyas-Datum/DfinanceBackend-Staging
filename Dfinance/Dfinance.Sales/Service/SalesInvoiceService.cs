using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Shared.Deserialize;
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Dfinance.Shared.Routes.InvRoute;
using System;
using Dfinance.Shared.Domain;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using System.Transactions;
using System.Text;
using Microsoft.Identity.Client;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Dfinance.DataModels.Dto.Finance;

using static Dfinance.Shared.Routes.v1.ApiRoutes;
using Dfinance.AuthCore.Domain;
using Dfinance.Core.Domain;
using Microsoft.CodeAnalysis.Semantics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Dfinance.Sales
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<SalesInvoiceService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        private readonly DataRederToObj _rederToObj;
        private readonly IItemMasterService _item;
        private readonly IWarehouseService _warehouse;
        private readonly ICustomerSupplierService _party;
        private readonly ICostCentreService _costCentre;
        private readonly CommonService _com;
        private readonly ISettingsService _settings;
        public SalesInvoiceService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment,
            ILogger<SalesInvoiceService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
            IInventoryItemService inventoryItemService, IInventoryPaymentService inventoryPaymentService, DataRederToObj rederToObj, IItemMasterService item,
            IWarehouseService warehouse, ICostCentreService costCentre, ICustomerSupplierService party, CommonService com, ISettingsService settings)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = inventoryAdditional;
            _itemService = inventoryItemService;
            _paymentService = inventoryPaymentService;

            _rederToObj = rederToObj;
            _item = item;
            _warehouse = warehouse;
            _costCentre = costCentre;
            _party = party;
            _com = com;
            _settings = settings;
        }

        //fills the batchno popup in item grid
        public CommonResponse BatchNoPopup(int locId,int itemId)
        {
            string criteria = "BatchwiseStock";
            int branchId = _authService.GetBranchId().Value;
            int transId = 1;
            var primaryVoucherID =  "NULL";
            var partyId = "null";
            var voucherId = "null";
            var ItemID = itemId != null ? itemId.ToString() : "NULL";
            var modeId = "null";
            var pageId = "null";
            DateTime? voucherDate =null;
            int userId=_authService.GetId().Value;
            var branchID = branchId != null ? branchId.ToString() : "NULL";
            var TransID = transId != null ? transId.ToString() : "NULL";

            var result = _context.CommandTextView
                .FromSqlRaw($"select dbo.GetCommandText('{criteria}',{primaryVoucherID},{branchID},{partyId},{locId},'True','False',{voucherId},{ItemID},'False',null,null,{modeId},{pageId},'{voucherDate}',{TransID},{userId})")
                .ToList();

            var res = result.FirstOrDefault();
            var data = _context.BatchNoPopupView.FromSqlRaw(res.commandText).ToList();
            return CommonResponse.Ok(data);

        }

        //for getting the default customer
        public CommonResponse GetDefaultCustomer()
        {
            var result = from ua in _context.FimaUniqueAccount
                         join a in _context.FiMaAccounts on ua.AccId equals a.Id
                         where ua.Keyword == "CASH CUSTOMER"
                         select new
                         {
                             AccountID = ua.AccId,
                             AccountName = a.Name
                         };
            var resultList = result.ToList();
            return CommonResponse.Ok(resultList);
        }
        public CommonResponse DeleteSales(int TransId, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 2))
                {
                    return PermissionDenied("Save Sales");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                _transactionService.DeleteTransactions(TransId);
                _logger.LogInformation("Delete successfully");
                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse CancelSales(int TransId, int pageId, string reason)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 2))
                {
                    return PermissionDenied("Save Sales");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                _transactionService.CancelTransaction(TransId, reason);
                _logger.LogInformation("Cancel successfully");
                return CommonResponse.Ok("Cancel successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillSales(int PageId, bool? post)
        {
            try
            {
                int branchid = _authService.GetBranchId().Value;
                var data = _context.Fillvoucherview.FromSqlRaw($"Exec LeftGridMasterSP @Criteria='FillVoucher',@BranchID='{branchid}',@MaPageMenuID={PageId},@Posted={post}").ToList();
                _logger.LogInformation("Fill successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillSalesById(int TransId)
        {
            try
            {

                string criteria = "Fill";
                PurchaseFillByIdDto salesFillByIdDto = new PurchaseFillByIdDto();

                _context.Database.OpenConnection();
                //if (!_authService.IsPageValid(PageId))
                //{
                //    return PageNotValid(PageId);
                //}
                //if (!_authService.UserPermCheck(PageId, 2))
                //{
                //    return PermissionDenied("Save Sales");
                //}
                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec VoucherSP @Criteria='{criteria}',@TransactionID='{TransId}'";

                    using (var reader = dbCommand.ExecuteReader())
                    {
                        salesFillByIdDto.fillTransactions = _rederToObj.Deserialize<FillTransactions>(reader).FirstOrDefault();
                        reader.NextResult();
                        salesFillByIdDto.fillTransactionEntries = _rederToObj.Deserialize<FillTransactionEntries>(reader).ToList();
                        reader.NextResult();
                        salesFillByIdDto.fillVoucherAllocationUsingRef = _rederToObj.Deserialize<FillVoucherAllocationUsingRef>(reader).FirstOrDefault();
                        reader.NextResult();
                        salesFillByIdDto.fillCheques = _rederToObj.Deserialize<FillCheques>(reader).ToList();
                        reader.NextResult();
                        salesFillByIdDto.fillTransCollnAllocations = _rederToObj.Deserialize<FillTransCollnAllocations>(reader).FirstOrDefault();
                        reader.NextResult();
                        salesFillByIdDto.fillInvTransItems = _rederToObj.Deserialize<FillInvTransItems>(reader).ToList();
                        reader.NextResult();
                        salesFillByIdDto.fillInvTransItemDetails = _rederToObj.Deserialize<FillInvTransItemDetails>(reader).FirstOrDefault();
                        reader.NextResult();
                        salesFillByIdDto.fillTransactionItemExpenses = _rederToObj.Deserialize<FillTransactionItemExpenses>(reader).FirstOrDefault();
                        reader.NextResult();
                        //purchaseFillByIdDto.fillDocuments = _rederToObj.Deserialize<FillDocuments>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransactionExpenses = _rederToObj.Deserialize<FillTransactionExpenses>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillDocumentRequests = _rederToObj.Deserialize<FillDocumentRequests>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillDocumentReferences = _rederToObj.Deserialize<FillDocumentReferences>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransactionReferences = _rederToObj.Deserialize<FillTransactionReferences>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransLoadSchedules = _rederToObj.Deserialize<FillTransLoadSchedules>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransCollections = _rederToObj.Deserialize<FillTransCollections>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransEmployees = _rederToObj.Deserialize<FillTransEmployees>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.vMFuelLog = _rederToObj.Deserialize<VMFuelLog>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillDocumentImages = _rederToObj.Deserialize<FillDocumentImages>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillHRFinalSettlement = _rederToObj.Deserialize<FillHRFinalSettlement>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransCostAllocations = _rederToObj.Deserialize<FillTransCostAllocations>(reader).FirstOrDefault();
                        //reader.NextResult();
                    }
                }

                if (salesFillByIdDto.fillTransactions != null)
                {
                    return CommonResponse.Ok(salesFillByIdDto);
                }
                _logger.LogInformation("Successfully FillThatTransaction");
                return CommonResponse.NotFound("Sales not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId)
        {
            var res = _item.FillTransItems(partyId, PageID, locId, voucherId);
            _logger.LogInformation("Successfully FillThatTransItems");
            return CommonResponse.Ok(res);
        }

        public CommonResponse GetData(int pageId, int voucherId)
        {
            int branchId = _authService.GetBranchId().Value;
            var voucherNo1 = _transactionService.GetAutoVoucherNo(voucherId);
            var voucherNo = voucherNo1.Data;
            var costcentre1 = _costCentre.FillCostCentre();
            var costcentre = costcentre1.Data;
            var warehouse1 = _warehouse.WarehouseDropdownUsingBranch(branchId);
            var warehouse = warehouse1.Data;
            _logger.LogInformation("Successfully Get All DropDownFill");
            return CommonResponse.Ok(new { VNo = voucherNo, CostCentre = costcentre, WareHouse = warehouse });
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
        public CommonResponse SaveSales(InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 2))
                    {
                        return PermissionDenied("Save Sales");
                    }
                    //int VoucherId=_com.GetVoucherId(PageId);
                    string Status = "Approved";
                    var transaction = _transactionService.SaveTransaction(salesDto, PageId, voucherId, Status).Data;
                    int TransId = 0;
                    var transType = transaction.GetType();
                    if (transType.Name == "String")
                    {
                        return CommonResponse.Ok(transaction);
                    }
                    else
                    {
                        TransId = (int)transaction;
                    }
                    int transpayId = TransId;
                    if (salesDto.TransactionEntries.Cash.Count > 0 || salesDto.TransactionEntries.Card.Count > 0 || salesDto.TransactionEntries.Cheque.Count > 0)
                    {
                        transpayId = (int)_transactionService.SaveTransactionPayment(salesDto, TransId, Status, 7).Data;
                        if (transpayId == 0)
                            transpayId = TransId;
                    }
                    if (salesDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(salesDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (salesDto.References.Count > 0 && salesDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = salesDto.References.Select(x => x.Id).ToList();

                        _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (salesDto.Items != null)
                    {
                        _itemService.SaveInvTransItems(salesDto.Items, voucherId, TransId, salesDto.ExchangeRate, salesDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesDto, PageId, TransId, transpayId).Data;

                        if (salesDto.TransactionEntries.Advance != null && salesDto.TransactionEntries.Advance.Any() || transpayId!=TransId)
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId, salesDto.TransactionEntries);
                        }

                    }
                    if (salesDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    _logger.LogInformation("Successfully Created");
                    transactionScope.Complete();
                    return CommonResponse.Created("Created Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse UpdateSales(InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 3))
            {
                return PermissionDenied("Update Sales");
            }
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    //int VoucherId = _com.GetVoucherId(PageId);
                    string Status = "Approved";
                    int TransId = (int)_transactionService.SaveTransaction(salesDto, PageId, voucherId, Status).Data;

                    int transpayId = (int)_transactionService.SaveTransactionPayment(salesDto, TransId, Status, 7).Data;

                    if (salesDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(salesDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (salesDto.References.Count > 0 && salesDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = salesDto.References.Select(x => x.Id).ToList();

                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (salesDto.Items != null)

                    {
                        _itemService.UpdateInvTransItems(salesDto.Items, voucherId, TransId, salesDto.ExchangeRate, salesDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesDto, PageId, TransId, transpayId).Data;

                        if (salesDto.TransactionEntries.Advance != null && salesDto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0) || transpayId!=TransId)
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId, salesDto.TransactionEntries);
                        }

                    }
                    if (salesDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Successfully Updated");
                    return CommonResponse.Created("Update Successfully");
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transactionScope.Dispose();
                    return CommonResponse.Error();
                }
            }

        }
        /// <summary>
        /// GetMonthlySalesSummary
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CommonResponse GetMonthlySalesSummary(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;               
                int? branchid = _authService.GetBranchId().Value;
                var query = new StringBuilder();
                query.Append("Exec MonthlyInventorySummarySP ");


                var parameters = new List<string>
        {
            $"@BranchID = {branchid ?? 0}",
            $"@DateFrom = '{startDate:yyyy-MM-dd}'",
            $"@DateUpto = '{endDate:yyyy-MM-dd}'"
        };
                query.Append(string.Join(", ", parameters));

                cmd.CommandText=query.ToString();
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
                _logger.LogError(ex.Message);

                return CommonResponse.Error();
            }
        }

        /// <summary>
        /// GetFillSalesDaySummary
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branch"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public CommonResponse GetFillSalesDaySummary(string? criteria, DateTime? startDate, DateTime? endDate, int? branch, int? user)
        {
            //if (!_authService.IsPageValid(pageId))
            //{
            //    return PageNotValid(pageId);
            //}
            //if (!_authService.UserPermCheck(pageId, 1))
            //{
            //    return PermissionDenied("Fill the data ");
            //}
            try
            {

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                if(startDate==null)
                    startDate = DateTime.Now;
                if(endDate==null)
                    endDate = DateTime.Now;
                string formattedStartDate = startDate?.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate?.ToString("yyyy-MM-dd");
                var branchId = branch != null ? branch.ToString() : "NULL";
                var userId = user != null ? user.ToString() : "NULL";
                string commandText = $"Exec SalesDaySummarySP @StartDate='{formattedStartDate}', @EndDate='{formattedEndDate}', @BranchID={branchId}, @UserID={userId}";

                if (criteria != null)
                {
                    var criteriaParam = cmd.CreateParameter();
                    criteriaParam.ParameterName = "@Criteria";
                    criteriaParam.Value = criteria;
                    cmd.Parameters.Add(criteriaParam);
                    commandText += ", @Criteria=@Criteria";
                }

                cmd.CommandText = commandText;
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
        /// <summary>
        /// GetSalesPurchaseSummary
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branch"></param>
        /// <param name="user"></param>
        /// <returns></returns>     
       
        public CommonResponse GetSalesPurchaseSummary(DateTime startDate, DateTime endDate, int? branch, int? user)
        {
            try
            {               
                using (var connection = _context.Database.GetDbConnection())
                {                    
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                        string formattedEndDate = endDate.ToString("yyyy-MM-dd");
                        var branchId = branch.HasValue ? branch.Value.ToString() : "NULL";
                        var userId = user.HasValue ? user.Value.ToString() : "NULL";                       
                        string commandText = $"Exec SalesPurchaseSummary @DateFrom='{formattedStartDate}', @DateUpto='{formattedEndDate}', @BranchID={branchId}, @UserID={userId}";
                        cmd.CommandText = commandText;

                        using (var reader = cmd.ExecuteReader())
                        {                           
                            List<List<Dictionary<string, object>>> allTables = new List<List<Dictionary<string, object>>>();                           
                            do
                            {
                                if (reader.HasRows)
                                {                                    
                                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();                                  
                                    while (reader.Read())
                                    {
                                        var row = new Dictionary<string, object>();                                       
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            row.Add(reader.GetName(i).Replace(" ", ""), reader.GetValue(i)?.ToString().Trim());
                                        }
                                        rows.Add(row); 
                                    }
                                    allTables.Add(rows); 
                                }
                            } while (reader.NextResult()); 
                          
                            if (allTables.Count > 0)
                            {
                                return CommonResponse.Ok(allTables); 
                            }
                            return CommonResponse.NoContent(); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

        /// <summary>
        /// AreaWiseSales
        /// </summary>
        /// <param name="viewby"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="item"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        public CommonResponse AreaWiseSales(string? viewby, DateTime startdate, DateTime enddate, int? item, int? Area)
        {
            //if (!_authService.IsPageValid(pageId))
            //{
            //    return PageNotValid(pageId);
            //}
            //if (!_authService.UserPermCheck(pageId, 1))
            //{
            //    return PermissionDenied("Fill the data ");
            //}
            try
            {
                int branch = 1;

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                string formattedStartDate = startdate.ToString("yyyy-MM-dd");
                string formattedEndDate = enddate.ToString("yyyy-MM-dd");


                string commandText = $"Exec AreaWiseSalesSummarySP @Criteria='{viewby}', @DateFrom='{formattedStartDate}', @DateUpto='{formattedEndDate}', @BranchID={branch}";

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Criteria"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateUpto"></param>
        /// <param name="VoucherID"></param>
        /// <param name="BranchID"></param>
        /// <param name="Detailed"></param>
        /// <param name="AccountID"></param>
        /// <param name="VoucherNo"></param>
        /// <param name="SalesManID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public CommonResponse SalesReport(string? Criteria, DateTime DateFrom, DateTime DateUpto, int? VoucherID, bool? Detailed, int? AccountID, string? VoucherNo, int? SalesManID)

        {
            try
            {
                int branch = _authService.GetBranchId().Value;
               // int userid = 118;
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                string formattedStartDate = DateFrom.ToString("yyyy-MM-dd");
                string formattedEndDate = DateUpto.ToString("yyyy-MM-dd");
                var salesmanId =SalesManID != null ? SalesManID.ToString() : "NULL";
                var criteria =Criteria != null ? $"'{Criteria.ToString()}'" : "NULL";
                var accountid = AccountID != null ? AccountID.ToString() : "NULL";
                var voucherid = VoucherID != null ? VoucherID.ToString() : "NULL";
                var details = Detailed != null ? $"'{Detailed.ToString()}'"  : "NULL";
                string commandText = $"Exec SalesReportSP @Criteria={criteria}, @DateFrom='{formattedStartDate}', @DateUpto='{formattedEndDate}', @BranchID={branch},@VoucherID = {voucherid},@AccountID= {accountid},@Detailed = {details},@SalesManID={salesmanId}";

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


        //Userwise profit report
        public CommonResponse UserwiseProfit(DateTime startDate, DateTime endDate, int pageId, int? User, bool? detailed)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Userwise Profit");
            }
            string criteria = "UserwiseProfit";
            int branchId = _authService.GetBranchId().Value;
            string voucherId = "null";
            string account = "null";
            string userId = User.HasValue ? User.Value.ToString() : "NULL";
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            _context.Database.GetDbConnection().Open();
            cmd.CommandText = $"Exec SalesReportSP @Criteria='{criteria}',@DateFrom='{startDate}',@DateUpto='{endDate}',@BranchID={branchId},@VoucherID={voucherId},@AccountID={account},@Detailed='{detailed}',@SalesManID={userId}";
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
                return CommonResponse.NoContent("No Data");
            }
        }

        /// <summary>
        /// SalesCommission
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateUpto"></param>
        /// <param name="BranchID"></param>
        /// <param name="@AccountID"></param>
        /// <param name="@UserID"></param>
        /// <returns></returns>
        public CommonResponse SalesCommission(DateTime startdate, DateTime enddate, int? salesmanId, int? userId)
        {
            try
            {
                int? BranchId = _authService.GetBranchId();

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                string formattedStartDate = startdate.ToString("yyyy-MM-dd");
                string formattedEndDate = enddate.ToString("yyyy-MM-dd");

                var salesman = salesmanId?.ToString() ?? "NULL";
                var user = userId?.ToString() ?? "NULL";

                string commandText = $"Exec SalesCommissionSP @BranchID={BranchId}, @DateFrom='{formattedStartDate}', @DateUpto='{formattedEndDate}', @AccountID={salesman},@UserID={user}";

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

        //TopCustomers & Suppliers Report
        public CommonResponse TopCustomerSupplier(DateTime startdate, DateTime enddate, int? pageId)
        {
            try
            {
                int? BranchId = _authService.GetBranchId();

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;

                string formattedStartDate = startdate.ToString("yyyy-MM-dd");
                string formattedEndDate = enddate.ToString("yyyy-MM-dd");
                var page = pageId?.ToString() ?? "NULL";
                string commandText = $"Exec TopCustomerSupplierSP @BranchID={BranchId}, @DateFrom='{formattedStartDate}', @DateUpto='{formattedEndDate}',@PageID={page}";

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
        //**************************BuyerList*************************************
        private CommonResponse GetItems()
        {
            var items = _context.ItemMaster.Where(i => i.Active == true && i.IsGroup == false && i.StockItem == true).Select(i => new { i.ItemCode, i.ItemName, i.Id }).ToList();
            return CommonResponse.Ok(items);
        }
        private CommonResponse GetParties()
        {
            var parties = (from a in _context.FiMaAccounts
                           join b in _context.FiMaBranchAccounts on a.Id equals b.AccountId
                           join P in _context.Parties on a.Id equals P.AccountId
                           where a.IsGroup == false && b.BranchId == 1 && P.Nature == "S"
                           select new
                           {
                               AccountCode = a.Alias,
                               AccountName = a.Name,
                               a.Id,
                           });
            return CommonResponse.Ok(parties);
        }
        public CommonResponse BuyerLoadData()
        {
            try
            {
                var items = GetItems().Data;
                var parties = GetParties().Data;
                return CommonResponse.Ok(new { Items = items, Parties = parties });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse BuyerListReport(DateTime DateFrom,DateTime DateUpto,string Mode,int? PartyId,int? ItemId)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                string dateFrom = $"'{DateFrom.ToString("yyyy-MM-dd")}'";
                string dateUpto = $"'{DateUpto.ToString("yyyy-MM-dd")}'";
                string mode = Mode !=null? $"'{Mode.ToString()}'": "Preferred";
                var itemId =ItemId != null ? ItemId.ToString() : "NULL";
                var partyId =PartyId != null ? PartyId.ToString() : "NULL";
                var branchID = _authService.GetBranchId().Value;
                cmd.CommandText = $"Exec BuyerListSP @DateFrom={dateFrom},@DateUpto={dateUpto},@Mode={mode},@BranchID={branchID},@AccountID={partyId},@ItemID={itemId}";
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
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
               
    }
}




