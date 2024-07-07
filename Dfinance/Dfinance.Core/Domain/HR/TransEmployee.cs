using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TransEmployee
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int HremployeeId { get; set; }
        public string? Note { get; set; }
        public int? TypeId { get; set; }

        public virtual HREmployee Hremployee { get; set; } = null!;
        public virtual FiTransaction Transaction { get; set; } = null!;
    }
}
