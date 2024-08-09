using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Item.Services.Interface
{
    public interface IItemMappingService
    {
        CommonResponse FillMasterItems();
        CommonResponse FillItemDetails(int itemId);
        CommonResponse SaveItemMapping(ItemMappingDto itemMappingDto, int pageId);
    }
}
