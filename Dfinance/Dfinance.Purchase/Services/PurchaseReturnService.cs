using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Dfinance.Purchase.Services
{
    public class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseReturnService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;

        public PurchaseReturnService(DFCoreContext context, IAuthService authService, ILogger<PurchaseReturnService> logger, IInventoryTransactionService transactionService
            , IInventoryAdditional additionalService, IInventoryItemService itemService, IInventoryPaymentService paymentService)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = additionalService;
            _itemService = itemService;
            _paymentService = paymentService;
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
        public CommonResponse SavePurchaseRtn(InventoryTransactionDto purchaseRtnDto, int PageId, int voucherId)
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
                        return PermissionDenied("Save PurchaseReturn");
                    }
                    string Status = "Approved";
                    string? tranType = null;

                    int TransId = (int)_transactionService.SaveTransaction(purchaseRtnDto, PageId, voucherId, Status).Data;
                    int transpayId=TransId;
                    if (purchaseRtnDto.TransactionEntries.Cash.Any(c => c.AccountCode.ID > 0) ||
                        purchaseRtnDto.TransactionEntries.Card.Any(c => c.AccountCode.ID > 0) ||
                        purchaseRtnDto.TransactionEntries.Cheque.Any(c => c.PDCPayable.ID > 0))
                    {
                        transpayId = (int)_transactionService.SaveTransactionPayment(purchaseRtnDto, TransId, Status, 7).Data;
                    }
                    if (purchaseRtnDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(purchaseRtnDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (purchaseRtnDto.Reference.Count > 0 && purchaseRtnDto.Reference.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = purchaseRtnDto.Reference.Select(x => x.Id).ToList();
                        _transactionService.SaveTransReference(TransId, referIds);
                    }

                   if (purchaseRtnDto.Items != null && purchaseRtnDto.Items.Count > 0)
                    {
                        _itemService.SaveInvTransItems(purchaseRtnDto, voucherId, TransId);
                    }

                    if (purchaseRtnDto.TransactionEntries != null)
                    {
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(purchaseRtnDto, PageId, TransId, transpayId).Data;
                        _transactionService.SaveVoucherAllocation(TransId, transpayId, purchaseRtnDto.TransactionEntries);
                    }

                    _logger.LogInformation("Purchase Return inserted");
                    transactionScope.Complete();
                    return CommonResponse.Ok("inserted successfully");

                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex.Message);
                     transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse UpdatePurchaseRtn(InventoryTransactionDto purchaseRtnDto, int PageId, int voucherId)
        {
           
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 3))
                    {
                        return PermissionDenied("Update PurchaseReturn");
                    }
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    string Status = "Approved";
                    string? tranType = null;

                    int TransId = (int)_transactionService.SaveTransaction(purchaseRtnDto, PageId, voucherId, Status).Data;
                    int transpayId = TransId;
                    if (purchaseRtnDto.TransactionEntries.Cash.Any(c => c.AccountCode.ID > 0) ||
                         purchaseRtnDto.TransactionEntries.Card.Any(c => c.AccountCode.ID > 0) ||
                         purchaseRtnDto.TransactionEntries.Cheque.Any(c => c.PDCPayable.ID > 0))
                    {
                        transpayId = (int)_transactionService.SaveTransactionPayment(purchaseRtnDto, TransId, Status, 7).Data;
                    }
                    if (purchaseRtnDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(purchaseRtnDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (purchaseRtnDto.Reference.Count > 0 && purchaseRtnDto.Reference.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = purchaseRtnDto.Reference.Select(x => x.Id).ToList();
                        _transactionService.UpdateTransReference(TransId, referIds);
                    }

                    if (purchaseRtnDto.Items != null && purchaseRtnDto.Items.Count > 0)
                    {
                        _itemService.UpdateInvTransItems(purchaseRtnDto, voucherId, TransId);
                    }

                    if (purchaseRtnDto.TransactionEntries != null)
                    {
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(purchaseRtnDto, PageId, TransId, transpayId).Data;
                            _transactionService.UpdateVoucherAllocation(TransId, transpayId, purchaseRtnDto.TransactionEntries);
                    }

                    transactionScope.Complete();
                    _logger.LogInformation("Purchase Return Updated");
                    return CommonResponse.Ok("updated successfully");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                     transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse DeletePurchaseRtn(int TransId, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Delete purchase Return");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");


                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, TransId);

                _logger.LogInformation("Delete successfully");
                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }


    }
}
