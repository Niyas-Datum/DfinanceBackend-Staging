﻿using AutoMapper;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.Item;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Restaurant.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Warehouse.Services.Interface;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics.Contracts;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dfinance.Restaurant
{
    public class RestaurantInvoice : IRestaurantInvoice
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<RestaurantInvoice> _logger;
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
        private readonly IMapper _mapper;
        private readonly IUserTrackService _userTrackService;
        public RestaurantInvoice(DFCoreContext context, IAuthService authService, IHostEnvironment environment, ILogger<RestaurantInvoice> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
            IInventoryItemService inventoryItemService, IInventoryPaymentService inventoryPaymentService, DataRederToObj rederToObj, IItemMasterService item,
            IWarehouseService warehouse, ICostCentreService costCentre, ICustomerSupplierService party, IUserTrackService userTrackService, CommonService com, ISettingsService settings, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _environment = environment;
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
            _mapper = mapper;
            _userTrackService = userTrackService;
        }
        private CommonResponse GetWaiter()
        {
            try
            {

                //fiMaAccount.AccountCategory == 3
                //                     &&
                var waitors = (from employee in _context.Hremployees
                               join fiMaAccount in _context.FiMaAccounts on employee.AccountId equals fiMaAccount.Id
                               join designation in _context.MaDesignations on employee.DesignationId equals designation.Id
                               where fiMaAccount.Active == true
                                     && designation.Name == "Waiter"
                               select new
                               {
                                   Name = fiMaAccount.Name,
                                   Code = fiMaAccount.Alias,
                                   ID = fiMaAccount.Id
                               }).ToList();
                _logger.LogInformation("FillWaiter successfully");
                return CommonResponse.Ok(waitors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse FillSection()
        {
            try
            {
                var sections = _context.MaMisc.Where(m => m.Key == "RestaurantSection" && m.Active == true).ToList();
                _logger.LogInformation("FillSection successfully");
                return CommonResponse.Ok(sections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse GetCategory()
        {
            try
            {
                var data = _context.CommodityViews.FromSqlRaw($"Exec RestKitchenCategorySP @Criteria='GetCategory'").ToList();
                _logger.LogInformation("FillCategory successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse FillTables(int sectionId)
        {
            try
            {
                int branchid = _authService.GetBranchId().Value;
                var data = _context.TableViews.FromSqlRaw($"Exec RestTablesSP @Criteria='FillTablesStatus',@SectionId={sectionId},@BranchID={branchid}").ToList();
                _logger.LogInformation("FillTables successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetLoadData(int voucherId)
        {
            var category = GetCategory();
            var waiter = GetWaiter();
            var section = FillSection();
            var voucherNo1 = _transactionService.GetAutoVoucherNo(voucherId);
            var voucherNo = voucherNo1.Data;
            return CommonResponse.Ok(new { Vno = voucherNo, Category = category.Data, Waiter = waiter.Data, Section = section.Data });
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
        public CommonResponse SaveRestaurentInvoice(InventoryTransactionDto RestaurentDto, int PageId, int voucherId, int sectionId, int tableId, string tableName, string? tokenId, string? deliveryId, int salesManId,string chairName)
        {
            var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();

            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    SetSettings();
                    if (RestaurentDto.FiTransactionAdditional == null)
                        RestaurentDto.FiTransactionAdditional = new InvTransactionAdditionalDto();
                    RestaurentDto.FiTransactionAdditional.CreditPeriod = tableId;
                    RestaurentDto.FiTransactionAdditional.AddressLine1 = tableName;
                    RestaurentDto.FiTransactionAdditional.AddressLine2 = chairName;
                    RestaurentDto.FiTransactionAdditional.OrderNo = tokenId;
                    RestaurentDto.FiTransactionAdditional.DeliveryNote = deliveryId;
                    RestaurentDto.FiTransactionAdditional.Days = sectionId;
                    if (RestaurentDto.FiTransactionAdditional.SalesMan == null)
                        RestaurentDto.FiTransactionAdditional.SalesMan = new PopUpDto();
                    RestaurentDto.FiTransactionAdditional.SalesMan.Id = salesManId;
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
                    RestaurentDto.FiTransactionAdditional.Code = GetNextOrderNo(voucherId).Data.ToString();
                    var transaction = _transactionService.SaveTransaction(RestaurentDto, PageId, voucherId, Status).Data;
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

                    if (RestaurentDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(RestaurentDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    //if (RestaurentDto.References.Count > 0 && RestaurentDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    //{
                    //    List<int?> referIds = RestaurentDto.References.Select(x => x.Id).ToList();

                    //    _transactionService.SaveTransReference(TransId, referIds);
                    //}
                    if (RestaurentDto.Items != null)
                    {
                        _itemService.SaveInvTransItems(RestaurentDto.Items, voucherId, TransId, RestaurentDto.ExchangeRate, RestaurentDto.FiTransactionAdditional.Warehouse?.Id ?? null);
                    }
                    var primaryVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
                    if ((VoucherType)primaryVoucherId == VoucherType.RestaurantInvoice && RestaurentDto.TransactionEntries != null)
                    {
                        if (RestaurentDto.TransactionEntries.Cash.Count > 0 || RestaurentDto.TransactionEntries.Card.Count > 0 || RestaurentDto.TransactionEntries.Cheque.Count > 0)
                        {
                            transpayId = (int)_transactionService.SaveTransactionPayment(RestaurentDto, TransId, Status, 2).Data;
                        }
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(RestaurentDto, PageId, TransId, transpayId).Data;

                    }
                    _userTrackService.AddUserActivity(RestaurentDto.VoucherNo, TransId, 0, RestaurentDto.Description, "FiTransactions", moduleName, 0, null);
                    _logger.LogInformation("Successfully Created");
                    transactionScope.Complete();
                    return CommonResponse.Ok(TransId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        public CommonResponse UpdateRestaurentInvoice(InventoryTransactionDto RestaurentDto, int PageId, int voucherId, int sectionId, int tableId, string tableName, string? tokenId, string? deliveryId, int salesManId, string chairName)
        {
            var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    if (RestaurentDto.FiTransactionAdditional == null)
                        RestaurentDto.FiTransactionAdditional = new InvTransactionAdditionalDto();
                    RestaurentDto.FiTransactionAdditional.CreditPeriod = tableId;
                    RestaurentDto.FiTransactionAdditional.AddressLine1 = tableName;
                    RestaurentDto.FiTransactionAdditional.AddressLine2 = chairName;
                    RestaurentDto.FiTransactionAdditional.OrderNo = tokenId;
                    RestaurentDto.FiTransactionAdditional.DeliveryNote = deliveryId;
                    RestaurentDto.FiTransactionAdditional.Days = sectionId;
                    if (RestaurentDto.FiTransactionAdditional.SalesMan == null)
                        RestaurentDto.FiTransactionAdditional.SalesMan = new PopUpDto();
                    RestaurentDto.FiTransactionAdditional.SalesMan.Id = salesManId;
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 2))
                    {
                        return PermissionDenied("Update Invoice");
                    }
                    //int VoucherId=_com.GetVoucherId(PageId);
                    string Status = "Approved";

                    var transaction = _transactionService.SaveTransaction(RestaurentDto, PageId, voucherId, Status).Data;
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

                    if (RestaurentDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(RestaurentDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    //if (RestaurentDto.References.Count > 0 && RestaurentDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    //{
                    //    List<int?> referIds = RestaurentDto.References.Select(x => x.Id).ToList();

                    //    _transactionService.SaveTransReference(TransId, referIds);
                    //}
                    if (RestaurentDto.Items != null)
                    {
                        _itemService.UpdateInvTransItems(RestaurentDto.Items, voucherId, TransId, RestaurentDto.ExchangeRate, RestaurentDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    var primaryVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
                    if ((VoucherType)primaryVoucherId == VoucherType.RestaurantInvoice && RestaurentDto.TransactionEntries != null)
                    {
                        if (RestaurentDto.TransactionEntries.Cash.Count > 0 || RestaurentDto.TransactionEntries.Card.Count > 0 || RestaurentDto.TransactionEntries.Cheque.Count > 0)
                        {
                            transpayId = (int)_transactionService.SaveTransactionPayment(RestaurentDto, TransId, Status, 2).Data;
                        }
                        if (RestaurentDto.TransactionEntries != null)
                        {

                            int TransEntId = (int)_paymentService.SaveTransactionEntries(RestaurentDto, PageId, TransId, transpayId).Data;

                        }

                    }
                    _userTrackService.AddUserActivity(RestaurentDto.VoucherNo, TransId, 1, RestaurentDto.Description, "FiTransactions", moduleName, 0, null);
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

        public CommonResponse GetKitchenCategory(int transactionId)
        {
            try
            {
                var data = _context.KitchenCategoryViews.FromSqlRaw($"Exec RestKitchenCategorySP @Criteria='GetKitchenCategory',@TransactionID={transactionId}").ToList();
                _logger.LogInformation("GetKitchenCategory successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse PrintKOT(int transactionId, int kitchenCatId)
        {
            try
            {
                var data = _context.PrintKotViews.FromSqlRaw($"Exec RestKitchenCategorySP @Criteria='PrintKOT',@TransactionID={transactionId},@ID={kitchenCatId}").ToList();
                _logger.LogInformation("PrintKOT successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetCategories()
        {
            try
            {
                var category = GetCategory().Data;
                _logger.LogInformation("GetKitchenCategory successfully");
                return CommonResponse.Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetProducts(int? categoryId = null)
        {
            try
            {
                string? catId = null;
                if (categoryId == null)
                    catId = "NULL";
                else
                    catId = categoryId.ToString();
                var priceCategoryId = _context.MaPriceCategory.Where(p => p.Name == "Dine In").Select(p => p.Id).FirstOrDefault();
                var data = _context.ProductVews.FromSqlRaw($"Exec RestKitchenCategorySP @Criteria='GetProducts',@CategoryID={catId},@PriceCategoryID={priceCategoryId}").ToList();
                List<ItemUnitsListView>? itemUnitList = new List<ItemUnitsListView>();
                foreach (var item in data)
                {
                    var units = FillItemUnitss(item.ID).Data;
                    var options = FillItemOptions(item.ID).Data as List<ItemOptionsView>;
                    if (units != null)
                    {
                        itemUnitList.Add(new ItemUnitsListView { Items = item, ItemUnits = (List<ItemUnitRestView>)units, ItemOptions = (List<ItemOptionsView>)options });
                    }
                }

                _logger.LogInformation("GetProducts successfully");
                return CommonResponse.Ok(new { Product = itemUnitList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        private string GetAutoVoucherNo(int voucherId)
        {
            int branchid = _authService.GetBranchId().Value;
            var vNo = (from trans in _context.FiTransaction
                       join addition in _context.FiTransactionAdditionals on trans.Id equals addition.TransactionId
                       where trans.VoucherId == voucherId && trans.Date == DateTime.Now.AddHours(-1 * dayCloseLagHours).Date && trans.CompanyId == branchid
                       select (addition.Code)).Max();
            int lastTransNo = Convert.ToInt32(vNo);
            lastTransNo++;
            return lastTransNo.ToString();
        }
        private VoucherNo GetNextTransactionNo(int voucherid)
        {
            var voucher = _context.FiMaVouchers
                    .Where(x => x.Id == voucherid)
                    .FirstOrDefault();
            int branchid = _authService.GetBranchId().Value;
            var result = _context.AccountCodeView
                .FromSqlRaw($"EXEC GetNextAutoEntryVoucherNoSP @VoucherID={voucherid}, @BranchID={branchid}").AsEnumerable()
                .FirstOrDefault();

            VoucherNo voucherNo = new VoucherNo
            {
                Code = voucher.Code,
                Result = result.AccountCode.ToString()
            };

            return voucherNo;
        }
        private int dayCloseLagHours = 0;
        private void SetSettings()
        {
            string[] keys = new string[] { "DayCloseLagHours" };
            var settings = _context.MaSettings
        .Where(m => keys.Contains(m.Key))
        .Select(m => new
        {
            Key = m.Key,
            Value = m.Value,
        }).ToList();
            dayCloseLagHours = Convert.ToInt32(settings.Where(s => s.Key == "DayCloseLagHours").Select(s => s.Value).FirstOrDefault());

        }
        //unit
        private CommonResponse FillItemUnitss(int? itemId)
        {
            try
            {
                var data = _context.ItemUnitRests.FromSqlRaw($"select * from InvItemUnits where ItemID={itemId}").ToList();
                if (data.Count == 0) data = null;
                _logger.LogInformation("FillItemOptions successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        //ItemOptions

        private CommonResponse FillItemOptions(int? itemId)
        {
            try
            {
                var data = _context.ItemOptionsViews.FromSqlRaw($"Exec InvItemOptionsSP @Criteria=2,@ItemId={itemId}").ToList();
                if (data.Count == 0) data = null;
                _logger.LogInformation("FillItemOptions successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse FillItemOptionsMaster()
        {
            try
            {
                var data = _context.ItemOptionsViews.FromSqlRaw($"Exec InvItemOptionsSP @Criteria=1").ToList();
                _logger.LogInformation("FillItemOptionsMaster successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveItemOptions(ItemOptionsDto itemOptionsDto)
        {
            try
            {
                var data = _context.Database.ExecuteSqlRaw($"Exec InvItemOptionsSP @Criteria=3,@ItemID={itemOptionsDto.ItemId},@Options='{itemOptionsDto.Options}'");
                _logger.LogInformation("InsertItemOptions successfully");
                return CommonResponse.Ok("Save Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse UpdateItemOptions(ItemOptionsDto itemOptionsDto)
        {
            try
            {
                var data = _context.Database.ExecuteSqlRaw($"Exec InvItemOptionsSP @Criteria=4,@ItemID={itemOptionsDto.ItemId},@Options='{itemOptionsDto.Options}',@ID={itemOptionsDto.Id}");
                _logger.LogInformation("InsertItemOptions successfully");
                return CommonResponse.Ok("Update Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetWaiterList()
        {
            try
            {
                return CommonResponse.Ok(GetWaiter().Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetSections()
        {
            try
            {
                return CommonResponse.Ok(FillSection().Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveAndPrintKOT(RestaurentDto restaurentDto, int sectionId, int tableId, string tableName, int salesManId,string chairName)
        {
            try
            {
                var restDto = _mapper.Map<RestaurentDto, InventoryTransactionDto>(restaurentDto);                
                var restItem = _mapper.Map<List<InvRestTransItemDto>, List<InvTransItemDto>>(restaurentDto.Items);
                restDto.Items = restItem;
                restDto.VoucherNo = GetNextTransactionNo(141).Result;
                if (restDto.Party == null)
                    restDto.Party = new PopUpDto() { Id = 309 };               
                var transId = SaveRestaurentInvoice(restDto, 469, 141, sectionId, tableId, tableName, null, null, salesManId,chairName).Data;
                var printKot = PrintKOT((int)transId, 1).Data;
                return CommonResponse.Ok(printKot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse GetNextOrderNo(int voucherId)
        {
            var branchId=_authService.GetBranchId();            
            var orderNo = _context.AccountCodeView.FromSqlRaw($"Exec GenerateOrderNoSP @BranchID={branchId},@VoucherID={voucherId}").AsEnumerable().FirstOrDefault();
            return CommonResponse.Ok(orderNo.AccountCode);
        }
        public CommonResponse GetItemsByTransId(int transId)
        {
            try
            {
                var items=(from T in _context.FiTransaction 
                           join TI in _context.InvTransItems on T.Id equals TI.TransactionId
                           join I in _context.ItemMaster on TI.ItemId equals I.Id
                           join A in _context.FiTransactionAdditionals on T.Id equals A.TransactionId
                           where TI.TransactionId == transId
                           select new
                           {
                               TI.Id,
                               I.ItemName,
                               TI.ItemId,
                               Quantity= TI.Qty,
                               OrederNo=A.Code,
                               Time= T.Date.ToString("HH:mm:ss")
                           }).ToList();
                    
                return CommonResponse.Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
    }
}
