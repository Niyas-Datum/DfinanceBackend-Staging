
using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Finance
{
    public class AccountsListDto
    {
        public int? Id{ get; set; }
       public DropDownDesc? List { get; set; }
        public List<AccountNamePopUpDto>? Accounts { get; set; }
       
    
    }
    
}
