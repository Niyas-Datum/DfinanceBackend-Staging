using Dfinance.DataModels.Dto.Common;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("dateFrom")]
        public DateTime? DateFrom { get; set; }
        [JsonPropertyName("dateUpto")]
        public DateTime? DateUpto { get; set; }
        [JsonPropertyName("branch")]
        public DropdownDto? Branch { get; set; }
        [JsonPropertyName("account")]
        public PopUpDto? Account { get; set; }
        [JsonPropertyName("user")]
        public PopUpDto? User { get; set; }

    }
    public class DayBookDto:FinStmtCommonDto
    {
        [JsonPropertyName("voucherType")]
        public DropdownDto? VoucherType { get; set; }
        [JsonPropertyName("detailed")]
        public bool? Detailed { get; set; }
        [JsonPropertyName("posted")]
        public bool? Posted { get; set; }
    }
    public class TrialBalanceDto:FinStmtCommonDto
    {
        [JsonPropertyName("transactions")]
        public DropdownDto? Transactions { get; set; }
        [JsonPropertyName("detailed")]
        public bool? Detailed { get; set; }
        [JsonPropertyName("openingVouchersOnly")]
        public bool? OpeningVouchersOnly { get; set; }
        [JsonPropertyName("showOpening")]
        public bool? ShowOpening { get; set; }
        [JsonPropertyName("showClosing")]
        public bool? ShowClosing { get; set; }
        [JsonPropertyName("ledgersOnly")]
        public bool? LedgersOnly { get; set; }
        [JsonPropertyName("costCentre")]
        public PopUpDto? CostCentre { get; set; }
    }
    public class CashBankBookDto
    {
        [JsonPropertyName("dateFrom")]
        public DateTime? DateFrom { get; set; }
        [JsonPropertyName("dateUpto")]
        public DateTime? DateUpto { get; set; }
        [JsonPropertyName("branch")]
        public DropdownDto Branch { get; set; }
        [JsonPropertyName("cashOrBank")]
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
        [JsonPropertyName("viewBy")]
        public char? ViewBy { get; set; }
        [JsonPropertyName("party")]
        public PopUpDto? Party { get; set; }
        [JsonPropertyName("salesman")]
        public PopUpDto? Salesman { get; set; }
    }
    public class SalesmanColDto
    {
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
        [JsonPropertyName("salesPerson")]
        public PopUpDto? SalesPerson { get; set; }
    }
    public class AgingReportDto:CommonDto
    {
        [JsonPropertyName("viewBy")]
        public char? ViewBy { get; set; }//Customer-C,Supplier-S
        [JsonPropertyName("staff")]
        public PopUpDto? Staff { get; set; }
        [JsonPropertyName("partyCategory")]
        public DropdownDto? PartyCategory { get; set; }
        [JsonPropertyName("salesArea")]
        public DropdownDto? SalesArea { get; set; }
    }
    public class eReturnsDto:CommonDto
    {
        [JsonPropertyName("viewBy")]
        public string? ViewBy { get; set; }
        [JsonPropertyName("voucher")]
        public PopUpDto? Voucher { get; set; }
        [JsonPropertyName("vatType")]
        public DropdownDto? VatType { get; set; }
    }
    public class CostCentreReportDto
    {
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
        [JsonPropertyName("costCentre")]
        public PopUpDto? CostCentre { get; set; }
        [JsonPropertyName("account")]
        public PopUpDto? Account {  get; set; }
        [JsonPropertyName("voucher")]
        public DropdownDto? Voucher { get;  set; }
    }
    public class CostCentAccStmtDto:CommonDto
    {
        public PopUpDto? CostCentre { get; set; }
    }
   
}
