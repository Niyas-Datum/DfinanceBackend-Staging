﻿using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.Transactions;
namespace Dfinance.Purchase.Services
{
    public class PurchaseService : IPurchaseService
    {

        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<PurchaseService> _logger;
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


        public PurchaseService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment,
            ILogger<PurchaseService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
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
        //getting new autogenerated voucherNo,dropdown for projects,warehouse dropdown,
        public CommonResponse GetData(int pageId, int voucherId)
        {
            int branchId = _authService.GetBranchId().Value;
            var voucherNo1 = _transactionService.GetAutoVoucherNo(voucherId);
            var voucherNo = voucherNo1.Data;
            var costcentre1 = _costCentre.FillCostCentre();
            var costcentre = costcentre1.Data;
            var warehouse1 = _warehouse.WarehouseDropdownUsingBranch(branchId);
            var warehouse = warehouse1.Data;
            return CommonResponse.Ok(new { VNo = voucherNo, CostCentre = costcentre, WareHouse = warehouse });
        }

        /// <summary>
        /// fill items in transaction pages
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="PageId"></param>
        /// <param name="locId"></param>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId)
        {
            var res = _item.FillTransItems(partyId, PageID, locId, voucherId);
            return CommonResponse.Ok(res);
        }
        /// <summary>
        /// fill left grid
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public CommonResponse FillPurchase(int PageId, bool? post)
        {
            try
            {
                int branchid = _authService.GetBranchId().Value;
                var data = _context.Fillvoucherview.FromSqlRaw($"Exec LeftGridMasterSP @Criteria='FillVoucher',@BranchID='{branchid}',@MaPageMenuID={PageId},@Posted={post}").ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }

        }
        /// <summary>
        /// Fill PurchaseById
        /// </summary>
        /// <param name="TransId"></param>
        /// <returns></returns>
        public CommonResponse FillPurchaseById(int TransId, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 3))
                {
                    return PermissionDenied("Fill Purchase");
                }
                string criteria = "Fill";
                PurchaseFillByIdDto purchaseFillByIdDto = new PurchaseFillByIdDto();

                _context.Database.OpenConnection();

                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec VoucherSP @Criteria='{criteria}',@TransactionID='{TransId}'";

