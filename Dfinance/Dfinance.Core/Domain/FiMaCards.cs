using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class FiMaCards
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public string Description { get; set; } = null!;

        public double? Commission { get; set; }

        public bool? Default { get; set; }
    }
}
