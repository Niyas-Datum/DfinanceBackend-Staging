namespace Dfinance.Core.Views.Finance
{
    public partial class BudgetRegPandLView : BudRegBalSheetCommon

    {  
        public int? ParentID { get; set; }
        public int? AccountGroupID { get; set; }
        public int? SubGroupOrderNo { get; set; }
        public int? ParentLevel { get; set; }       
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string? GroupType { get; set; }
        public bool? ShowChild { get; set; }
        public int? AOrderNo { get; set; }
        public bool? ShowRow { get; set; }
        public int? PandLGroup { get; set; }
       
    }

    public partial class BalanceSheetView: BudRegBalSheetCommon
    {

    }
    public partial class BudRegBalSheetCommon
    {       
        public int? ID { get; set; }
        public bool? VoucherEntry { get; set; }
        public decimal? SortField { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BudgetAmount { get; set; }
    }
    public partial class MonthwiseCommonView
    {
        public bool? VoucherEntry { get; set; }
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public int? PandLGroup { get; set; }
        public decimal? SortField { get; set; }
        public decimal? BudgetAmount { get; set; }
        public decimal? January { get; set; }
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
        public decimal? Jan_Budget { get; set; }
        public decimal? Jan_Diff { get; set; }
        public decimal? Feb_Budget { get; set; }
        public decimal? Feb_Diff { get; set; }
        public decimal? Mar_Budget { get; set; }
        public decimal? Mar_Diff { get; set; }
        public decimal? Apr_Budget { get; set; }
        public decimal? May_Budget { get; set; }
        public decimal? Jun_Budget { get; set; }
        public decimal? Jul_Budget { get; set; }
        public decimal? Aug_Budget { get; set; }
        public decimal? Sep_Budget { get; set; }
        public decimal? Oct_Budget { get; set; }
        public decimal? Nov_Budget { get; set; }
        public decimal? Dec_Budget { get; set; }
        public decimal? Dec_Diff { get; set; }
        public decimal? Apr_Diff { get; set; }
        public decimal? May_Diff { get; set; }
        public decimal? Jun_Diff { get; set; }
        public decimal? Jul_Diff { get; set; }
        public decimal? Aug_Diff { get; set; }
        public decimal? Sep_Diff { get; set; }
        public decimal? Oct_Diff { get; set; }
        public decimal? Nov_Diff { get; set; }
    }
    public partial class MonthwiseBalSheetView:MonthwiseCommonView
    {
        
    }
    public partial class MonthwisePandLView:MonthwiseCommonView
    {
        public decimal ActualAmount {  get; set; }
        public int? PandLGroup { get; set; }
        public decimal? SortField { get; set; }
        public decimal? BudgetAmount { get; set; }
    }
}
