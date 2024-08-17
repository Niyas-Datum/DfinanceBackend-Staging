using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;

namespace Dfinance.Item.Services.Interface
{
    public interface IInternationalBarCodeService
    {
        CommonResponse FillInternBarCode();
        CommonResponse SaveUpdateIntBarcCode(List<IntnBarCodeDto> intnBarCodeDto);
    }
}
