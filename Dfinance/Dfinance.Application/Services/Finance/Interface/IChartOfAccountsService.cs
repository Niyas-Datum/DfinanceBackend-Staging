using Dfinance.Application.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IChartOfAccountsService
    {
        CommonResponse FillAccounts(int parentId ,bool tree);
        CommonResponse FillAccountsById(int Id);
        CommonResponse DropdownAccounts();
        CommonResponse DropDownSubGroup(int id,string Keyword="");
        CommonResponse DropDownAccountCategory();
        CommonResponse SaveAccount(ChartOfAccountsDto accountsDto);
        CommonResponse UpdateAccounts(ChartOfAccountsDto accountsDto,int Id);
        CommonResponse DeleteAccount(int Id);




    }
}
