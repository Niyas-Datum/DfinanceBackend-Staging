using Dfinance.Application.Dto.Common;

namespace Dfinance.Application.Dto.Finance
{
    public class AccountsListDto
    {
        public int? Id{ get; set; }
       public DropDownDesc? List { get; set; }
        public List<AccountNamePopUpDto> Accounts { get; set; }
       
    
    }
    
}
