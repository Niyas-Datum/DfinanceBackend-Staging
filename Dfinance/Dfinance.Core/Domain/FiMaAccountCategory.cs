using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class FiMaAccountCategory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<FiMaAccount> FiMaAccounts { get; set; } = new List<FiMaAccount>();
    }
}
