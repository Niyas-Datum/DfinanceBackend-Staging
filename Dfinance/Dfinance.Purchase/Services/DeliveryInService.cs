using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Dfinance.Purchase.Services
{
    public class DeliveryInService:IDeliveryInService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        private readonly DataRederToObj _rederToObj;
        private readonly IItemMasterService _item;
        private readonly IWarehouseService _warehouse;
        private readonly ICostCentreService _costCentre;
        
        public DeliveryInService(DFCoreContext context, IAuthService authService,ILogger<PurchaseService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
            IInventoryItemService inventoryItemService, IInventoryPaymentService inventoryPaymentService, DataRederToObj rederToObj, IItemMasterService item,
            IWarehouseService warehouse, ICostCentreService costCentre)
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

        public CommonResponse SaveDeliveyIn(InventoryTransactionDto invTranseDto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save DeliveyIn");
            }
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    string Status = "Approved";
                    //var invMapp = _mapper.Map<InventoryTransactionDto, FiTransaction>(invTranseDto);
                    int TransId = (int)_transactionService.SaveTransaction(invTranseDto, pageId, voucherId, Status).Data;
                    if (invTranseDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(invTranseDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (invTranseDto.References.Count > 0 && invTranseDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = invTranseDto.References.Select(x => x.Id).ToList();
                        _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (invTranseDto.Items != null)
                    {
                        _itemService.SaveInvTransItems(invTranseDto.Items, voucherId, TransId, invTranseDto.ExchangeRate, invTranseDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    transactionScope.Complete();                  
                    _logger.LogInformation("DeliveyIn Saved Successfully");
                    return CommonResponse.Created("DeliveyIn Saved Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Save DeliveryIn");
                    transactionScope.Dispose();
                    return CommonResponse.Error();
                }
            }
        }
        public CommonResponse UpdateDeliveryIn(InventoryTransactionDto invTranseDto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Update DeliveyIn");
            }
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    string Status = "Approved";
                    //var invMapp = _mapper.Map<InventoryTransactionDto, FiTransaction>(invTranseDto);
                    int TransId = (int)_transactionService.SaveTransaction(invTranseDto, pageId, voucherId, Status).Data;
                    if (invTranseDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(invTranseDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (invTranseDto.References.Count > 0 && invTranseDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = invTranseDto.References.Select(x => x.Id).ToList();
                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (invTranseDto.Items != null && voucherId == 17)
                    {
                        _itemService.UpdateInvTransItems(invTranseDto.Items, voucherId, TransId, invTranseDto.ExchangeRate, invTranseDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    transactionScope.Complete();
                    transactionScope.Complete();
                    _logger.LogInformation("DeliveyIn Updated Successfully");
                    return CommonResponse.Created("DeliveyIn Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Update DeliveryIn");
                    transactionScope.Dispose();
                    return CommonResponse.Error();
                }
            }
        }
        public CommonResponse DeleteDeliveryIn(int TransId, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Delete DeliveyIn");
                }
                var result = _transactionService.DeleteTransactions(TransId);
                _logger.LogInformation("Successfully Deleted DeliveyIn");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Delete DeliveyIn");
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse CancelDeliveryIn(int TransId, int pageId,string reason)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Cancel DeliveyIn");
                }
                var result = _transactionService.CancelTransaction(TransId,reason);
                _logger.LogInformation("Successfully Canceled DeliveyIn");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Cancel DeliveyIn");
                return CommonResponse.Error(ex);
            }
        }
    }
}
