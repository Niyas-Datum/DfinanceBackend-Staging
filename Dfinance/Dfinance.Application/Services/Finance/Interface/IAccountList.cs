using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IAccountListService
    {        
        CommonResponse SaveAccountsList(AccountsListDto accountList);        
        CommonResponse FillAccountList(int ListId);
        CommonResponse FillAccountListByID(int Id);
        CommonResponse AccountListPopUp();       
    }
}
