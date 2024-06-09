using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory.Purchase.PurchaseReport;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Reports.Interface;
using Dfinance.Shared.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dfinance.Purchase.Reports
{
    public class PurchaseReportService : IPurchaseReportService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseReportService> _logger;
        public PurchaseReportService(DFCoreContext context, IAuthService authService, ILogger<PurchaseReportService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        /// <summary>
        /// fillreport=>purchaseregiser
        /// </summary>
        /// <param name="reportdto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CommonResponse FillPurchaseReport(PurchaseReportDto reportdto)
        {
            string criteria = null;
            object result = null;

            try
            {
                if (reportdto.ViewBy == true)
                {
                    criteria = null;
                    var result1 = _context.PurchaseReportView.FromSqlRaw($@"
                EXEC InventoryRegisterSP 
                @Criteria = '{criteria}',
                @DateFrom = '{reportdto.From}',
                @DateUpto = '{reportdto.To}',
                @BranchID = '{reportdto.Branch?.Id}',
                @BasicVTypeID = '{reportdto.BaseType?.Id}',
                @VTypeID = '{reportdto.VoucherType?.Id}',
                @AccountID = '{reportdto.customerSupplier?.Id}',
                @PaymentTypeID = '{reportdto.PaymentType?.Id}',
                @ItemID = '{reportdto.Item?.Id}',
                @Inventory = '{reportdto.Inventory}',
                @CounterID = '{reportdto.Counter?.Id}',
                @PartyInvNo = '{reportdto.InvoiceNo?.Id}',
                @BatchNo = '{reportdto.BatchNo?.Name}',
                @UserID = '{reportdto.User?.Id}',
                @AreaID = '{reportdto.Area?.Id}',
                @StaffID = '{reportdto.Staff?.Id}'
            ").ToList();
                }
                else
                {
                    criteria = "Extract";
                    result = _context.PurchaseReportViews.FromSqlRaw($@"
                EXEC InventoryRegisterSP 
                @Criteria = '{criteria}',
                @DateFrom = '{reportdto.From}',
                @DateUpto = '{reportdto.To}',
                @BranchID = '{reportdto.Branch?.Id}',
                @BasicVTypeID = '{reportdto.BaseType?.Id}',
                @VTypeID = '{reportdto.VoucherType?.Id}',
                @AccountID = '{reportdto.customerSupplier?.Id}',
                @PaymentTypeID = '{reportdto.PaymentType?.Id}',
                @ItemID = '{reportdto.Item?.Id}',
                @Inventory = '{reportdto.Inventory}',
                @CounterID = '{reportdto.Counter?.Id}',
                @PartyInvNo = '{reportdto.InvoiceNo?.Id}',
                @BatchNo = '{reportdto.BatchNo?.Name}',
                @UserID = '{reportdto.User?.Id}',
                @AreaID = '{reportdto.Area?.Id}',
                @StaffID = '{reportdto.Staff?.Id}'
            ").ToList();
                }
               
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }



    }
}
