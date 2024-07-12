using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class FiTransactionEntry
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string DrCr { get; set; } = null!;
        public string? Nature { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal? Fcamount { get; set; }
        public DateTime? BankDate { get; set; }
        public int? RefPageTypeId { get; set; }
        public int? RefPageTableId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Description { get; set; }
        public string? TranType { get; set; }
        public DateTime? DueDate { get; set; }
        public int? RefTransId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? RefTransactionId { get; set; }
        public decimal? ExchRate { get; set; }
        public decimal? TaxPerc { get; set; }

        public virtual FiMaAccount Account { get; set; } = null!;
        public virtual Currency? Currency { get; set; }
        //public virtual MaPageType? RefPageType { get; set; }
        public virtual FiTransactionEntry? RefTrans { get; set; }
        public virtual FiTransaction Transaction { get; set; } = null!;
        //public virtual ICollection<FiCheque> FiCheques { get; set; }
       // public virtual ICollection<FiChequesTran> FiChequesTrans { get; set; }
        public virtual ICollection<FiVoucherAllocation> FiVoucherAllocations { get; set; }
       // public virtual ICollection<InvTransItem> InvTransItems { get; set; }
        public virtual ICollection<FiTransactionEntry> InverseRefTrans { get; set; }
        public virtual ICollection<TransCollection> TransCollections { get; set; }
        public virtual ICollection<TransCostAllocation> TransCostAllocations { get; set; }
        public virtual ICollection<TransExpense> TransExpenses { get; set; }
       // public virtual ICollection<TransItemExpense> TransItemExpenses { get; set; }
       // public virtual ICollection<VmfuelLog> VmfuelLogs { get; set; }
    }
}
