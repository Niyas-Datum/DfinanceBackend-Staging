using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class SpFillCostCentreByIdG
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string PType { get; set; }
        public string? Type { get; set; }
        public string? SerialNo { get; set; }
        public string? RegNo { get; set; }
        public string? SupplierName { get; set; }
        public int? SupplierID { get; set; }
        public string? SupplierCode { get; set; }
        public string? ClientName { get; set; }
        public int? ClientID { get; set; }
        public string? ClientCode { get; set; }
        public string? StaffIDName { get; set; }
        public int? StaffID { get; set; }
        public string? StaffIDCode { get; set; }
        public string? StaffID1Name { get; set; }
        public int? StaffID1 { get; set; }
        public string? StaffID1Code { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? SDate { get; set; }
        public string? Make { get; set; }
        public string? MYear { get; set; }
        public DateTime? EDate { get; set; }
        public decimal? ContractValue { get; set; }
        public decimal? InvoicedAmt { get; set; }
        public bool? IsPaid { get; set; }
        public string? Site { get; set; }
        public int? ParentID { get; set; }
        public bool? IsGroup { get; set; }
        public int? CostCategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? ParentName { get; set; }
    }
    public class SpFillCostCentreG
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class FillPopupView
    {
        public int ID { get; set; }
        public string Name {  get; set; }
        public string Code {  set; get; }
    }
}
