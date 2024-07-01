using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Finance
{
    public class BudgetingDto
    {
        public int? TransactionId { get; set; }
        public string VoucherNo {  get; set; }//fitransaction
        public DateTime VoucherDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Name { get; set;}
        public DropdownDto Type { get; set;}
        public string? Narration {  get; set;}
        public bool? ProfitAndLoss { get; set;}
        public bool? BalanceSheet { get; set;}
        public List<BudgetAccount>? BudgetAccounts { get; set;}

    }
    public class BudgetAccount
    {
        public AccountDto Account { get; set; }     
        public decimal? Amount { get; set; }
        public decimal? January { get; set;}
        public decimal? February { get; set; }
        public decimal? March { get; set; }
        public decimal? April { get; set; }
        public decimal? May { get; set; }
        public decimal? June { get; set; }
        public decimal? July { get; set; }
        public decimal? August { get; set; }    
        public decimal? September { get; set; }
        public decimal? October { get; set; }
        public decimal? November { get; set; }
        public decimal? December { get; set; }
    }
}
