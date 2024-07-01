using Dfinance.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core
{
    public class FiTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int VoucherId { get; set; }
        public int SerialNo { get; set; }
        public string? TransactionNo { get; set; }
        public bool IsPostDated { get; set; }
        public int? CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public int? RefPageTypeId { get; set; }
        public int? RefPageTableId { get; set; }
        public string? ReferenceNo { get; set; }
        public int CompanyId { get; set; }
        public int? FinYearId { get; set; }
        public string? InstrumentType { get; set; }
        public string? InstrumentNo { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string? InstrumentBank { get; set; }
        public string? CommonNarration { get; set; }
        public int AddedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public string? ApproveNote { get; set; }
        public string? Action { get; set; }
        public int? StatusId { get; set; }
        public bool? IsAutoEntry { get; set; }
        public bool? Posted { get; set; }
        public bool? Active { get; set; }
        public bool? Cancelled { get; set; }
        public int? AccountId { get; set; }
        public string? Description { get; set; }
        public int? RefTransId { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? CostCentreId { get; set; }
        public int? PageId { get; set; }
        public string? MachineName { get; set; }

        public virtual FiMaAccount? Account { get; set; }
        public virtual MaEmployee AddedByNavigation { get; set; } = null!;
        public virtual MaEmployee? ApprovedByNavigation { get; set; }
        public virtual MaCompany Company { get; set; } = null!;
        public virtual CostCentre? CostCentre { get; set; }
        public virtual Currency? Currency { get; set; }
        public virtual TblMaFinYear? FinYear { get; set; }
        public virtual FiTransaction? RefTrans { get; set; }
        public virtual MaMisc? Status { get; set; }
       public virtual Voucher Voucher { get; set; } = null!;
       // public virtual FiTransactionAdditionals? FiTransactionAdditionalTransaction { get; set; }
        public virtual ICollection<BudgetMonth> BudgetMonths { get; set; }
        //public virtual ICollection<DocumentReference> DocumentReferences { get; set; }
        //public virtual ICollection<DocumentRequest> DocumentRequests { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }
        //public virtual ICollection<EduFeeDetail> EduFeeDetails { get; set; }
        //public virtual ICollection<EduFeePostingDetail> EduFeePostingDetails { get; set; }
        public virtual ICollection<FiTransactionAdditionals> FiTransactionAdditionalRefTransId1Navigations { get; set; }
        public virtual ICollection<FiTransactionEntry> FiTransactionEntries { get; set; }
       
        public virtual ICollection<FiVoucherAllocation> FiVoucherAllocationRefTrans { get; set; }
       
        public virtual ICollection<FiVoucherAllocation> FiVoucherAllocationVidNavigations { get; set; }
        //public virtual ICollection<HmsconsultationDetail> HmsconsultationDetails { get; set; }
        //public virtual ICollection<HmseyeTest> HmseyeTests { get; set; }
        //public virtual ICollection<HmsgeneralFinding> HmsgeneralFindings { get; set; }
        //public virtual ICollection<HmslabTest> HmslabTests { get; set; }
        //public virtual ICollection<Hmsscanning> Hmsscannings { get; set; }
        //public virtual ICollection<HmstoothDetail> HmstoothDetails { get; set; }
        //public virtual ICollection<HrfinalSettlement> HrfinalSettlements { get; set; }
        //public virtual ICollection<HrinvoiceDetail> HrinvoiceDetails { get; set; }
        //public virtual ICollection<HrprojectTran> HrprojectTrans { get; set; }
        //public virtual ICollection<Hrsalary> Hrsalaries { get; set; }
        //public virtual ICollection<HrtimeSheetDetail> HrtimeSheetDetails { get; set; }
        //public virtual ICollection<InvOptic> InvOptics { get; set; }
        public virtual ICollection<InvTransItems> InvTransItems { get; set; }
        public virtual ICollection<InvUniqueItems> InvUniqueItems { get; set; }
        public virtual ICollection<FiTransaction> InverseRefTrans { get; set; }
        //public virtual ICollection<JobDetail> JobDetails { get; set; }
        public virtual ICollection<TransCollection> TransCollections { get; set; }
        //public virtual ICollection<TransCriterion> TransCriteria { get; set; }
        //public virtual ICollection<TransEmployee> TransEmployees { get; set; }
        public virtual ICollection<TransExpense> TransExpenses { get; set; }
        //public virtual ICollection<TransItemExpense> TransItemExpenses { get; set; }
        //public virtual ICollection<TransLoadSchedule> TransLoadSchedules { get; set; }
        public virtual ICollection<TransReference> TransReferenceRefTrans { get; set; }
        public virtual ICollection<TransReference> TransReferenceTransactions { get; set; }
        //public virtual ICollection<TransReminder> TransReminders { get; set; }
        //public virtual ICollection<TransShippingItem> TransShippingItems { get; set; }
    }
}
