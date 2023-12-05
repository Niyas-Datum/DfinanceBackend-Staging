using Dfinance.Shared.Domain;
using Dfinance.Application.Dto.Inventory;

namespace Dfinance.Application.Services.Inventory.Interface
{
    public interface IAreaMasterService
    {
        CommonResponse SaveAreaMaster(MaAreaDto maAreaDto);
        CommonResponse UpdateAreaMaster(MaAreaDto maAreaDto, int Id);
        CommonResponse DeleteAreaMaster(int Id);
        CommonResponse FillAreaMaster();
        CommonResponse FillAreaMasterById(int Id);
    }
}
