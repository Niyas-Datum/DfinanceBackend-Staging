using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Dfinance.Sales.Service
{
    public class SalesB2BService:ISalesB2BService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<SalesInvoiceService> _logger;
        private readonly IPurchaseWithoutTaxService _obj;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryTransactionService _transactionService;
        public SalesB2BService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment, IInventoryTransactionService transactionService,
            ILogger<SalesInvoiceService> logger, IInventoryItemService inventoryItemService, IPurchaseWithoutTaxService obj)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
            _logger = logger;
            _itemService = inventoryItemService;
            _obj = obj;
            _transactionService = transactionService;
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
        public CommonResponse SaveSalesB2B(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Sales(B2B)");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int TransId = (int)_obj.SaveTransactions(dto, pageId, voucherId).Data;
                    //int TransId = 0;
                    int? transpayId = 0;
                    if (dto.TransactionEntries.Cash.Count > 0 || dto.TransactionEntries.Cheque.Count > 0 || dto.TransactionEntries.Card.Count > 0)
                    {
                        transpayId = (int)_obj.SaveTransactionPayment(dto, TransId, 7).Data;
                    }
                    if (dto.TransactionAdditional != null)
                    {
                        _obj.SaveTransactionAdditional(dto.TransactionAdditional, TransId, voucherId);
                    }
                    if (dto.References.Count > 0 && dto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = dto.References.Select(x => x.Id).ToList();

                        _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (dto.Items != null)
                    {
                        _itemService.SaveInvTransItems(dto.Items, voucherId, TransId, dto.ExchangeRate, dto.TransactionAdditional.Warehouse.Id);
                    }
                    if (dto.TransactionEntries != null)
                    {
                        int TransEntId = (int)_obj.SaveTransactionEntries(dto, pageId, TransId, transpayId ?? 0).Data;

                        if (dto.TransactionEntries.Advance != null && dto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0) || transpayId != TransId)
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId ?? 0, dto.TransactionEntries);
                        }
                    }
                    if (dto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Sales(B2B) Saved Successfully");
                    return CommonResponse.Created("Sales(B2B) Saved Successfully");
                }
                catch
                {
                    transactionScope.Dispose();
                    _logger.LogError("Failed to Save Sales(B2B)");
                    transactionScope.Dispose();
                    return CommonResponse.Error("Failed to Save Sales(B2B)");
                }
            }
        }

        public CommonResponse UpdateSalesB2B(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Update Sales(B2B)");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int TransId = (int)_obj.SaveTransactions(dto, pageId, voucherId).Data;
                    //int TransId = 0;
                    int? transpayId = 0;
                    if (dto.TransactionEntries.Cash.Count > 0 || dto.TransactionEntries.Cheque.Count > 0 || dto.TransactionEntries.Card.Count > 0)
                    {
                        transpayId = (int)_obj.SaveTransactionPayment(dto, TransId, 2).Data;
                    }
                    if (dto.TransactionAdditional != null)
                    {
                        _obj.SaveTransactionAdditional(dto.TransactionAdditional, TransId, voucherId);
                    }
                    if (dto.References.Count > 0 && dto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = dto.References.Select(x => x.Id).ToList();

                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (dto.Items != null)
                    {
                        _itemService.UpdateInvTransItems(dto.Items, voucherId, TransId, dto.ExchangeRate, dto.TransactionAdditional.Warehouse.Id);
                    }
                    if (dto.TransactionEntries != null)
                    {
                        int TransEntId = (int)_obj.SaveTransactionEntries(dto, pageId, TransId, transpayId ?? 0).Data;

                        if (dto.TransactionEntries.Advance != null && dto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0) || transpayId != TransId)
                        {
                            _transactionService.UpdateVoucherAllocation(TransId, transpayId ?? 0, dto.TransactionEntries);
                        }
                    }
                    if (dto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Sales(B2B) Updated Successfully");
                    return CommonResponse.Created("Sales(B2B) Updated Successfully");
                }
                catch
                {
                    transactionScope.Dispose();
                    _logger.LogError("Failed to Update Sales(B2B)");
                    transactionScope.Dispose();
                    return CommonResponse.Error("Failed to Update Sales(B2B)");
                }
            }
        }
    }
}
