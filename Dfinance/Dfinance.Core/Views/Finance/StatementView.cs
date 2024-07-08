namespace Dfinance.Core.Views.Finance
{
    public class CommonView1
    {
        public string? VType { get; set; }
        public DateTime? VDate { get; set; }
        public string? VNo { get; set; }        
        public string? CommonNarration { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
    }

    public class DayBookView : CommonView1
    {
        public int? ID { get; set; }
        public int? VoucherID { get; set; }        
        public int? BasicVTypeID { get; set; }
        public string? BasicVType { get; set; }        
        public string? Particulars { get; set; }        
        public string? BillDetails { get; set; }       
        public int? CreatedBy { get; set; }
        public string? Posted { get; set; }
        public string? ApprovalStatus { get; set; }
        public bool? IsAutoEntry { get; set; }
        public string? UserName { get; set; }        public string? TranType { get; set; }
    }
    public partial class CommonView2
    {
        public int? ID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
    }
    
    public class AccStatementView : CommonView1
    {
        public int? VID { get; set; }
        public int? VEID { get; set; }
        public int? AccountID { get; set; }       
        public string? RefNo { get; set; }        
        public string? Narration { get; set; }
        public string? Particulars { get; set; }        
        public decimal? RBalance { get; set; }
        public string? User { get; set; }
    }
    public class BillwiseStatementView
    {
        public int? VID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public int? VType { get; set; }
        public string? CommonNarration { get; set; }
        public DateTime? EffDate { get; set; }
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public decimal? AccBalance { get; set; }
        public string? PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DueDays { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Allocated { get; set; }
        public decimal? Balance { get; set; }
        public decimal? RBalance { get; set; }
        public string? TranType { get; set; }
        public string? ReferenceNo { get; set; }
        public string? DeliveryLocation { get; set; }
    }
    
    public class BalanceSheetStmtView
    {
        public List<BalSheetView1> balSheet1 { get; set; }
        public List<BalSheetView2> balSheet2 { get; set; }        
    }
    public partial class BalSheetCommonView
    {
        public int? ID { get; set; }
        public bool? VoucherEntry { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }

    }
    public partial class BalSheetView1 : BalSheetCommonView
    {

    }
    public partial class BalSheetView2 : BalSheetCommonView
    {

    }
    public partial class BalSheetView3 : BalSheetCommonView
    {
        public decimal? SortField { get; set; }
    }
 

    public class SalesmanCollectionView
    {
        public List<SalesmanColView1> salesManView1 { get; set; }
        public List<SalesmanColView2> salesManView2 { get; set; }
    }
    public class SalesmanColView1
    {
        public int? VID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public int? VTypeID { get; set; }
        public int? AccountID { get; set; }
        public string? AccName { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public string? TranType { get; set; }
    }
    public class SalesmanColView2
    {
        public string? Account { get; set; }
        public decimal? Sales { get; set; }
        public decimal? SalesReturn { get; set; }
    }
   
    public class ProfitAndLossStmtView
    {
        public List<ProfitAndLossView1> profitAndLossView1 { get; set; }
        public List<ProfitAndLossView2> profitAndLossView2 { get; set; }
    }
    public class ProfitAndLossBaseView
    {
        public int? ID { get; set; }
        public bool? VoucherEntry { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
        public int? PandLGroup { get; set; }

    }
    public class ProfitAndLossView1 : ProfitAndLossBaseView
    {

    }
    public class ProfitAndLossView2 : ProfitAndLossBaseView
    {

    }
    public class ProfitAndLossView3 : ProfitAndLossBaseView
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
        public decimal? SortField { get; set; }
        public bool? ShowRow { get; set; }
    }
    public class CashFlowStmtView
    {
        public List<CashFlowView1> cashFlowView1 { get; set; }
        public List<CashFlowView2> cashFlowView2 { get; set; }
    }
    public class CashFlowView1
    {
        public int? ID { get; set; }
        public bool? VoucherEntry { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
    }
    public class CashFlowView2 : CashFlowView1
    {

    }
    public class AgingReportView
    {
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }

    }
    
    public class CostCentreStmtView
    {
        public List<CostCentreView1> costCentreView1 {  get; set; }
        public List<CostCentreView2> costCentreView2 { get; set; }
    }
    public class CostCentreView1
    {
        public bool? Main { get; set; }
        public int? VID { get; set; }
        public DateTime? VDate { get; set; }
        public string? VNo { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
    }
    public class CostCentreView2
    {
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
    }
    
}
