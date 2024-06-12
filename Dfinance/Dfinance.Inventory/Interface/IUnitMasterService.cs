using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Interface
{
    public interface IUnitMasterService
    {
        CommonResponse FillMaster();
        CommonResponse FillByUnit(string unit);
        CommonResponse UnitDropDown();
        CommonResponse SaveUnitMaster(UnitMasterDto unitmasterDto);
        CommonResponse UpdateUnitMaster(UnitMasterDto unitmasterDto);
        CommonResponse DeleteUnitMaster(string unit);
        CommonResponse UnitPopup();
    }
}
