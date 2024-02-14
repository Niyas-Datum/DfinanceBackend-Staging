using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    
    public class FillAccountList
    {
        public int? ID { get; set; }
        public int ListID { get; set; }
        public int AccountID { get; set; }
        public int BranchID { get; set; }
        public string? Alias { get; set; }
        public string? Name { get; set; }
             
             
    }
  
    public class FillMaster
    { 
        public int ID { get; set; }
        public string? AccountName { get; set; }
        public string? Description { get; set; } 

    }
}
