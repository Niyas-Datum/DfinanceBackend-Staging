using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IFinanceYearService
    {
        CommonResponse FillAllFinanceYear();
        CommonResponse FillFinanceYearById(int Id);
        CommonResponse SaveFinanceYear(FinanceYearDto financeYearDto);
        CommonResponse UpdateFinanceYear(FinanceYearDto financeYearDto, int Id);
        CommonResponse DeleteFinanceYear(int Id);
    }
}
