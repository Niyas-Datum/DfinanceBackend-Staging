using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;


namespace Dfinance.Application.Services.General.Interface
{
    public interface ICostCategoryService
    {
        CommonResponse SaveCostCategory(CostCategoryDto costCategoryDto);
        CommonResponse UpdateCostCategory(CostCategoryDto costCategoryDto, int Id);
        CommonResponse DeleteCostCategory(int Id);
        CommonResponse FillCostCategory();
        CommonResponse FillCostCategoryById(int Id);
        CommonResponse FillCostCategoryDropDown();
    }
}
