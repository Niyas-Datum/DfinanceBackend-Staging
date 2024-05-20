
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class MaMisc
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public bool? AllowDelete { get; set; }
        public int? DevCode { get; set; }
        public string? Code { get; set; }
        public string Key { get; set; }

        //relationships
        public ICollection<ItemMaster> Items { get; set; }
        public virtual ICollection<FiTransaction> FiTransactions { get; set; }
        public virtual ICollection<FiTransactionAdditionals> FiTransactionAdditionalCountryOfOrigins { get; set; }
        public virtual ICollection<FiTransactionAdditionals> FiTransactionAdditionalModes { get; set; }
        public virtual ICollection<FiTransactionAdditionals> FiTransactionAdditionalTypes { get; set; }
    }
}
