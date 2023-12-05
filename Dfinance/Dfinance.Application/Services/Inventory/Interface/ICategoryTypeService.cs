using Dfinance.Application.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Inventory.Interface
{
    public interface ICategoryTypeService
    {
        CommonResponse SaveCategoryType(CategoryTypeDto categoryTypeDto);

        CommonResponse GetNextCode();

        CommonResponse FillCategoryType();

        CommonResponse FillCategoryTypeById(int Id);

        CommonResponse UpdateCategoryType(CategoryTypeDto categoryTypeDto, int Id);

        CommonResponse DeleteCategoryType(int Id);

        CommonResponse CategoryTypePopUp();
    }
}
