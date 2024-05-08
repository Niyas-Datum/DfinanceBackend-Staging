using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
namespace Dfinance.Warehouse.Services.Interface
{
    public interface IWarehouseService
    {
        CommonResponse WarehouseDropdown();
        CommonResponse WarehouseDropdownUsingBranch();
        CommonResponse BranchWiseWarehouseFill();
        CommonResponse WarehouseFillMaster();
        CommonResponse WarehouseFillById(int Id);
        CommonResponse Save(WarehouseDto warehouseDto);
        CommonResponse Delete(int Id);
    }
}
