

namespace Dfinance.Core.Views.Finance
{

    public class ChartofAccView
    {

        public int ID { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string? Narration { get; set; }
        public bool IsGroup { get; set; }
        public int? SubGroup { get; set; }
        public int? AccountCategory { get; set; }
        public int? Parent { get; set; }
        public int CreatedBy { get; set; }
        public int? CreatedBranchID { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsBillWise { get; set; }
        public bool? Active { get; set; }
        public string Alias { get; set; }
        public int? AccountTypeID { get; set; }
        public int? SortField { get; set; }
        public int? Level { get; set; }
        public bool FinalAccount { get; set; }
        public int AccountGroup { get; set; }
        public bool? SystemAccount { get; set; }
        public bool? PreventExtraPay { get; set; }
        public string? AlternateName { get; set; }


    }



    public class ChartofAccViewById
    {
        public int ID { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string? Narration { get; set; }
        public bool IsGroup { get; set; }
        public int? SubGroup { get; set; }
        public int? AccountCategory { get; set; }
        public int? Parent { get; set; }
        public int CreatedBy { get; set; }
        public int? CreatedBranchID { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsBillWise { get; set; }
        public bool? Active { get; set; }
        public string Alias { get; set; }
        public int? AccountTypeID { get; set; }
        public int? SortField { get; set; }
        public int? Level { get; set; }
        public bool FinalAccount { get; set; }
        public int AccountGroup { get; set; }

        public bool? PreventExtraPay { get; set; }
        public string? AlternateName { get; set; }

    }
	//page: Ledger -> list all accounts
    public class FillLedgers
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

    }
}

