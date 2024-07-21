using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Statements.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [Authorize]
    [ApiController]
    public class FinStatementController : BaseController
    {
        private readonly IFinStatementService _finStmt;
        public FinStatementController(IFinStatementService finStmt)
        {            
            _finStmt = finStmt;
        }
        [HttpPost(FinRoute.FinStmt.finStmt)]
        public IActionResult FillFinStatements(string jsonDto, int pageId)
        {
            try
            {
                var result = _finStmt.FillFinStatements(jsonDto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost(FinRoute.FinStmt.accStmt)]
        public IActionResult FillAccStatement(FinStmtCommonDto commonDto, int pageId, int? voucherId)
        {
            try
            {
                var result = _finStmt.FillAccStatement(commonDto, pageId,voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.FinStmt.billWiseStmt)]
        public IActionResult FillBillwiseStatement(BillwiseStmtDto billWiseDto, int pageId)
        {
            try
            {
                var result = _finStmt.FillBillwiseStmt(billWiseDto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.FinStmt.balSheetStmt)]
        public IActionResult FillBalanceSheet(BalanceSheetDto balSheetDto, int pageId)
        {
            try
            {
                var result = _finStmt.FillBalanceSheet(balSheetDto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        
        
            [HttpPost(FinRoute.FinStmt.salesManCol)]
        public IActionResult FillSalesmanColReport(SalesmanColDto Dto, int pageId)
        {
            try
            {
                var result = _finStmt.FillSalesmanColReport(Dto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost(FinRoute.FinStmt.profitLoss)]
        public IActionResult FillProfitAndLoss(BalanceSheetDto balSheetDto, int pageId)
        {
            try
            {
                var result = _finStmt.FillProfitAndLoss(balSheetDto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.FinStmt.cashFlow)]
        public IActionResult FillCashFlowStmt(CommonDto commonDto, int pageId)
        {
            try
            {
                var result = _finStmt.FillCashFlowStmt(commonDto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPost(FinRoute.FinStmt.costCentrRep)]
        public IActionResult FillCostCentreRep(CostCentreReportDto dto, int pageId)
        {
            try
            {
                var result = _finStmt.FillCostCentreRep(dto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }       
    }
}
