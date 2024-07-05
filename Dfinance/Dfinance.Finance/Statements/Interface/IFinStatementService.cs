using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Statements.Interface
{
    public interface IFinStatementService
    {
        CommonResponse FillDayBook(DayBookDto dayBookDto, int pageId);
        CommonResponse FillTrialBalance(TrialBalanceDto trialBalanceDto, int pageId);
        CommonResponse FillCashOrBankBook(CashBankBookDto cashBank, int pageId);
        CommonResponse FillAccStatement(FinStmtCommonDto commonDto, int pageId, int? voucherId);
        CommonResponse FillBillwiseStmt(BillwiseStmtDto billwiseDto, int pageId);
        CommonResponse FillBalanceSheet(BalanceSheetDto balSheetDto, int pageId);
        CommonResponse FillSalesmanColReport(SalesmanColDto dto, int pageId);
        CommonResponse FillConsolMonthwise(CommonDto commonDto, int pageId);
        CommonResponse FillPartyOutStanding(PartyOutStandingDto partyDto, int pageId);
        CommonResponse FillCreditDebitBal(CommonDto commonDto, int pageId);
        CommonResponse FillProfitAndLoss(BalanceSheetDto balSheetDto, int pageId);
        CommonResponse FillCashFlowStmt(CommonDto commonDto, int pageId);
        CommonResponse FillAgingReport(AgingReportDto agingRepDto, int pageId);
        CommonResponse FilleReturn(eReturnsDto eReturnsDto, int pageId);
        CommonResponse FillCostCentreRep(CostCentreReportDto dto, int pageId);
        CommonResponse FillAccountBreakUp(CostCentreReportDto dto, int pageId);
    }
}
