using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Extensions.Logging;

namespace Dfinance.Purchase.Services
{
    public class GoodsInTransitService:IGoodsInTransitService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        
        private readonly IUserTrackService _userTrack;
        public GoodsInTransitService(DFCoreContext context, IAuthService authService, ILogger<PurchaseService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
             IInventoryItemService inventoryItemService, IInventoryPaymentService inventoryPaymentService, IUserTrackService userTrack)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = inventoryAdditional;
            _itemService = inventoryItemService;
            _paymentService = inventoryPaymentService;
           
            _userTrack = userTrack;
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
        //saving GoodsInTransit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryTransactionDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>      
        /// <returns></returns>
        public CommonResponse SaveGoodsInTransit(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 2))
            {
                return PermissionDenied("Save GoodsInTransit");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string Status = "Approved";
                    string? tranType = null;
                    //save FiTransaction
                    int TransId = (int)_transactionService.SaveTransaction(invTranseDto, PageId, voucherId, Status).Data;
                    //save FiTransactionAdditionals
                    _additionalService.SaveTransactionAdditional(invTranseDto.FiTransactionAdditional, TransId, voucherId);
                    //save InvTransItems
                    if (invTranseDto.Items != null && invTranseDto.Items.Count > 0)
                    {

                        _itemService.SaveInvTransItems(invTranseDto.Items, voucherId, TransId, invTranseDto.ExchangeRate, invTranseDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    //save TransactionExpense
                    if (invTranseDto.TransactionEntries != null)
                    {
                        if (invTranseDto.TransactionEntries.Tax.Count > 0)
                        {
                            tranType = "Tax";
                            _paymentService.SaveTransactionExpenses(invTranseDto.TransactionEntries.Tax, TransId, tranType);
                        }
                        if (invTranseDto.TransactionEntries.AddCharges != null && invTranseDto.TransactionEntries.AddCharges.Count > 0)
                        {
                            tranType = "Expense";
                            _paymentService.SaveTransactionExpenses(invTranseDto.TransactionEntries.AddCharges, TransId, tranType);
                        }
                    }
                    transaction.Commit();
                    //var jsonData = JsonSerializer.Serialize(invTranseDto);
                    //_userTrack.AddUserActivity(invTranseDto.VoucherNo, TransId, 0, "Added", "FiTransactions", "Purchase Order", 0, jsonData);
                    _logger.LogInformation("GoodsInTransit Saved successfully");
                    return CommonResponse.Ok("GoodsInTransit Saved successfully!");
                }

                catch (Exception ex)
                {
                    _logger.LogError("Failed GoodsInTransit Save");
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        //update GoodsInTransit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryTransactionDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>      
        /// <returns></returns>
        public CommonResponse UpdateGoodsInTransit(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 3))
            {
                return PermissionDenied("Update GoodsInTransit");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string Status = "Approved";
                    string? tranType = null;
                    int TransId = (int)_transactionService.SaveTransaction(invTranseDto, PageId, voucherId, Status).Data;
                    _additionalService.SaveTransactionAdditional(invTranseDto.FiTransactionAdditional, TransId, voucherId);
                    if (invTranseDto.Items != null && invTranseDto.Items.Count > 0)
                    {
                        _itemService.UpdateInvTransItems(invTranseDto.Items, voucherId, TransId, invTranseDto.ExchangeRate, invTranseDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (invTranseDto.TransactionEntries != null)
                    {
                        if (invTranseDto.TransactionEntries.Tax.Count > 0)
                        {
                            tranType = "Tax";

                            _paymentService.UpdateTransactionExpenses(invTranseDto.TransactionEntries.Tax, TransId, tranType);
                        }
                        if (invTranseDto.TransactionEntries.AddCharges != null && invTranseDto.TransactionEntries.AddCharges.Count > 0)
                        {
                            tranType = "Expense";
                            _paymentService.UpdateTransactionExpenses(invTranseDto.TransactionEntries.AddCharges, TransId, tranType);
                        }
                    }
                    _logger.LogInformation("GoodsInTransit Updated Successfully");
                    transaction.Commit();
                   // var jsonData = JsonSerializer.Serialize(invTranseDto);
                   // _userTrack.AddUserActivity(invTranseDto.VoucherNo, TransId, 1, "Updated", "FiTransactions", "GoodsInTransit", 0, jsonData);
                    return CommonResponse.Ok("GoodsInTransit Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError("GoodsInTransit Updation Failed");
                    transaction.Rollback();
                    return CommonResponse.Error("GoodsInTransit Updation Failed");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="TransId"></param>      
        /// <returns></returns>
        public CommonResponse DeleteGoodsInTransit(int TransId, int PageId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 5))
            {
                return PermissionDenied("Delete GoodsInTransit");
            }
            try
            {
                var result = _transactionService.DeleteTransactions(TransId);
                _logger.LogInformation("Deletion of GoodsInTransit Failed");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Deletion of GoodsInTransit Failed");
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse CancelGoodsInTransit(int TransId, int PageId,string reason)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 5))
            {
                return PermissionDenied("Cancel GoodsInTransit");
            }
            try
            {
                var result = _transactionService.CancelTransaction(TransId,reason);
                _logger.LogInformation("Cancel of GoodsInTransit Failed");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cancel of GoodsInTransit Failed");
                return CommonResponse.Error(ex);
            }
        }
    }
}
