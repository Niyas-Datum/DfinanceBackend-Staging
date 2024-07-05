using System.Diagnostics.Contracts;

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
    public partial class TrialBalanceView : CommonView2
    {       
        public bool? VoucherEntry { get; set; }        
        public decimal? OpenDebit { get; set; }
        public decimal? OpenCredit { get; set; }        
        public decimal? ClosDebit { get; set; }
        public decimal? ClosCredit { get; set; }
    }
    public partial class CashBankBookView : CommonView2
    {       
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
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
    public class GroupStatementView
    {
        public int? ID { get; set; }
        public int? ParentID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public int? ParentLevel { get; set; }
        public int? VoucherEntry { get; set; }
        public int? SortField { get; set; }
        public decimal? PrevDebit { get; set; }
        public decimal? PrevCredit { get; set; }
        public decimal? Opening { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
        public int? Count { get; set; }
    }
    public class PaymentAnalysisView
    {
        public int? VID { get; set; }
        public DateTime? VDate { get; set; }
        public string? VNo { get; set; }
        public int? AccountID { get; set; }
        public string? AccountName { get; set; }
        public DateTime? ClearedOn { get; set; }
        public int? NoOfDays { get; set; }
        public DateTime? DueOn { get; set; }
        public int? NoOfDaysFromDueDate { get; set; }
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
    //fill consolidated monthwise statement
    public class ConsolMonthwiseView
    {
        public int? ID { get; set; }
        public bool? IsGroup { get; set; }
        public string? AccountName { get; set; }
        public decimal? Balance { get; set; }
        public decimal? JanDr { get; set; }
        public decimal? JanCr { get; set; }
        public decimal? FebDr { get; set; }
        public decimal? FebCr { get; set; }
        public decimal? MarDr { get; set; }
        public decimal? MarCr { get; set; }
        public decimal? AprDr { get; set; }
        public decimal? AprCr { get; set; }
        public decimal? MayDr { get; set; }
        public decimal? MayCr { get; set; }
        public decimal? JunDr { get; set; }
        public decimal? JunCr { get; set; }
        public decimal? JulDr { get; set; }
        public decimal? JulCr { get; set; }
        public decimal? AugDr { get; set; }
        public decimal? AugCr { get; set; }
        public decimal? SepDr { get; set; }
        public decimal? SepCr { get; set; }
        public decimal? OctDr { get; set; }
        public decimal? OctCr { get; set; }
        public decimal? NovDr { get; set; }
        public decimal? NovCr { get; set; }
        public decimal? DecDr { get; set; }
        public decimal? DecCr { get; set; }

    }
    public class PartyOutstandingView
    {
        public int? AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? AccountCode { get; set; }
        public string? Category { get; set; }
        public DateTime? CustomerSince { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? AccountBalance { get; set; }
        public decimal? DueAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DueDays { get; set; }
        public decimal? CrLimitBalance { get; set; }
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
    public class DebitCreditView
    {
        public int? ID { get; set; }
        public string? Code { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Balance { get; set; }
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
    public class eReturnView
    {
        public int ID { get; set; }
        public string? VNo { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? Date { get; set; }
        public string? Particular { get; set; }
        public string? VatNo { get; }
        public decimal? InputVAT { get; set; }
        public decimal? OutputVAT { get; set; }
        public decimal? InPutTaxableAmount { get; set; }
        public decimal? OutPutTaxableAmount { get; set; }
        public decimal? InputNoneTaxableAmount { get; set; }
        public decimal? OutPutNoneTaxableAmount { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public int? PrimaryVoucherID { get; set; }
        public int? VoucherID { get; set; }
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
