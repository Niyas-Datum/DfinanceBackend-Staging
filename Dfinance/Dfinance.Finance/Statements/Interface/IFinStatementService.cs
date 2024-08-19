using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Finance.Statements.Interface
{
    public interface IFinStatementService
    {
        CommonResponse FillFinStatements(string jsonDto, int pageId);        
        CommonResponse FillAccStatement(FinStmtCommonDto commonDto, int pageId, int? voucherId);
        CommonResponse FillBillwiseStmt(BillwiseStmtDto billwiseDto, int pageId);
        CommonResponse FillBalanceSheet(BalanceSheetDto balSheetDto, int pageId);
        CommonResponse FillSalesmanColReport(SalesmanColDto dto, int pageId);       
        CommonResponse FillProfitAndLoss(BalanceSheetDto balSheetDto, int pageId);
        CommonResponse FillCashFlowStmt(CommonDto commonDto, int pageId);       
        CommonResponse FillCostCentreRep(CostCentreReportDto dto, int pageId);
        CommonResponse VatComputation(DateTime dateFrom, DateTime dateUpto, int pageId);
    }
}
