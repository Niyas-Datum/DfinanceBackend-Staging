using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Finance.Services.Interface
{
    public interface IBudgetingService
    {
        CommonResponse FillMaster(int? TransId, int? PageId, int? voucherId);
        //CommonResponse AccountPopup();
        CommonResponse FillProfitLossBalsheet(bool? profitLoss, bool? balsheet);
        CommonResponse SaveBudget(BudgetingDto budgetDto, int pageId, int voucherId);
        CommonResponse DeleteBudget(BudgetingDto budgetDto, int pageId, int voucherId, bool? cancel = false);
    }
}
