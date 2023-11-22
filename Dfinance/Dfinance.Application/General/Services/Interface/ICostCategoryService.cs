using Dfinance.Application.Dto.General;
using Dfinance.Shared.Domain;


namespace Dfinance.Application.General.Services.Interface
{
    public interface ICostCategoryService
    {
        CommonResponse SaveCostCategory(CostCategoryDto costCategoryDto);
        CommonResponse UpdateCostCategory(CostCategoryDto costCategoryDto, int Id);
        CommonResponse DeleteCostCategory(int Id);
        CommonResponse FillCostCategory();
        CommonResponse FillCostCategoryById(int Id);
    }
}
