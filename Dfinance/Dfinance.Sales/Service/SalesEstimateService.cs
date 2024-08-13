using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Sales.Service.Interface;
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
using System.Threading.Tasks;
using System.Transactions;

namespace Dfinance.Sales.Service
{
    public class SalesEstimateService : ISalesEstimateService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<SalesEstimateService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        private readonly DataRederToObj _rederToObj;
        private readonly IItemMasterService _item;
        private readonly IWarehouseService _warehouse;
        private readonly ICustomerSupplierService  _party;
        private readonly ICostCentreService _costCentre;
        private readonly CommonService _com;
        private readonly ISettingsService _settings;
        public SalesEstimateService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment,
            ILogger<SalesEstimateService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
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
        public CommonResponse SaveSalesEstimate(InventoryTransactionDto salesDto, int PageId, int voucherId)
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
                        _itemService.SaveInvTransItems(salesDto.Items, voucherId, TransId, salesDto.ExchangeRate, salesDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (salesDto.TransactionEntries != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(salesDto, PageId, TransId, transpayId).Data;

                        if (salesDto.TransactionEntries.Advance != null && salesDto.TransactionEntries.Advance.Any())
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
        public CommonResponse UpdateSalesEstimate(InventoryTransactionDto salesDto, int PageId, int voucherId)
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

