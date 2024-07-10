using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.ChartOfAccount.Services.Finance.Interface
{
    public interface IChartOfAccountsService
    {
        CommonResponse FillAccounts(int parentId ,bool tree);
        CommonResponse DropdownAccount();
        CommonResponse FillAccountsById(int Id);
        CommonResponse DropdownAccounts();
        CommonResponse DropDownSubGroup(int id,string Keyword="");
        CommonResponse DropDownAccountCategory();
        CommonResponse SaveAccount(ChartOfAccountsDto accountsDto);
        CommonResponse UpdateAccounts(ChartOfAccountsDto accountsDto,int Id);
        CommonResponse DeleteAccount(int Id);
        CommonResponse AccountPopUp(int? pageId = null);
        CommonResponse FillAccountGroup();


    }
}
