using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Transactions;

namespace Dfinance.Sales.Service
{
    public class SalesPosService : ISalesPosService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<SalesReturnService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        private readonly IWarehouseService _warehouseService;
        public SalesPosService(DFCoreContext context, IAuthService authService, IHostEnvironment environment, ILogger<SalesReturnService> logger,
            IInventoryTransactionService transactionService, IInventoryAdditional additionalService, IInventoryItemService itemService, 
            IInventoryPaymentService paymentService,IWarehouseService warehouseService)
        {
            _context = context;
            _authService = authService;
            _environment = environment;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = additionalService;
            _itemService = itemService;
            _paymentService = paymentService;
            _warehouseService = warehouseService;
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
        public CommonResponse SaveSalesPos(InventoryTransactionDto salesPosDto, int PageId, int voucherId)
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
                        return PermissionDenied("Save SalesPos");
                    }
                    //int VoucherId=_com.GetVoucherId(PageId);
                    string Status = "Approved";
                    var transaction = _transactionService.SaveTransaction(salesPosDto, PageId, voucherId, Status).Data ;
                    int TransId = 0;
                    var transType = transaction.GetType();
                    var branchID = _authService.GetBranchId().Value;
                    dynamic location = _warehouseService.WarehouseDropdownUsingBranch(branchID).Data;

                    if (location != null)
                    {                        
                        var defaultLocation =(List<DropDownViewIsdeft>) location;   
                        if (defaultLocation.Any(loc=>loc.IsDefault ==true))
                             salesPosDto.FiTransactionAdditional.Warehouse.Id = Convert.ToInt32(defaultLocation.Where(loc=>loc.IsDefault==true).Select(loc=>loc.ID).FirstOrDefault());
                        
                    }

                    if (transType.Name == "String")
                    {
                        return CommonResponse.Ok(transaction);
                    }
                    else
                    {
                        TransId = (int)transaction;
                    }
                    int transpayId = TransId;
                    if (salesPosDto.TransactionEntries.Cash.Count > 0 || salesPosDto.TransactionEntries.Card.Count > 0 || salesPosDto.TransactionEntries.Cheque.Count > 0)
                    {
                        transpayId = (int)_transactionService.SaveTransactionPayment(salesPosDto, TransId, Status, 7).Data;
                    }
                    if (salesPosDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(salesPosDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (salesPosDto.References.Count > 0 && salesPosDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = salesPosDto.References.Select(x => x.Id).ToList();

                        _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (salesPosDto.Items != null)
                    {
                        _itemService.SaveInvTransItems(salesPosDto.Items, voucherId, TransId, salesPosDto.ExchangeRate, salesPosDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesPosDto.TransactionEntries != null)
                    {                        

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesPosDto, PageId, TransId, transpayId).Data;

                        if (salesPosDto.TransactionEntries.Advance != null && salesPosDto.TransactionEntries.Advance.Any() || transpayId!=TransId)
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId, salesPosDto.TransactionEntries);
                        }

                    }
                    if (salesPosDto != null)
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

        public CommonResponse UpdateSalesPos(InventoryTransactionDto salesPosDto, int PageId, int voucherId)
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
                        return PermissionDenied("Update SalesPos");
                    }
                    //int VoucherId=_com.GetVoucherId(PageId);
                    string Status = "Approved";
                    var transaction = _transactionService.SaveTransaction(salesPosDto, PageId, voucherId, Status).Data;
                    int TransId = 0;
                    var transType = transaction.GetType();
                    var branchID = _authService.GetBranchId().Value;
                    dynamic location = _warehouseService.WarehouseDropdownUsingBranch(branchID).Data;

                    if (location != null)
                    {
                        var defaultLocation = (List<DropDownViewIsdeft>)location;
                        if (defaultLocation.Any(loc => loc.IsDefault == true))
                            salesPosDto.FiTransactionAdditional.Warehouse.Id = Convert.ToInt32(defaultLocation.Where(loc => loc.IsDefault == true).Select(loc => loc.ID).FirstOrDefault());

                    }
                    if (transType.Name == "String")
                    {
                        return CommonResponse.Ok(transaction);
                    }
                    else
                    {
                        TransId = (int)transaction;
                    }
                    int transpayId = TransId;
                    if (salesPosDto.TransactionEntries.Cash.Count > 0 || salesPosDto.TransactionEntries.Card.Count > 0 || salesPosDto.TransactionEntries.Cheque.Count > 0)
                    {
                        transpayId = (int)_transactionService.SaveTransactionPayment(salesPosDto, TransId, Status, 7).Data;
                    }
                    if (salesPosDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(salesPosDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (salesPosDto.References.Count > 0 && salesPosDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = salesPosDto.References.Select(x => x.Id).ToList();

                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (salesPosDto.Items != null)
                    {
                        _itemService.UpdateInvTransItems(salesPosDto.Items, voucherId, TransId, salesPosDto.ExchangeRate, salesPosDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesPosDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesPosDto, PageId, TransId, transpayId).Data;

                        if (salesPosDto.TransactionEntries.Advance != null && salesPosDto.TransactionEntries.Advance.Any() || transpayId != TransId)
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId, salesPosDto.TransactionEntries);
                        }

                    }
                    if (salesPosDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    _logger.LogInformation("Successfully Updated");
                    transactionScope.Complete();
                    return CommonResponse.Created("Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
    }
}
