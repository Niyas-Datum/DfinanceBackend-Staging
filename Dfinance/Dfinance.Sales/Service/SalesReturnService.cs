﻿using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using System.Transactions;

namespace Dfinance.Sales
{

    public class SalesReturnService : ISalesReturnService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<SalesReturnService> _logger;
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
        public SalesReturnService(DFCoreContext context, IAuthService authService, IHostEnvironment environment, ILogger<SalesReturnService> logger, IInventoryTransactionService transactionService, IInventoryAdditional additionalService, IInventoryItemService itemService, IInventoryPaymentService paymentService, DataRederToObj rederToObj, IItemMasterService item, IWarehouseService warehouse, ICustomerSupplierService party, ICostCentreService costCentre, CommonService com, ISettingsService settings)
        {
            _context = context;
            _authService = authService;
            _environment = environment;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = additionalService;
            _itemService = itemService;
            _paymentService = paymentService;
            _rederToObj = rederToObj;
            _item = item;
            _warehouse = warehouse;
            _party = party;
            _costCentre = costCentre;
            _com = com;
            _settings = settings;
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
        /// <summary>
        /// SalesReturn And SalesReturnForm9(save)
        /// </summary>
        /// <param name="salesRetunDto"></param>
        /// <param name="PageId"></param>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public CommonResponse SaveSalesReturn(InventoryTransactionDto salesRetunDto, int PageId, int voucherId)
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
                        return PermissionDenied("Save SalesRetun");
                    }
                    //int VoucherId=_com.GetVoucherId(PageId);
                    string Status = "Approved";

                    var transaction=_transactionService.SaveTransaction(salesRetunDto, PageId, voucherId, Status).Data;
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
                    if (salesRetunDto.TransactionEntries.Cash.Count > 0 || salesRetunDto.TransactionEntries.Card.Count > 0 || salesRetunDto.TransactionEntries.Cheque.Count > 0)
                    {
                        salesRetunDto.Description = null;
                        transpayId = (int)_transactionService.SaveTransactionPayment(salesRetunDto, TransId, Status, 2).Data;
                    }
                    if (salesRetunDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(salesRetunDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (salesRetunDto.References.Count > 0 && salesRetunDto.References.Any(x => x.Sel == true))
                    {
                        List<int?> referIds = salesRetunDto.References.Select(x => x.Id).ToList();

                         _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (salesRetunDto.Items != null)
                    {
                        _itemService.SaveInvTransItems(salesRetunDto.Items, voucherId, TransId, salesRetunDto.ExchangeRate, salesRetunDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesRetunDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesRetunDto, PageId, TransId, transpayId).Data;
                        if (salesRetunDto.TransactionEntries.Advance != null && salesRetunDto.TransactionEntries.Advance.Any(a=>a.VID!=null || a.VID!=0)|| transpayId!=TransId)

                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId, salesRetunDto.TransactionEntries);
                        }
                    }
                    if (salesRetunDto != null)
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
                    return CommonResponse.Error();
                }
            }
        }
        /// <summary>
        /// SalesReturn And SalesReturnForm9(update)
        /// </summary>
        /// <param name="salesRetunDto"></param>
        /// <param name="PageId"></param>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public CommonResponse UpdateSalesReturn(InventoryTransactionDto salesReturnDto, int PageId, int voucherId)
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
                    //Update Transactions
                    int TransId = (int)_transactionService.SaveTransaction(salesReturnDto, PageId, voucherId, Status).Data;
                    int transpayId = TransId;
                    if (salesReturnDto.TransactionEntries.Cash.Count > 0 || salesReturnDto.TransactionEntries.Card.Count > 0 || salesReturnDto.TransactionEntries.Cheque.Count > 0)
                    {
                        salesReturnDto.Description = null;
                        transpayId = (int)_transactionService.SaveTransactionPayment(salesReturnDto, TransId, Status, 2).Data;
                    }

                    if (salesReturnDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(salesReturnDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (salesReturnDto.References.Count > 0 && salesReturnDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = salesReturnDto.References.Select(x => x.Id).ToList();

                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (salesReturnDto.Items != null)

                    {
                        _itemService.UpdateInvTransItems(salesReturnDto.Items, voucherId, TransId, salesReturnDto.ExchangeRate, salesReturnDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesReturnDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesReturnDto, PageId, TransId, transpayId).Data;
                        if (salesReturnDto.TransactionEntries.Advance != null && salesReturnDto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0) || TransId != transpayId)
                        {

                            _transactionService.SaveVoucherAllocation(TransId, transpayId, salesReturnDto.TransactionEntries);

                        }
                    }
                    if (salesReturnDto != null)
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
        public CommonResponse DeleteSalesReturn(int TransId,int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Delete SalesReturn");
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
        public CommonResponse CancelSalesReturn(int TransId,int pageId,string reason)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Cancel SalesReturn");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                _transactionService.CancelTransaction(TransId,reason);
                _logger.LogInformation("Canceled successfully");
                return CommonResponse.Ok("Canceled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

    }
}
