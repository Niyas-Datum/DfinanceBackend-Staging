using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Inventory.Interface
{
    public interface IAreaMasterService
    {
        CommonResponse PopArea();
        CommonResponse SaveAreaMaster(MaAreaDto maAreaDto);
        CommonResponse UpdateAreaMaster(MaAreaDto maAreaDto, int Id);
        CommonResponse DeleteAreaMaster(int Id);
        CommonResponse FillAreaMaster();
        CommonResponse FillAreaMasterById(int Id);
    }
}