                    using (var reader = dbCommand.ExecuteReader())
                    {
                        purchaseFillByIdDto.fillTransactions = _rederToObj.Deserialize<FillTransactions>(reader).FirstOrDefault();
                        reader.NextResult();
                        purchaseFillByIdDto.fillTransactionEntries = _rederToObj.Deserialize<FillTransactionEntries>(reader).ToList();
                        reader.NextResult();
                        purchaseFillByIdDto.fillVoucherAllocationUsingRef = _rederToObj.Deserialize<FillVoucherAllocationUsingRef>(reader).FirstOrDefault();
                        reader.NextResult();
                        purchaseFillByIdDto.fillCheques = _rederToObj.Deserialize<FillCheques>(reader).ToList();
                        reader.NextResult();
                        purchaseFillByIdDto.fillTransCollnAllocations = _rederToObj.Deserialize<FillTransCollnAllocations>(reader).FirstOrDefault();
                        reader.NextResult();
                        purchaseFillByIdDto.fillInvTransItems = _rederToObj.Deserialize<FillInvTransItems>(reader).ToList();
                        reader.NextResult();
                        purchaseFillByIdDto.fillInvTransItemDetails = _rederToObj.Deserialize<FillInvTransItemDetails>(reader).FirstOrDefault();
                        reader.NextResult();
                        purchaseFillByIdDto.fillTransactionItemExpenses = _rederToObj.Deserialize<FillTransactionItemExpenses>(reader).FirstOrDefault();
                        reader.NextResult();
                        purchaseFillByIdDto.fillDocuments = _rederToObj.Deserialize<FillDocuments>(reader).FirstOrDefault();
                        reader.NextResult();
                        purchaseFillByIdDto.FillTransactionAdditional = _rederToObj.Deserialize<FillTransactionAdditional>(reader).FirstOrDefault();
                        reader.NextResult();

                        purchaseFillByIdDto.fillTransactionExpenses = _rederToObj.Deserialize<FillTransactionExpenses>(reader).ToList();
                        reader.NextResult();
                        //purchaseFillByIdDto.fillDocumentRequests = _rederToObj.Deserialize<FillDocumentRequests>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillDocumentReferences = _rederToObj.Deserialize<FillDocumentReferences>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransactionReferences = _rederToObj.Deserialize<FillTransactionReferences>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransLoadSchedules = _rederToObj.Deserialize<FillTransLoadSchedules>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransCollections = _rederToObj.Deserialize<FillTransCollections>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransEmployees = _rederToObj.Deserialize<FillTransEmployees>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.vMFuelLog = _rederToObj.Deserialize<VMFuelLog>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillDocumentImages = _rederToObj.Deserialize<FillDocumentImages>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillHRFinalSettlement = _rederToObj.Deserialize<FillHRFinalSettlement>(reader).FirstOrDefault();
                        //reader.NextResult();
                        //purchaseFillByIdDto.fillTransCostAllocations = _rederToObj.Deserialize<FillTransCostAllocations>(reader).FirstOrDefault();
                        //reader.NextResult();
                    }
                }

                if (purchaseFillByIdDto.fillTransactions != null)
                {
                    return CommonResponse.Ok(purchaseFillByIdDto);
                }

                return CommonResponse.NotFound("Purchase not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Save Purchase
        /// </summary>
        /// <param name="invTranseDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>
        /// <returns></returns>
        public CommonResponse SavePurchase(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 2))
            {
                return PermissionDenied("Save Purchase");
            }
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    //int VoucherId=_com.GetVoucherId(PageId);
                    string Status = "Approved";
                    var transaction = _transactionService.SaveTransaction(invTranseDto, PageId, voucherId, Status).Data;
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
                    int transpayId = (int)_transactionService.SaveTransactionPayment(invTranseDto, TransId, Status, 2).Data;
                    if (invTranseDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(invTranseDto.FiTransactionAdditional, TransId, voucherId);
                    }

                    if (invTranseDto.References.Count > 0 && invTranseDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = invTranseDto.References.Select(x => x.Id).ToList();

                        _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (invTranseDto.Items != null && voucherId == 17)
                    {
                        _itemService.SaveInvTransItems(invTranseDto.Items, voucherId, TransId, invTranseDto.ExchangeRate, invTranseDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (invTranseDto.TransactionEntries != null)
                    {
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(invTranseDto, PageId, TransId, transpayId).Data;

                        if (invTranseDto.TransactionEntries.Advance != null && invTranseDto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0))
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId, invTranseDto.TransactionEntries);
                        }
                    }
                    if (invTranseDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Purchase Saved Successfully");
                    return CommonResponse.Created("Created Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError("Failed to Save Purchase");
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        /// <summary>
        /// Update Purchace
        /// </summary>
        /// <param name="invTranseDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>
        /// <returns></returns>
        public CommonResponse UpdatePurchase(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 3))
            {
                return PermissionDenied("Update Purchase");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    //int VoucherId = _com.GetVoucherId(PageId);
                    string Status = "Approved";
                    int TransId = (int)_transactionService.SaveTransaction(invTranseDto, PageId, voucherId, Status).Data;

                    int transpayId = (int)_transactionService.SaveTransactionPayment(invTranseDto, TransId, Status, 2).Data;

                    if (invTranseDto.FiTransactionAdditional != null)
                    {
                        _additionalService.SaveTransactionAdditional(invTranseDto.FiTransactionAdditional, TransId, voucherId);
                    }
                    if (invTranseDto.References.Count > 0 && invTranseDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = invTranseDto.References.Select(x => x.Id).ToList();
                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (invTranseDto.Items != null)
                    {
                        _itemService.UpdateInvTransItems(invTranseDto.Items, voucherId, TransId, invTranseDto.ExchangeRate, invTranseDto.FiTransactionAdditional.Warehouse.Id);
                    }
                    if (invTranseDto.TransactionEntries != null)
                    {
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(invTranseDto, PageId, TransId, transpayId).Data;

                        if (invTranseDto.TransactionEntries.Advance != null && invTranseDto.TransactionEntries.Advance.Any(a => a.AccountID != null || a.AccountID != 0))
                        {
                            _transactionService.UpdateVoucherAllocation(TransId, transpayId, invTranseDto.TransactionEntries);
                        }
                    }
                    if (invTranseDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Purchase Update Successfully");
                    return CommonResponse.Created("Prchase Update Successfully");
                }

                catch (Exception ex)
                {
                    _logger.LogError("Updation of Purchase Failed");
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>


        public CommonResponse DeletePurchase(int TransId, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 5))
                {
                    return PermissionDenied("Delete Purchase");
                }
                var result = _transactionService.DeletePurchase(TransId);
                _logger.LogInformation("Successfully Deleted Purchase");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Delete Purchase");
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// Inv=>Report=>PurchaseRegister
        /// </summary>
        /// <param name="reportdto"></param>
        /// <returns></returns>
        public CommonResponse GetPurchaseReport(PurchaseReportDto reportdto)
        {
            object result = null;

            try
            {
                string query = $@"
            EXEC InventoryRegisterSP 
            @DateFrom = '{reportdto.From}',
            @DateUpto = '{reportdto.To}',
            @BranchID = '{reportdto.Branch?.Id ?? 0}'";

                if (reportdto.BaseType?.Id != 0 && reportdto.BaseType.Id != -1)
                {
                    query += $", @BasicVTypeID = '{reportdto.BaseType.Id}'";
                }

                if (reportdto.VoucherType?.Id != 0 && reportdto.VoucherType.Id != -1)
                {
                    query += $", @VTypeID = '{reportdto.VoucherType.Id}'";
                }
                if (reportdto.Detailed==true)
                {
                    query += $", @Detailed = '{reportdto.Detailed}'";
                }
                if (reportdto.Inventory == true)
                {
                    query += $", @Inventory = '{reportdto.Inventory}'";
                }
                if (reportdto.Columnar == true)
                {
                    query += $", @Columnar = '{reportdto.Columnar}'";
                }
                if (reportdto.GroupItem == true)
                {
                    query += $", @IsGroupItemReport = '{reportdto.GroupItem}'";
                }

                if (!string.IsNullOrEmpty(reportdto.InvoiceNo))
                {
                    query += $", @PartyInvNo = '{reportdto.InvoiceNo}'";
                }

                if (reportdto.customerSupplier?.Id != 0)
                {
                    query += $", @AccountID = '{reportdto.customerSupplier.Id}'";
                }

                if (reportdto.PaymentType?.Id != 0)
                {
                    query += $", @PaymentTypeID = '{reportdto.PaymentType.Id}'";
                }

                if (reportdto.Item?.Id != 0)
                {
                    query += $", @ItemID = '{reportdto.Item.Id}'";
                }

                if (reportdto.Counter?.Id != 0)
                {
                    query += $", @CounterID = '{reportdto.Counter.Id}'";
                }

                if (!string.IsNullOrEmpty(reportdto.BatchNo))
                {
                    query += $", @BatchNo = '{reportdto.BatchNo}'";
                }

                if (reportdto.User?.Id != 0)
                {
                    query += $", @UserID = '{reportdto.User.Id}'";
                }

                if (reportdto.Staff?.Id != 0)
                {
                    query += $", @StaffID = '{reportdto.Staff.Id}'";
                }

                if (reportdto.Area?.Id != 0)
                {
                    query += $", @AreaID = '{reportdto.Area.Id}'";
                }

                // Add criteria if provided
                if (reportdto.ViewBy==false)
                {
                    query += $", @Criteria = 'Extract'";
                    result = _context.PurchaseReportViews.FromSqlRaw(query).ToList();
                    return CommonResponse.Ok(result);
                }
               

                result = _context.PurchaseReportView.FromSqlRaw(query).ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        //public CommonResponse GetPurchaseReport(PurchaseReportDto reportdto)
        //{

        //    object result = null;

        //    try
        //    {
        //        if (reportdto.ViewBy == true)
        //        {

        //            var inventory = $@"
        //            EXEC InventoryRegisterSP 
        //            @DateFrom = '{reportdto.From}',
        //            @DateUpto = '{reportdto.To}',
        //            @BranchID = '{reportdto.Branch.Id ?? 0}',
        //            @BranchID = '{reportdto.Branch.Id ?? 0}',
        //            @UserID = '{reportdto.User.Id ?? 0}'
        //            @BasicVTypeID = '{reportdto.BaseType.Id ?? 0}'
        //        ";
        //            result = _context.PurchaseReportView.FromSqlRaw(inventory).ToList();
        //            //var query = $@"EXEC InventoryRegisterSP @Criteria = '{criteria}'
        //            //  @DateFrom = '01/01/1990',
        //            //  @DateUpto = '06/06/2024',
        //            //   @BranchID = '1'";
        //            //@BasicVTypeID = '{reportdto.BaseType.Id??0}',
        //            //@VTypeID = '{reportdto.VoucherType.Id ?? 0}',
        //            //@AccountID = '{reportdto.customerSupplier.Id??0}',
        //            //@PaymentTypeID = '{reportdto.PaymentType.Id ?? 0}',
        //            //@ItemID = '{reportdto.Item.Id??0}',
        //            //@Inventory = '{reportdto.Inventory}',
        //            //@CounterID = '{reportdto.Counter}',
        //            //@PartyInvNo = '{reportdto.InvoiceNo}',
        //            //@BatchNo = '{reportdto.BatchNo}',
        //            //@UserID = '{reportdto.User.Id ?? 0}',
        //            //@AreaID = '{reportdto.Area.Id??0}',
        //            //@StaffID = '{reportdto.Staff.Id??0}'
        //        }
        //        else
        //        {
        //            string criteria = "Extract";
        //            var finance = $@"
        //            EXEC InventoryRegisterSP 
        //            @Criteria = '{criteria}',
        //            @DateFrom = '{reportdto.From}',
        //            @DateUpto = '{reportdto.To}',
        //            @BranchID = '{reportdto.Branch.Id ?? 0}',
        //            @UserID = '{reportdto.User.Id ?? 0}',
        //            @BasicVTypeID = '{reportdto.BaseType.Id ?? 0}'
        //                                                            ";
        //            result = _context.PurchaseReportViews.FromSqlRaw(finance).ToList();
        //        }

        //        return CommonResponse.Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return CommonResponse.Error(ex.Message);
        //    }
    }
}

