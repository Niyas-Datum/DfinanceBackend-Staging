using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Transactions;

namespace Dfinance.Purchase.Services
{
    public class ItemReservationService : IItemReservationService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<IItemReservationService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IUserTrackService _userTrackService;
        private readonly IWarehouseService _warehouseService;
        private readonly IMapper _mapper;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;

        public ItemReservationService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment,
            ILogger<IItemReservationService> logger, IUserTrackService userTrackService, IInventoryTransactionService transactionService,
            IWarehouseService warehouseService, IMapper mapper, IInventoryAdditional inventoryAdditional, IInventoryItemService inventoryItemService)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
            _logger = logger;
            _transactionService = transactionService;
            _userTrackService = userTrackService;
            _warehouseService = warehouseService;
            _mapper = mapper;
            _additionalService = inventoryAdditional;
            _itemService = inventoryItemService;
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
        private CommonResponse GetParties()
        {
            var branchId = _authService.GetBranchId().Value;
            var accountCategories = new int[] { 1, 2 };
            var result = from A in _context.FiMaAccounts
                         join B in _context.FiMaBranchAccounts on A.Id equals B.AccountId
                         join P in _context.Parties on A.Id equals P.AccountId into joinedParties
                         from P in joinedParties.DefaultIfEmpty() // Left join
                         where A.IsGroup == false
                            && A.Active == true
                            // && accountCategories.Contains(A.AccountCategory)
                            && A.AccountCategory == 1 || A.AccountCategory == 2
                            && B.BranchId == branchId
                         select new
                         {
                             AccountCode = A.Alias,
                             AccountName = A.Name,
                             Address = (P.AddressLineOne == null || P.AddressLineOne.Trim() == "" ? "" :
                                        (P.AddressLineTwo != null && P.City != null && P.Pobox != null ? P.AddressLineOne + ", " : P.AddressLineOne + ". ")) +
                                       (P.AddressLineTwo == null || P.AddressLineTwo.Trim() == "" ? "" :
                                        (P.City != null && P.Pobox != null ? P.AddressLineTwo + ", " : P.AddressLineTwo + ". ")) +
                                       (P.City == null || P.City.Trim() == "" ? "" :
                                        (P.Pobox != null ? P.City + ", " : P.City + ". ")) +
                                       (P.Pobox == null || P.Pobox.Trim() == "" ? "" : P.Pobox + "."),
                             ID = A.Id
                         };

            var resultList = result.ToList();
            return CommonResponse.Ok(resultList);
        }
        private CommonResponse GetProjects()
        {
            var projects = _context.CostCentre.Where(c => c.PType == "Real").Select(c => new { ProjectCode = c.Code, ProjectName = c.Description, c.Id }).ToList();
            return CommonResponse.Ok(projects);
        }
        public CommonResponse GetLoadData()
        {
            try
            {
                var branchId = _authService.GetBranchId().Value;
                var locations = _warehouseService.WarehouseDropdownUsingBranch(branchId).Data;
                var parties = GetParties().Data;
                var projects = GetProjects().Data;
                _logger.LogInformation("LoadData");
                return CommonResponse.Ok(new { Locations = locations, Parties = parties, Projects = projects });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse FillVoucher(int pageId)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            var branchId = _authService.GetBranchId().Value;
            cmd.CommandText = $"Exec LeftGridMasterSP @Criteria='FillVoucher',@BranchID={branchId},@MaPageMenuID={pageId}";
            _context.Database.GetDbConnection().Open();
            using (var reader = cmd.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(reader);
                if (tb.Rows.Count > 0)
                {
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in tb.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in tb.Columns)
                        {
                            row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                        }
                        rows.Add(row);
                    }
                    return CommonResponse.Ok(rows);
                }
                return CommonResponse.NoContent();
            }


        }
        public CommonResponse FillMaster(int pageId, int? TransactionId = null)
        {
            try
            {
                _logger.LogInformation("FillVoucher");
                if (TransactionId == null && pageId != null)
                {
                    return CommonResponse.Ok(FillVoucher(pageId).Data);
                }
                else
                {
                    return CommonResponse.Ok(FillVoucherById(TransactionId).Data);
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse FillVoucherById(int? TransactionID)
        {

            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;

            var branchId = _authService.GetBranchId().Value;
            cmd.CommandText = $"Exec VoucherSP @Criteria='FillVoucherByTransactionID',@TransactionID={TransactionID}";
            _context.Database.GetDbConnection().Open();
            using (var reader = cmd.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(reader);
                if (tb.Rows.Count > 0)
                {
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in tb.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in tb.Columns)
                        {
                            row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                        }
                        rows.Add(row);
                    }
                    return CommonResponse.Ok(rows);
                }
                return CommonResponse.NoContent();
            }


        }
        public CommonResponse SaveItemReserv(ItemReservationDto ItemReservation, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 2))
            {
                return PermissionDenied("Save ItemReservation");
            }
            var reservDto = _mapper.Map<ItemReservationDto, InventoryTransactionDto>(ItemReservation);
            var reservItem = _mapper.Map<List<InvTransItemReservDto>, List<InvTransItemDto>>(ItemReservation.Items);
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    if (reservDto.FiTransactionAdditional == null)
                        reservDto.FiTransactionAdditional = new InvTransactionAdditionalDto();
                    if (reservDto.FiTransactionAdditional.Warehouse == null)
                        reservDto.FiTransactionAdditional.Warehouse = new DataModels.Dto.Common.DropdownDto();
                    reservDto.FiTransactionAdditional.Warehouse.Id = ItemReservation.Warehouse.Id;
                    reservDto.FiTransactionAdditional.PartyNameandAddress = ItemReservation.PartyInfo;
                    reservDto.Items = reservItem;

                    if (reservDto != null)
                    {
                        var transId = (int)_transactionService.SaveTransaction(reservDto, PageId, voucherId, "Approved").Data;
                        _additionalService.SaveTransactionAdditional(reservDto.FiTransactionAdditional, transId, voucherId);
                        if (reservDto.Items != null)
                            _itemService.SaveInvTransItems(reservItem, voucherId, transId, null, ItemReservation.Warehouse.Id);
                    }

                    _logger.LogInformation("Successfully Created");
                    transactionScope.Complete();
                    return CommonResponse.Ok("Successfully Created");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    return CommonResponse.Error(ex);
                }
            }

        }

        public CommonResponse UpdateItemReserv(ItemReservationDto ItemReservation, int PageId, int voucherId)
        {
            if (!_authService.IsPageValid(PageId))
            {
                return PageNotValid(PageId);
            }
            if (!_authService.UserPermCheck(PageId, 2))
            {
                return PermissionDenied("Update ItemReservation");
            }
            var reservDto = _mapper.Map<ItemReservationDto, InventoryTransactionDto>(ItemReservation);
            var reservItem = _mapper.Map<List<InvTransItemReservDto>, List<InvTransItemDto>>(ItemReservation.Items);
            using (var transactionScope = new TransactionScope())

            {
                try
                {
                    if (reservDto.FiTransactionAdditional == null)
                        reservDto.FiTransactionAdditional = new InvTransactionAdditionalDto();
                    if (reservDto.FiTransactionAdditional.Warehouse == null)
                        reservDto.FiTransactionAdditional.Warehouse = new DataModels.Dto.Common.DropdownDto();
                    reservDto.FiTransactionAdditional.Warehouse.Id = ItemReservation.Warehouse.Id;
                    reservDto.FiTransactionAdditional.PartyNameandAddress = ItemReservation.PartyInfo;
                    reservDto.Items = reservItem;

                    if (reservDto != null)
                    {
                        var transId = (int)_transactionService.SaveTransaction(reservDto, PageId, voucherId, "Approved").Data;
                        _additionalService.SaveTransactionAdditional(reservDto.FiTransactionAdditional, transId, voucherId);
                        if (reservDto.Items != null)
                            _itemService.UpdateInvTransItems(reservItem, voucherId, transId, null, ItemReservation.Warehouse.Id);
                    }

                    _logger.LogInformation("Successfully Updated");
                    transactionScope.Complete();
                    return CommonResponse.Ok("Successfully Updated");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    return CommonResponse.Error(ex);
                }
            }

        }
        private CommonResponse FillInvTransItems(int TransId)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"Exec VoucherAdditionalsSP @criteria='FillInvTransItems',@TransactionID={TransId}";
            if (_context.Database.GetDbConnection().State == ConnectionState.Closed)            
                _context.Database.GetDbConnection().Open();
            using (var reader = cmd.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(reader);
                if (tb.Rows.Count > 0)
                {
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in tb.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in tb.Columns)
                        {
                            row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                        }
                        rows.Add(row);
                    }                   
                    return CommonResponse.Ok(rows);
                }
            }
            return CommonResponse.NotFound();
        }

        public CommonResponse FillById(int transId)
        {
            var transDisclist = _transactionService.FillTransactionbyId(transId).Data;           
            var additionals = _additionalService.FillTransactionAdditionals(transId).Data;
            var items = FillInvTransItems(transId).Data;
            //var reservDto = _mapper.Map<InventoryTransactionDto,ItemReservationDto >(trans);
            //var reservItem = _mapper.Map<List<InvTransItemDto>,List<InvTransItemReservDto> >(items);
            //reservDto.Items = reservItem;
            //reservDto.Warehouse.Id = trans.FiTransactionAdditional.Warehouse.Id;
            //reservDto.PartyInfo = trans.FiTransactionAdditional.PartyNameandAddress;
            return CommonResponse.Ok(new { Transactions = transDisclist,TransAdditionals=additionals,Items=items });
        }
    }
}
