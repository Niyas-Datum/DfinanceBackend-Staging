using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace Dfinance.Purchase.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseOrderService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        private readonly DataRederToObj _rederToObj;
        private readonly IItemMasterService _item;
        private readonly IWarehouseService _warehouse;
        private readonly ICostCentreService _costCentre;
        private readonly IUserTrackService _userTrack;
        public PurchaseOrderService(DFCoreContext context, IAuthService authService, ILogger<PurchaseOrderService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
             IInventoryItemService inventoryItemService, IInventoryPaymentService inventoryPaymentService, DataRederToObj rederToObj, IItemMasterService item,
             IWarehouseService warehouse, ICostCentreService costCentre, IUserTrackService userTrack)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = inventoryAdditional;
            _itemService = inventoryItemService;
            _paymentService = inventoryPaymentService;
            _rederToObj = rederToObj;
            _item = item;
            _warehouse = warehouse;
            _costCentre = costCentre;
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
        //saving PurchaseOrder
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryTransactionDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>      
        /// <returns></returns>
        public CommonResponse SavePurchaseOrder(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 2))
            {
                return PermissionDenied("Save Purchase Order");
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
                    var jsonData = JsonSerializer.Serialize(invTranseDto);
                    _userTrack.AddUserActivity(invTranseDto.VoucherNo, TransId, 0, "Added", "FiTransactions","Purchase Order", 0,jsonData);
                    _logger.LogInformation("Purchase order Saved successfully");
                    return CommonResponse.Ok("Purchase order Saved successfully!");
                }

                catch (Exception ex)
                {
                    _logger.LogError("Failed Purchase Order Save");
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        //update Purchase  order
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryTransactionDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>      
        /// <returns></returns>
        public CommonResponse UpdatePurchaseOrder(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 3))
            {
                return PermissionDenied("Update Purchase Order");
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
                    _logger.LogInformation("Purchase Order Updated Successfully");
                    transaction.Commit();
                    var jsonData = JsonSerializer.Serialize(invTranseDto);
                    _userTrack.AddUserActivity(invTranseDto.VoucherNo, TransId, 1, "Updated", "FiTransactions", "Purchase Order", 0, jsonData);
                    return CommonResponse.Ok("Purchase Order Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Purchase Order Updation Failed");
                    transaction.Rollback();
                    return CommonResponse.Error("Purchase Order Updation Failed");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="TransId"></param>      
        /// <returns></returns>
        public CommonResponse DeletePurchaseOrder(int TransId,int PageId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 5))
            {
                return PermissionDenied("Delete Purchase Order");
            }
            try
            {
                var result = _transactionService.DeletePurchase(TransId);
                _logger.LogInformation("Deletion of Purchase Order Failed");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Deletion of Purchase Order Failed");
                return CommonResponse.Error(ex);
            }
        }
    }
}
        

