using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface  ICardMaster
    {
        CommonResponse SaveCardMaster(CardMasterDto cardMaster); 
        CommonResponse UpdateCardMaster(CardMasterDto cardMaster,int Id);
        CommonResponse DeleteCardMaster(int Id);
        CommonResponse FillCardMaster (int Id);
        CommonResponse FillMaster();
    }
}
