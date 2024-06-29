using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Finance
{
    public class BranchAccountsDto
    {        
        public int AccountID { get; set; }
        public List <int>?   BranchID { get; set; }
        public  byte IsBit {  get; set; }
    }
}
