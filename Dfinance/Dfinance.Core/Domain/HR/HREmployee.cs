using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class HREmployee
    {
        //public Hremployee()
        //{
        //    TransEmployees = new HashSet<TransEmployee>();
        //    Vmdrivers = new HashSet<Vmdriver>();
        //}

        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime? Dob { get; set; }
        public int? BloodGroupId { get; set; }
        public int? MartialStatusId { get; set; }
        public int? ReligionId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Pobox { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? ContactPerson { get; set; }
        public string? ConPersonPhone { get; set; }
        public string? ConPersonAddress { get; set; }
        public string? ConPersonRelation { get; set; }
        public int? EmiratesId { get; set; }
        public int? CityId { get; set; }
        public int? AreaId { get; set; }
        public int? CategoryId { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? NationalityId { get; set; }
        public string? Remarks { get; set; }
        public int? CountryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ProjectId { get; set; }
        public string? BankAccountNo { get; set; }
        public int? CreditAccountId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public int? VisaTypeId { get; set; }
        public int? VisaDesiginationId { get; set; }
        public int? GenderId { get; set; }
        public int StatusId { get; set; }
        public bool? LeaveSalaryId { get; set; }
        public int BranchId { get; set; }
        public string? SalaryMode { get; set; }
        public DateTime? IncrementDate { get; set; }
        public int? LeavePerYearId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public decimal? MinistrySalary { get; set; }
        public int? SeniorityNo { get; set; }
        public string? MinistryStatus { get; set; }
        public DateTime? Eosdate { get; set; }
        public int? GradeId { get; set; }
        public string? RemarksFinancial { get; set; }
        public int? TicketId { get; set; }
        public int? LeaveOpeningBalance { get; set; }
        public DateTime? OnDate { get; set; }
        public int? LeaveTaken { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? PaymentModeId { get; set; }
        public int? LoanAccountId { get; set; }
        public string? BankRefNo { get; set; }
        public decimal? Otrate { get; set; }
        public int? DesiginationAtJoiningId { get; set; }
        public int? GradeAtJoiningId { get; set; }
        public string? Sponsor { get; set; }
        public string? UniqueId { get; set; }
        public string? AgentId { get; set; }
        public string? TerminalBenifitsNominee { get; set; }
        public bool? MixedSalary { get; set; }
        public string? Section { get; set; }
        public int? ContractTypeId { get; set; }
        public bool? RateIncrementByPercent { get; set; }
        public int? BusinessDivisionId { get; set; }
        public string? VehicleDescription { get; set; }
        public int? AirTicketSectorId { get; set; }
        public decimal? NormalHour { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeId { get; set; }
        public string? SpecialRecognition { get; set; }
        public string? Memo { get; set; }
        public int? PeriodsPerWeek { get; set; }
        public int? GratuityAccountId { get; set; }
        public int? LeaveSalaryAccountId { get; set; }
        public int? TicketAccountId { get; set; }
        public decimal? TicketAmount { get; set; }
        public int? LabourCardStatusId { get; set; }
        public int? CampusId { get; set; }
        public string? Speciality { get; set; }
        public int? PositionId { get; set; }
        public string? HiredFrom { get; set; }
        public string? Accomodation { get; set; }
        public string? ApprovedFor { get; set; }
        public string? SpouseEmployment { get; set; }
        public int? SpouseNationality { get; set; }
        public string? SpouseDesignation { get; set; }
        public string? SpouseQualification { get; set; }
        public bool? VisaCancelled { get; set; }
        public DateTime? VisaCancelledDate { get; set; }
        public bool? LabourCardCancelled { get; set; }
        public DateTime? LabourCardCancelledDate { get; set; }
        public int? LanguageId { get; set; }
        public string? Camp { get; set; }
        public string? Room { get; set; }
        public int? EnglishSkillId { get; set; }
        public int? ComputerSkillId { get; set; }
        public decimal? Rate { get; set; }
        public string? Transportation { get; set; }
        public string? HealthInsurance { get; set; }
        public string? ChildTuition { get; set; }
        public int? NoOfChildren { get; set; }
        public string? ArabicName { get; set; }
        public DateTime? Mdate { get; set; }
        public string? Muser { get; set; }
        public int? StateId { get; set; }
        public decimal? Target { get; set; }
        public int? WorkMonth { get; set; }
        public int? BonusMonth { get; set; }
        public decimal? CompanyPerc { get; set; }
        public decimal? Otperc { get; set; }
        public decimal? DeductionRate { get; set; }
        public decimal? DeductionPerc { get; set; }
        //public bool? WeekendHolidayPayable { get; set; }

        //public virtual FiMaAccount Account { get; set; } = null!;
        //public virtual FiMaAccount? CreditAccount { get; set; }
        //public virtual MaDepartment? Department { get; set; }
        //public virtual MaDesignation? DesiginationAtJoining { get; set; }
        //public virtual MaDesignation? Designation { get; set; }
        //public virtual FiMaAccount? GratuityAccount { get; set; }
        //public virtual FiMaAccount? LeaveSalaryAccount { get; set; }
        //public virtual FiMaAccount? LoanAccount { get; set; }
        ////public virtual MaMisc? PaymentMode { get; set; }
        //public virtual CostCentre? Project { get; set; }
        //public virtual MaMisc? SpouseNationalityNavigation { get; set; }
        //public virtual MaMisc Status { get; set; } = null!;
        //public virtual FiMaAccount? TicketAccount { get; set; }
        //public virtual ICollection<TransEmployee> TransEmployees { get; set; }
        //public virtual ICollection<Vmdriver> Vmdrivers { get; set; }
    }
}
