using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Statements.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.Finance.Statements
{
    public class FinStatementService : IFinStatementService
    {
        private readonly DFCoreContext _context;
        private readonly ILogger<FinStatementService> _logger;
        private readonly IAuthService _authService;
        private readonly DataRederToObj _rederToObj;
        public FinStatementService(DFCoreContext context, ILogger<FinStatementService> logger, IAuthService authService, DataRederToObj rederToObj)
        {
            _context = context;
            _logger = logger;
            _authService = authService;
            _rederToObj = rederToObj;
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
        //fill Day book
        public CommonResponse FillDayBook(DayBookDto dayBookDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Day Book");
            }
            try
            {
                string criteria = "FillFinanceVoucherSummary";
                var dayBookData = _context.DayBookView.FromSqlRaw("Exec FinTransactionsSP @Criteria={0},@DateFrom={1},@DateUpto={2},@BranchID={3},@VoucherID={4}," +
                    "@UserID={5},@Detailed={6},@AutoEntry={7}",
                    criteria, dayBookDto.DateFrom, dayBookDto.DateUpto, dayBookDto.Branch.Id, dayBookDto.VoucherType.Id, dayBookDto.User.Id, dayBookDto.Detailed, false).ToList();
                return CommonResponse.Ok(dayBookData);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to fill DayBook");
                return CommonResponse.Error("Failed to fill DayBook");
            }
        }
        //fill trial balance
        public CommonResponse FillTrialBalance(TrialBalanceDto trialBalanceDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill TrialBalance");
            }

            try
            {
                string sp = "";
                if ((Page)pageId == Page.TrialBalance)
                    sp = "TrialBalanceSP";
                else if ((Page)pageId == Page.CostCentre_TrialBalance)
                    sp = "CostCentreTrialBalanceSP";
                var trialBalance = _context.TrialBalanceView.FromSqlRaw("Exec " + sp + " @DateFrom={0},@DateUpto={1},@BranchID={2},@Opening={3}," +
               "@OpeningBalance={4},@ClosingBalance={5},@TransactionType={6}",
               trialBalanceDto.DateFrom, trialBalanceDto.DateUpto, trialBalanceDto.Branch.Id, trialBalanceDto.OpeningVouchersOnly,
               trialBalanceDto.ShowOpening, trialBalanceDto.ShowClosing, trialBalanceDto.Transactions.Id).ToList();
                return CommonResponse.Ok(trialBalance);
            }
            catch
            {
                _logger.LogError("Failed to fill TrialBalance");
                return CommonResponse.Error("Failed to fill TrialBalance");
            }
        }
        //Cash or Bank book
        public CommonResponse FillCashOrBankBook(CashBankBookDto cashBank, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Cash/ Bank Book");
            }
            try
            {
                var data = _context.CashBankBookView.FromSqlRaw("Exec CashBankBookSP @DateFrom={0},@DateUpto={1},@BranchID={2},@GroupAccount={3}",
                                                            cashBank.DateFrom, cashBank.DateUpto, cashBank.Branch.Id, cashBank.CashOrBank.Value).ToList();
                return CommonResponse.Ok(data);
            }
            catch
            {
                _logger.LogError("Failed to fill Cash/ Bank Book");
                return CommonResponse.Error("Failed to fill Cash/ Bank Book");
            }
        }

        //fill Account Statement
        public CommonResponse FillAccStatement(FinStmtCommonDto commonDto, int pageId, int? voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("");
            }
            object result = "";
            try
            {
                if ((Page)pageId == Page.AccountStatement)
                {
                    result = _context.AccStatementView.FromSqlRaw("Exec AccountStatementSP @DateFrom={0},@DateUpTo={1},@AccountID={2},@VTypeID={3},@BranchID={4}," +
              "@Opening={5},@UserID={6}",
              commonDto.DateFrom, commonDto.DateUpto, commonDto.Account.Id, voucherId, commonDto.Branch.Id, true, commonDto.User.Id).ToList();

                }
                else if ((Page)pageId == Page.GroupStatement)
                {
                    result = _context.GroupStatementView.FromSqlRaw("Exec GroupSummarySP @DateFrom={0},@DateUpTo={1},@AccountID={2},@BranchID={3},@UserID={4}," +
                        "@AllGroup={5}", commonDto.DateFrom, commonDto.DateUpto, commonDto.Account.Id, commonDto.Branch.Id, commonDto.User.Id,
                        commonDto.Account.Id == null ? true : false).ToList();

                }
                return CommonResponse.Ok(result);
            }
            catch
            {
                _logger.LogError("Failed to fill Account or Group Statement");
                return CommonResponse.Error("Failed to Load");
            }
        }
        //fill billwise statement
        public CommonResponse FillBillwiseStmt(BillwiseStmtDto billwiseDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Billwise Statement");
            }
            try
            {
                if (billwiseDto.DateFrom == null || billwiseDto.DateUpto == null)
                    return CommonResponse.Ok();
                bool? receivables = billwiseDto.BillType.Value == "Receivables" ? true : false;
                bool? payables = billwiseDto.BillType.Value == "Payables" ? true : false;
                bool? EffectiveDateAsVDate = (billwiseDto.VoucherDate == true) ? false : true;
                DateTime? effDateFrom = null, effDateTo = null, dateFrom = null, dateUpto = null;
                int? accountId = null, accCatId = null, accGroup = null;
                int? dueDaysFrom = null, dueDaysUpto = null;
                if (EffectiveDateAsVDate == true)
                {
                    effDateFrom = billwiseDto.DateFrom;
                    effDateTo = billwiseDto.DateUpto;
                }
                else
                {
                    dateFrom = billwiseDto.DateFrom;
                    dateUpto = billwiseDto.DateUpto;
                }
                accountId = billwiseDto.Account.Id;
                accCatId = billwiseDto.AccCategory.Id;
                accGroup = billwiseDto.AccGroup.Id;
                dueDaysFrom = billwiseDto.DaysFrom;
                dueDaysUpto = billwiseDto.DaysUpto;
                var billwise = _context.BillwiseStatementView.FromSqlRaw("Exec BillwiseSP @EffDateFrom={0},@EffDateTo={1},@DateFrom={2},@DateUpto={3},@BranchID={4}," +
                    "@AccountID={5},@Receivables={6},@Payables={7},@Detailed={8},@PendingBill={9},@DueDaysFrom={10},@DueDaysUpto={11},@AccCategoryID={12},@AccGroup={13}",
                    effDateFrom, effDateTo, dateFrom, dateUpto, billwiseDto.Branch.Id, accountId, receivables, payables, billwiseDto.Detailed, billwiseDto.PendingBills,
                    dueDaysFrom, dueDaysUpto, accCatId, accGroup).ToList();
                return CommonResponse.Ok(billwise);
            }
            catch
            {
                _logger.LogError("Failed to fill Billwise Statement");
                return CommonResponse.Error("Failed to fill Billwise Statement");
            }
        }
        //fill balance sheet
        public CommonResponse FillBalanceSheet(BalanceSheetDto balSheetDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Balance Sheet");
            }
            try
            {
                int branchId = _authService.GetBranchId().Value;
                if ((Page)pageId == Page.BalanceSheet)
                {
                    if (balSheetDto.ViewBy.Value == "Two Sided")
                    {
                        BalanceSheetStmtView balSheet = new BalanceSheetStmtView();
                        _context.Database.GetDbConnection().Open();
                        using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                        {
                            dbCommand.CommandText = $"Exec BalanceSheetSP @DateFrom='{balSheetDto.DateFrom}',@DateUpto='{balSheetDto.DateUpto}',@BranchID='{branchId}',@TwoSided='{true}'";
                            using (var reader = dbCommand.ExecuteReader())
                            {
                                balSheet.balSheet1 = _rederToObj.Deserialize<BalSheetView1>(reader).ToList();
                                reader.NextResult();
                                balSheet.balSheet2 = _rederToObj.Deserialize<BalSheetView2>(reader).ToList();
                                reader.NextResult();
                            }
                        }
                        return CommonResponse.Ok(balSheet);
                    }

                    var defaults = _context.BalSheetView3.FromSqlRaw($"Exec BalanceSheetSP @DateFrom='{balSheetDto.DateFrom}',@DateUpto='{balSheetDto.DateUpto}',@BranchID='{branchId}',@TwoSided='{false}'").ToList();
                    return CommonResponse.Ok(defaults);
                }
                string criteria = "CostcenterwiseBalanceSheet";
                var data = _context.BalSheetView3.FromSqlRaw($"Exec BalanceSheetSP @Criteria='{criteria}', @DateFrom='{balSheetDto.DateFrom}',@DateUpto='{balSheetDto.DateUpto}',@BranchID='{balSheetDto.Branch.Id}',@CostCentreCategoryID='{balSheetDto.Category.Id}',@CostCentre='{balSheetDto.CostCentre.Id}',@CostCenterwise='{false}',@ViewCostcenter='{false}',@Detailed='{true}'").ToList();
                return CommonResponse.Ok(data);
            }
            catch
            {
                _logger.LogError("Failed to fill Balance Sheet");
                return CommonResponse.Error("Failed to fill Balance Sheet");
            }
        }


        //fills consolidated monthwise statement and payment analysis
        public CommonResponse FillConsolMonthwise(CommonDto commonDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Consolidated Monthwise statement");
            }
            int branchId = _authService.GetBranchId().Value;
            object data = null;
            try
            {

                if ((Page)pageId == Page.ConsolidatedMonthwise)
                {
                    data = _context.ConsolMonthwiseView.FromSqlRaw("exec ConsolidatedMonthwiseSP @FromDate={0},@ToDate={1},@BranchID={2}",
                    commonDto.DateFrom, commonDto.DateUpto, branchId).ToList();
                }
                else if ((Page)pageId == Page.Payment_Analysis)
                {
                    data = _context.PaymentAnalysisView.FromSqlRaw("exec PerfomanceAnalysisSP @DateFrom = {0},@DateUpto = {1},@BranchID = {2},@AccountID = {3}",
                       commonDto.DateFrom, commonDto.DateUpto, branchId, commonDto.Account.Id).ToList();
                }
                return CommonResponse.Ok(data);
            }
            catch
            {
                _logger.LogError("Failed to fill Consolidated Monthwise statement or Payment analysis");
                return CommonResponse.Error("Failed to load Statement");
            }
        }
        public CommonResponse FillPartyOutStanding(PartyOutStandingDto partyDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Party Outstanding statement");
            }
            int? AccountId = null, SalesmanId = null;
            try
            {
                AccountId = partyDto.Party.Id;
                SalesmanId = partyDto.Salesman.Id;
                var data = _context.PartyOutstandingView.FromSqlRaw("exec PartyOutstandingSP @DateFrom={0},@DateUpto={1},@BranchID={2},@Nature={3},@AccountID={4}," +
                    "@SalesManID={5}", partyDto.DateFrom, partyDto.DateUpto, partyDto.Branch.Id, partyDto.ViewBy, AccountId, SalesmanId).ToList();
                return CommonResponse.Ok(data);
            }
            catch
            {
                _logger.LogError("Failed to fill Party Outstanding statement");
                return CommonResponse.Error("Failed to fill Party Outstanding statement");
            }
        }
        //fills salesman-collection report
        public CommonResponse FillSalesmanColReport(SalesmanColDto dto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Salesman Collection Report ");
            }
            try
            {
                int branchId = _authService.GetBranchId().Value;
                string criteria = "SalesMansCollectionReport";
                SalesmanCollectionView fill = new SalesmanCollectionView();
                _context.Database.GetDbConnection().Open();
                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec SalesReportSP @Criteria='{criteria}',@DateFrom='{dto.StartDate}',@DateUpto='{dto.EndDate}',@BranchID='{branchId}',@VoucherID='{null}',@AccountID='{null}',@VoucherNo='{null}',@Detailed='{null}',@SalesManID='{null}'";
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        fill.salesManView1 = _rederToObj.Deserialize<SalesmanColView1>(reader).ToList();
                        reader.NextResult();
                        fill.salesManView2 = _rederToObj.Deserialize<SalesmanColView2>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return CommonResponse.Ok(fill);
            }
            catch
            {
                _logger.LogError("Failed to fill Salesman Collection Report ");
                return CommonResponse.Error("Failed to fill Salesman Collection Report ");
            }
        }
        //fills creditor-debitor balance statement
        public CommonResponse FillCreditDebitBal(CommonDto commonDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Creditor Debitor Balance ");
            }
            try
            {
                string criteria = "CreditorDebitorBalance";
                int branchId = _authService.GetBranchId().Value;
                var data = _context.DebitCreditView.FromSqlRaw("exec BillWiseStmtSP @Criteria={0},@AccountID={1},@BranchID={2},@DateFrom={3},@DateUpto={4}",
                    criteria, commonDto.Account.Id, branchId, commonDto.DateFrom, commonDto.DateUpto).ToList();
                return CommonResponse.Ok(data);
            }
            catch
            {
                _logger.LogError("Failed to fill Creditor Debitor Balance ");
                return CommonResponse.Error("Failed to fill Creditor Debitor Balance ");
            }
        }
        //fills profit and loss statement
        //fills cost centre wise profit and loss
        public CommonResponse FillProfitAndLoss(BalanceSheetDto balSheetDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Profit and Loss Report ");
            }
            try
            {
                if ((Page)pageId == Page.ProfitAndLoss)
                {
                    if (balSheetDto.ViewBy.Value == "Two Sided")
                    {
                        ProfitAndLossStmtView profitAndLoss = new ProfitAndLossStmtView();
                        _context.Database.GetDbConnection().Open();
                        using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                        {
                            dbCommand.CommandText = $"Exec ProfitAndLossSP @DateFrom='{balSheetDto.DateFrom}',@DateUpto='{balSheetDto.DateUpto}',@BranchID='{balSheetDto.Branch.Id}',@TwoSided='{true}'";
                            using (var reader = dbCommand.ExecuteReader())
                            {
                                profitAndLoss.profitAndLossView1 = _rederToObj.Deserialize<ProfitAndLossView1>(reader).ToList();
                                reader.NextResult();
                                profitAndLoss.profitAndLossView2 = _rederToObj.Deserialize<ProfitAndLossView2>(reader).ToList();
                                reader.NextResult();
                            }
                        }
                        return CommonResponse.Ok(profitAndLoss);
                    }

                    var data = _context.ProfitAndLossView3.FromSqlRaw($"Exec ProfitAndLossSP @DateFrom='{balSheetDto.DateFrom}',@DateUpto='{balSheetDto.DateUpto}',@BranchID='{balSheetDto.Branch.Id}',@TwoSided='{false}'").ToList();
                    return CommonResponse.Ok(data);
                }
                string criteria = "CostCentreWisePandL";
                var data1 = _context.ProfitAndLossView3.FromSqlRaw($"Exec ProfitAndLossSP @Criteria='{criteria}', @DateFrom='{balSheetDto.DateFrom}',@DateUpto='{balSheetDto.DateUpto}',@BranchID='{balSheetDto.Branch.Id}',@CostCentreCategoryID='{balSheetDto.Category.Id}',@CostCentre='{balSheetDto.CostCentre.Id}',@CostCenterwise='{false}',@ViewCostcenter='{false}',@Detailed='{true}'").ToList();
                return CommonResponse.Ok(data1);
            }
            catch
            {
                _logger.LogError("Failed to fill Profit and Loss Report ");
                return CommonResponse.Error("Failed to fill Profit and Loss Report ");
            }
        }
        //fills Cash flow statement
        public CommonResponse FillCashFlowStmt(CommonDto commonDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Cash Flow Statement ");
            }
            try
            {
                int branchId = _authService.GetBranchId().Value;
                string criteria = "Normal";
                CashFlowStmtView cashFlow = new CashFlowStmtView();
                _context.Database.GetDbConnection().Open();
                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec CashFlowStatementSP @DateFrom='{commonDto.DateFrom}',@Criteria='{criteria}',@DateUpto='{commonDto.DateUpto}',@BranchID='{branchId}'";
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        cashFlow.cashFlowView1 = _rederToObj.Deserialize<CashFlowView1>(reader).ToList();
                        reader.NextResult();
                        cashFlow.cashFlowView2 = _rederToObj.Deserialize<CashFlowView2>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return CommonResponse.Ok(cashFlow);
            }
            catch
            {
                _logger.LogError("Failed to fill Cash Flow Statement ");
                return CommonResponse.Error("Failed to fill Cash Flow Statement ");
            }
        }
        //fills Aging report
        public CommonResponse FillAgingReport(AgingReportDto agingRepDto, int pageId)
        {

            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill AgingReport ");
            }
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var accountID = agingRepDto.Account?.Id.HasValue ?? false ? agingRepDto.Account.Id.ToString() : "NULL";
                string staffID = agingRepDto.Staff?.Id.HasValue ?? false ? agingRepDto.Staff.Id.ToString() : "NULL";
                string areaID = agingRepDto.SalesArea?.Id.HasValue ?? false ? agingRepDto.SalesArea.Id.ToString() : "NULL";
                string partyCategoryID = agingRepDto.PartyCategory?.Id.HasValue ?? false ? agingRepDto.PartyCategory.Id.ToString() : "NULL";

                cmd.CommandText = $"Exec AgingSP @DateFrom='{agingRepDto.DateFrom}',@DateUpto='{agingRepDto.DateUpto}',@Branchid={agingRepDto.Branch.Id},@Nature='{agingRepDto.ViewBy}',@AccountID={accountID},@StaffID={staffID},@AreaID={areaID},@PartyCategoryID={partyCategoryID}";

                // cmd.CommandText = $"Exec AgingSP @DateFrom='{agingRepDto.DateFrom}',@DateUpto='{agingRepDto.DateUpto}',@Branchid={agingRepDto.Branch.Id},@Nature='{agingRepDto.ViewBy}',@AccountID={agingRepDto.Account.Id},@StaffID={agingRepDto.Staff.Id},@AreaID={agingRepDto.SalesArea.Id},@PartyCategoryID={agingRepDto.PartyCategory.Id}";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tb = new DataTable();
                        tb.Load(reader);

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
            catch
            {
                _logger.LogError("Failed to fill Aging Report ");
                return CommonResponse.Error("Failed to fill Aging Report ");
            }
        }

        //fills e return
        public CommonResponse FilleReturn(eReturnsDto eReturnsDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill eReturn ");
            }
            try
            {
                string viewBy = "FinanceVoucherwise";
                var data = _context.eReturnView.FromSqlRaw("exec eReturnsSP @DateFrom={0},@DateUpto={1},@Branchid={2},@ViewBy={3}," +
                    "@VATType={4},@VoucherID={5},@UserID={6}",
                    eReturnsDto.DateFrom, eReturnsDto.DateUpto, eReturnsDto.Branch.Id, viewBy, eReturnsDto.VatType.Value, eReturnsDto.Voucher.Id, eReturnsDto.User.Id).ToList();
                return CommonResponse.Ok(data);
            }
            catch
            {
                _logger.LogError("Failed to fill eReturn ");
                return CommonResponse.Error("Failed to fill eReturn ");
            }
        }
        //fills costcentre report
        public CommonResponse FillCostCentreRep(CostCentreReportDto dto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill CostCentre report ");
            }
            try
            {
                string criteria = "CostCentreReport";
                int branchId = _authService.GetBranchId().Value;

                CostCentreStmtView view = new CostCentreStmtView();
                _context.Database.GetDbConnection().Open();
                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec CostCentreSP @DateFrom='{dto.StartDate}',@Criteria='{criteria}',@DateUpto='{dto.EndDate}',@BranchID={branchId}";
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        view.costCentreView1 = _rederToObj.Deserialize<CostCentreView1>(reader).ToList();
                        reader.NextResult();
                        view.costCentreView2 = _rederToObj.Deserialize<CostCentreView2>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return CommonResponse.Ok(view);
            }
            catch
            {
                _logger.LogError("Failed to fill CostCentre report ");
                return CommonResponse.Error("Failed to fill CostCentre report ");
            }
        }
        //fills Account Breakup and CostCentre Breakup
        public CommonResponse FillAccountBreakUp(CostCentreReportDto dto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill the data ");
            }
            try
            {
                int branchId = _authService.GetBranchId().Value;
                string criteria = "";
                if ((Page)pageId == Page.AccountBreakup)
                    criteria = "ProjectwiseCashBook";
                else if ((Page)pageId == Page.CostCenter_Breakup)
                    criteria = "ProjectDetails";

                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Exec CostCentreSP @DateFrom='{dto.StartDate}',@Criteria='{criteria}',@DateUpto='{dto.EndDate}',@BranchID={branchId}";
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tb = new DataTable();
                        tb.Load(reader);

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
            catch
            {
                _logger.LogError("Failed to fill Account Breakup/CostCentre Breakup ");
                return CommonResponse.Error("Failed to fill the data ");
            }
        }
    }
}
