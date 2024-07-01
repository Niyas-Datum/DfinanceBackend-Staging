using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class FiMaBranchAccounts
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int BranchId { get; set; }
        public virtual FiMaAccount Account { get; set; } = null!;
        public virtual MaCompany Branch { get; set; } = null!;
    }
}
