using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class FiAccountsList
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public int AccountId { get; set; }

        public int BranchId { get; set; }
    }
}
