using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Item.Services.Interface
{
    public interface IPriceCategoryService
    {
        CommonResponse FillMaster();
        CommonResponse FillPriceCategoryById(int Id);
        CommonResponse SavePriceCategory(PriceCategoryDto priceCategory, int PageId);
        CommonResponse UpdatePriceCategory(PriceCategoryDto priceCategory, int PageId);
        CommonResponse DeletePriceCategory(int id, int PageId);

    }
}
