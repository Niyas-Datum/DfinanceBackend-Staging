using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Warehouse;
using Dfinance.Shared.Domain;
namespace Dfinance.Warehouse.Services.Interface
{
    public interface IWarehouseService
    {
        CommonResponse WarehouseDropdown();
        CommonResponse WarehouseDropdownUsingBranch(int createdBranchId);
        CommonResponse BranchWiseWarehouseFill();
        CommonResponse WarehouseFillMaster();
        CommonResponse WarehouseFillById(int Id);
        CommonResponse Save(WarehouseDto warehouseDto);
        CommonResponse Delete(int Id);
        CommonResponse FillLocationMaster();
        CommonResponse FillLocationById(int Id);
        CommonResponse SaveLocation(LocationTypeDto LocationTypeDto, int PageId);
        CommonResponse DeleteLocationType(int Id, int PageId);
    }
}
