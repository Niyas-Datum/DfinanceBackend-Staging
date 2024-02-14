using Dfinance.Application.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IAccountList
    {
        CommonResponse SaveAccountsList(List<AccountsListDto> accountList);
        CommonResponse FillAccountList(int ListId);
        CommonResponse FillAccountListByID(int Id);
        CommonResponse AccountListPopUp();
        
    }
}
