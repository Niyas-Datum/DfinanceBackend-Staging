using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Extensions.Logging;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
namespace Dfinance.Purchase.Services
{
    public class PurchaseEnquiryService : IPurchaseEnquiryService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseEnquiryService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        public PurchaseEnquiryService(DFCoreContext context, IAuthService authService, ILogger<PurchaseEnquiryService> logger, IInventoryTransactionService transactionService
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

        public CommonResponse SavePurchaseEnquiry(InventoryTransactionDto purchaseEnqDto, int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {
                    string Status = "Approved";
                    string? tranType = null;
                    int TransId = (int)_transactionService.SaveTransaction(purchaseEnqDto, PageId, voucherId, Status).Data;
                    _additionalService.SaveTransactionAdditional(purchaseEnqDto.FiTransactionAdditional, TransId, voucherId);

                    if (purchaseEnqDto.Items != null && purchaseEnqDto.Items.Count > 0)
                    {
                        _itemService.SaveInvTransItems(purchaseEnqDto.Items, voucherId, TransId, purchaseEnqDto.ExchangeRate, purchaseEnqDto.FiTransactionAdditional.Warehouse.Id);
                    }

                    if (purchaseEnqDto.TransactionEntries != null)
                    {
                        if (purchaseEnqDto.TransactionEntries.Tax.Count > 0)
                        {
                            tranType = "Tax";
                            _paymentService.SaveTransactionExpenses(purchaseEnqDto.TransactionEntries.Tax, TransId, tranType);
                        }
                        if (purchaseEnqDto.TransactionEntries.AddCharges != null && purchaseEnqDto.TransactionEntries.AddCharges.Count > 0)
                        {
                            tranType = "Expense";
                            _paymentService.SaveTransactionExpenses(purchaseEnqDto.TransactionEntries.AddCharges, TransId, tranType);
                        }

                    }
                    _logger.LogInformation("Purchase enquiry inserted successfully");
                    transaction.Commit();
                    return CommonResponse.Ok("Inserted Successfully!");
                }
                
                catch (Exception ex) 
                { 
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }

            }
        }

        public CommonResponse UpdatePurchaseEnquiry(InventoryTransactionDto purchaseEnqDto, int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {
                    string Status = "Approved";
                    string? tranType = null;
                    int TransId = (int)_transactionService.SaveTransaction(purchaseEnqDto, PageId, voucherId, Status).Data;
                    _additionalService.SaveTransactionAdditional(purchaseEnqDto.FiTransactionAdditional, TransId, voucherId);
                    
                    if (purchaseEnqDto.Items != null && purchaseEnqDto.Items.Count > 0)
                    {
                        _itemService.UpdateInvTransItems(purchaseEnqDto.Items, voucherId, TransId, purchaseEnqDto.ExchangeRate, purchaseEnqDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (purchaseEnqDto.TransactionEntries != null)
                    {
                        if (purchaseEnqDto.TransactionEntries.Tax.Count > 0)
                        {
                            tranType = "Tax";
                            _paymentService.UpdateTransactionExpenses(purchaseEnqDto.TransactionEntries.Tax, TransId, tranType);
                        }
                        if (purchaseEnqDto.TransactionEntries.AddCharges != null && purchaseEnqDto.TransactionEntries.AddCharges.Count > 0)
                        {
                            tranType = "Expense";
                            _paymentService.UpdateTransactionExpenses(purchaseEnqDto.TransactionEntries.AddCharges, TransId, tranType);
                        }

                    }
                    _logger.LogInformation("Purchase enquiry updated successfully");
                    transaction.Commit();
                    return CommonResponse.Ok("Updated Successfully!");
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse DeletePurchaseEnq(int TransId)
        {
            try
            {
                var result = _transactionService.DeletePurchase(TransId);
                _logger.LogInformation("PurchaseEnq deleted successfully");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

    }
}
