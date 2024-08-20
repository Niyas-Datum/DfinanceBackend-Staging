using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Inventory.Interface
{
    public interface IDosageMasterService
    {
        CommonResponse FillMasterAndById(int? Id);
        CommonResponse SaveUpdateDosage(DosageDto dosageDto, int PageId);
        CommonResponse DeleteDosage(int Id, int PageId);
    }
}
