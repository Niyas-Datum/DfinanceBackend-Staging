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
using System.Text.Json;

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

        public CommonResponse FillFinStatements(string jsonDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill the Statement");
            }
            try
            {
                DayBookDto dayBookDto;
                TrialBalanceDto trialBalanceDto;
                CommonDto commonDto;
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                _context.Database.GetDbConnection().Open();
                object userId = null, branchId = null, voucherId = null;
                object detailed = null;

                //fills DayBook
                if ((Page)pageId == Page.DayBook)
                {
                    dayBookDto = JsonSerializer.Deserialize<DayBookDto>(jsonDto);
                    userId = dayBookDto.User?.Id.HasValue ?? false ? dayBookDto.User.Id.ToString() : "NULL";
                    branchId = dayBookDto.Branch?.Id.HasValue ?? false ? dayBookDto.Branch.Id.ToString() : "NULL";
                    voucherId = dayBookDto.VoucherType?.Id.HasValue ?? false ? dayBookDto.VoucherType.Id.ToString() : "NULL";
                    detailed = dayBookDto.Detailed == true ? 1 : 0;
                    string criteria = "FillFinanceVoucherSummary";
                    cmd.CommandText = $"Exec FinTransactionsSP @Criteria='{criteria}',@DateFrom='{dayBookDto.DateFrom}',@DateUpto='{dayBookDto.DateUpto}',@BranchID={branchId},@VoucherID={voucherId},@UserID={userId},@Detailed={detailed},@AutoEntry='{false}'";
                }

                //trial balance and CostCentre trial balance
                else if ((Page)pageId == Page.TrialBalance || (Page)pageId == Page.CostCentre_TrialBalance)
                {
                    trialBalanceDto = JsonSerializer.Deserialize<TrialBalanceDto>(jsonDto);
                    string sp = "";
                    object openig = trialBalanceDto.OpeningVouchersOnly == true ? 1 : 0;
                    object showOpening = trialBalanceDto.ShowOpening == true ? 1 : 0;
                    object showClosing = trialBalanceDto.ShowClosing == true ? 1 : 0;
                    object transaction = trialBalanceDto.Transactions?.Id.HasValue ?? false ? trialBalanceDto.Transactions.Id.ToString() : "NULL";
                    if (((Page)pageId == Page.TrialBalance))
                        sp = "TrialBalanceSP";
                    else
                        sp = "CostCentreTrialBalanceSP";

                    branchId = trialBalanceDto.Branch?.Id.HasValue ?? false ? trialBalanceDto.Branch.Id.ToString() : "NULL";
                    cmd.CommandText = $"Exec {sp} @DateFrom='{trialBalanceDto.DateFrom}',@DateUpto='{trialBalanceDto.DateUpto}',@BranchID={branchId},@Opening={openig},@OpeningBalance={showOpening},@ClosingBalance={showClosing},@TransactionType={transaction}";
                }

                //Cash or Bank book
                else if ((Page)pageId == Page.CashOrBank_Book)
                {
                    CashBankBookDto cashBankDto;
                    cashBankDto = JsonSerializer.Deserialize<CashBankBookDto>(jsonDto);
                    branchId = cashBankDto.Branch?.Id.HasValue ?? false ? cashBankDto.Branch.Id.ToString() : "NULL";
                    cmd.CommandText = $"Exec CashBankBookSP @DateFrom='{cashBankDto.DateFrom}',@DateUpto='{cashBankDto.DateUpto}',@BranchID={branchId},@GroupAccount='{cashBankDto.CashOrBank.Value}'";
                }

                //Consolidated monthwise
                else if ((Page)pageId == Page.ConsolidatedMonthwise)
                {
                    commonDto = JsonSerializer.Deserialize<CommonDto>(jsonDto);
                    branchId = _authService.GetBranchId().Value;
                    cmd.CommandText = $"Exec ConsolidatedMonthwiseSP @FromDate='{commonDto.DateFrom}',@ToDate='{commonDto.DateUpto}',@BranchID={branchId}";
                }
                //payment analysis
                else if ((Page)pageId == Page.Payment_Analysis)
                {
                    commonDto = JsonSerializer.Deserialize<CommonDto>(jsonDto);
                    object accountId = commonDto.Account?.Id.HasValue ?? false ? commonDto.Account.Id.ToString() : "NULL";
                    branchId = _authService.GetBranchId().Value;
                    cmd.CommandText = $"Exec PerfomanceAnalysisSP @DateFrom ='{commonDto.DateFrom}',@DateUpto='{commonDto.DateUpto}',@BranchID = {branchId},@AccountID={accountId}";
                }
                //party outstanding
                else if ((Page)pageId == Page.PartyOutStanding)
                {
                    PartyOutStandingDto partyDto;
                    partyDto = JsonSerializer.Deserialize<PartyOutStandingDto>(jsonDto);
                    object AccountId = partyDto.Party?.Id.HasValue ?? false ? partyDto.Party.Id.ToString() : "NULL";
                    object SalesmanId = partyDto.Salesman?.Id.HasValue ?? false ? partyDto.Salesman.Id.ToString() : "NULL";
                    branchId = partyDto.Branch?.Id.HasValue ?? false ? partyDto.Branch.Id.ToString() : "NULL";
                    cmd.CommandText = $"Exec PartyOutstandingSP @DateFrom='{partyDto.DateFrom}',@DateUpto='{partyDto.DateUpto}',@BranchID={branchId},@Nature='{partyDto.ViewBy}',@AccountID={AccountId},@SalesManID={SalesmanId}";

                }
                //Debitor creditor balance pageId=480
                else if ((Page)pageId == Page.Creditor_Debitor_Balance)
                {
                    commonDto = JsonSerializer.Deserialize<CommonDto>(jsonDto);
                    branchId = _authService.GetBranchId().Value;
                    object accountId = commonDto.Account?.Id.HasValue ?? false ? commonDto.Account.Id.ToString() : "NULL";
                    string criteria = "CreditorDebitorBalance";
                    cmd.CommandText = $"Exec BillWiseStmtSP @Criteria='{criteria}',@AccountID={accountId},@BranchID={branchId},@DateFrom='{commonDto.DateFrom}',@DateUpto='{commonDto.DateUpto}'";
                }
                //aging report
                else if ((Page)pageId == Page.Aging_Report)
                {
                    AgingReportDto agingRepDto;
                    agingRepDto = JsonSerializer.Deserialize<AgingReportDto>(jsonDto);
                    var accountID = agingRepDto.Account?.Id.HasValue ?? false ? agingRepDto.Account.Id.ToString() : "NULL";
                    string staffID = agingRepDto.Staff?.Id.HasValue ?? false ? agingRepDto.Staff.Id.ToString() : "NULL";
                    string areaID = agingRepDto.SalesArea?.Id.HasValue ?? false ? agingRepDto.SalesArea.Id.ToString() : "NULL";
                    string partyCategoryID = agingRepDto.PartyCategory?.Id.HasValue ?? false ? agingRepDto.PartyCategory.Id.ToString() : "NULL";
                    cmd.CommandText = $"Exec AgingSP @DateFrom='{agingRepDto.DateFrom}',@DateUpto='{agingRepDto.DateUpto}',@Branchid={agingRepDto.Branch.Id},@Nature='{agingRepDto.ViewBy}',@AccountID={accountID},@StaffID={staffID},@AreaID={areaID},@PartyCategoryID={partyCategoryID}";

                }
                //e-Return
                else if ((Page)pageId == Page.eReturn)
                {
                    eReturnsDto eReturnsDto;
                    eReturnsDto = JsonSerializer.Deserialize<eReturnsDto>(jsonDto);
                    string viewBy = "FinanceVoucherwise";
                    branchId = eReturnsDto.Branch?.Id.HasValue ?? false ? eReturnsDto.Branch.Id.ToString() : "NULL";
                    cmd.CommandText = $"Exec eReturnsSP @DateFrom='{eReturnsDto.DateFrom}',@DateUpto='{eReturnsDto.DateUpto}',@Branchid={branchId},@ViewBy='{viewBy}'";
                }
                //Account breakup and CostCentre breakup
                else if ((Page)pageId == Page.AccountBreakup || (Page)pageId == Page.CostCenter_Breakup)
                {
                    CostCentreReportDto dto;
                    dto = JsonSerializer.Deserialize<CostCentreReportDto>(jsonDto);
                    string criteria = "";
                    branchId = _authService.GetBranchId().Value;
                    if ((Page)pageId == Page.AccountBreakup)
                        criteria = "ProjectwiseCashBook";
                    else if ((Page)pageId == Page.CostCenter_Breakup)
                        criteria = "ProjectDetails";
                    cmd.CommandText = $"Exec CostCentreSP @DateFrom='{dto.StartDate}',@Criteria='{criteria}',@DateUpto='{dto.EndDate}',@BranchID={branchId}";
                }

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
                    return CommonResponse.NoContent("No Data");
                }
            }
            catch
            {
                _logger.LogError("Failed to fill Finance Statement");
                return CommonResponse.Error("Failed to Load");
            }
        }

        //fill Account Statement, CostCentre account statement and group statement
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
                if ((Page)pageId == Page.AccountStatement || (Page)pageId == Page.CostCentre_AccountStatement)
                {
                    if (commonDto.CostCentre.Id == null)
                    {
                        result = _context.AccStatementView.FromSqlRaw("Exec AccountStatementSP @DateFrom={0},@DateUpTo={1},@AccountID={2},@VTypeID={3},@BranchID={4}," +
              "@Opening={5},@UserID={6}", commonDto.DateFrom, commonDto.DateUpto, commonDto.Account.Id, voucherId, commonDto.Branch.Id, true, commonDto.User.Id).ToList();

                    }
                    else
                    {
                        result = _context.AccStatementView.FromSqlRaw("Exec CostCentreAccountStatementSP @DateFrom={0},@DateUpTo={1},@AccountID={2},@VTypeID={3},@BranchID={4}," +
              "@Opening={5},@CostCenterID={6}", commonDto.DateFrom, commonDto.DateUpto, commonDto.Account.Id, voucherId, commonDto.Branch.Id, true, commonDto.CostCentre.Id).ToList();
                    }

                }
                else if ((Page)pageId == Page.GroupStatement)
                {
                    var cmd = _context.Database.GetDbConnection().CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    bool allGroup = commonDto.Account.Id == null ? true : false;
                    var accountID = commonDto.Account?.Id.HasValue ?? false ? commonDto.Account.Id.ToString() : "NULL";
                    var branchID = commonDto.Account?.Id.HasValue ?? false ? commonDto.Account.Id.ToString() : "NULL";
                    var userId = commonDto.Account?.Id.HasValue ?? false ? commonDto.Account.Id.ToString() : "NULL";
                    cmd.CommandText = $"Exec GroupSummarySP @DateFrom='{commonDto.DateFrom}',@DateUpTo='{commonDto.DateUpto}',@AccountID={accountID},@BranchID={branchID},@UserID={userId},@AllGroup='{allGroup}'";
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

        //VAT computation
        public CommonResponse VatComputation(DateTime dateFrom,DateTime dateUpto,int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill VatComputation report ");
            }
            int branchId = _authService.GetBranchId().Value;
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            _context.Database.GetDbConnection().Open();
            cmd.CommandText = $"Exec VATComputationSP @DateFrom='{dateFrom}',@DateUpto='{dateUpto}',@BranchID={branchId}";
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
                return CommonResponse.NoContent("No Data");
            }
        }
      
    }
}
