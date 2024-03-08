using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Item.Services.Inventory.Interface
{
    public interface IItemUnitsService
    {
        CommonResponse SaveItemUnits(ItemUnitsDto units, int branch, int ItemId);
        CommonResponse UpdateItemUnits(ItemUnitsDto units, int ItemId, int branch);
        CommonResponse DeleteItemUnits(int UnitId);
        CommonResponse FillItemUnits(int ItemId, int BranchId);
    }
}
