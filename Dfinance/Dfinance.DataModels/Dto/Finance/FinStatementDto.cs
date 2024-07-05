using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Finance
{
    public class FinStmtCommonDto:CommonDto
    {       
        //public PopUpDto? User { get; set; }       
        public PopUpDto? AccountGroup { get; set; }
        public PopUpDto? CostCentre { get; set; }
    }
    public class CommonDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUpto { get; set; }
        public DropdownDto? Branch { get; set; }
        public PopUpDto? Account { get; set; }
        public PopUpDto? User { get; set; }

    }
    public class DayBookDto:FinStmtCommonDto
    {       
        public DropdownDto? VoucherType { get; set; }        
        public bool? Detailed { get; set; }
        public bool? Posted { get; set; }
    }
    public class TrialBalanceDto:FinStmtCommonDto
    {       
        public DropdownDto? Transactions { get; set; }
        public bool? Detailed { get; set; }
        public bool? OpeningVouchersOnly { get; set; }
        public bool? ShowOpening { get; set; }
        public bool? ShowClosing { get; set; }
        public bool? LedgersOnly { get; set; }
        public PopUpDto? CostCentre { get; set; }
    }
    public class CashBankBookDto:FinStmtCommonDto
    {       
        public DropDownDtoNature CashOrBank {  get; set; }      
    }
    public class BillwiseStmtDto:FinStmtCommonDto
    {
        public bool? EffectiveDate { get; set; }
        public bool? VoucherDate { get; set; }
        public int? DaysFrom {  get; set; }
        public int? DaysUpto { get; set; }
        public DropdownDto? BillType {  get; set; }
        public PopUpDto? AccGroup { get; set; }
        public PopUpDto? AccCategory { get; set; }
        public bool? Detailed { get; set; }
        public bool? PendingBills { get; set; }
    }
    public class BalanceSheetDto:CommonDto
    {
        public DropdownDto? ViewBy {  get; set; }
        public bool? Detailed {  get; set; }
        public DropdownDto Category { get; set; }
        public PopUpDto? CostCentre { get; set; }
    }
    public class PartyOutStandingDto : CommonDto
    {
        public char? ViewBy { get; set; }
        public PopUpDto? Party { get; set; }
        public PopUpDto? Salesman { get; set; }
    }
    public class SalesmanColDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PopUpDto? SalesPerson { get; set; }
    }
    public class AgingReportDto:CommonDto
    {
        public char? ViewBy { get; set; }//Customer-C,Supplier-S
        public PopUpDto? Staff { get; set; }
        public DropdownDto? PartyCategory { get; set; }
        public DropdownDto? SalesArea { get; set; }
    }
    public class eReturnsDto:CommonDto
    {
        public string? ViewBy { get; set; }
        public PopUpDto? Voucher { get; set; }
        public DropdownDto? VatType { get; set; }
    }
    public class CostCentreReportDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PopUpDto? CostCentre { get; set; }
        public PopUpDto? Account {  get; set; }
        public DropdownDto? Voucher { get;  set; }
    }
    public class CostCentAccStmtDto:CommonDto
    {
        public PopUpDto? CostCentre { get; set; }
    }
   
}
