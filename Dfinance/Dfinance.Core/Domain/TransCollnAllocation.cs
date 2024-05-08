using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TransCollnAllocation
    {
        public int Id { get; set; }
        public int? TransCollectionId { get; set; }
        /// <summary>
        /// VoucherAllocation ID
        /// </summary>
        public int? VallocationId { get; set; }
        /// <summary>
        /// Amount allocated to this collection
        /// </summary>
        public decimal? Amount { get; set; }
        public string? Description { get; set; }

        public virtual TransCollection? TransCollection { get; set; }
        public virtual FiVoucherAllocation? Vallocation { get; set; }
    }
}
