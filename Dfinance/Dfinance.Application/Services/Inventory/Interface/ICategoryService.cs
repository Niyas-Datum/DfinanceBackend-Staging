using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Inventory.Interface
{
    public interface ICategoryService
    {
        CommonResponse SaveCategory(CategoryDto categoryDto);

        CommonResponse UpdateCategory(CategoryDto categoryDto, int Id);

        CommonResponse DeleteCategory(int Id);

        CommonResponse FillCategory();

        CommonResponse FillCategoryById(int Id);

        CommonResponse GetNextCode();
    }
}
