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

        public CommonResponse DeleteSales(int TransId,int pageId)
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
        public CommonResponse CancelSales(int TransId,int pageId,string reason)
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
                    var transaction =_transactionService.SaveTransaction(salesDto, PageId, voucherId, Status).Data;
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
                        transpayId = (int)_transactionService.SaveTransactionPayment(salesDto, TransId, Status, 2).Data;
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
                        _itemService.SaveInvTransItems(salesDto.Items, voucherId, TransId,salesDto.ExchangeRate,salesDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesDto, PageId, TransId, transpayId).Data;                       

                        if (salesDto.TransactionEntries.Advance != null && salesDto.TransactionEntries.Advance.Any())
                        {                            
                            _transactionService.SaveVoucherAllocation(TransId,transpayId, salesDto.TransactionEntries);                            
                        }
                        
                    }
                    if(salesDto!=null)
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

                        if (salesDto.TransactionEntries.Advance != null && salesDto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0))
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
       
       
    }
}
