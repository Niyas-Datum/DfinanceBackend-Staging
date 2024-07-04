using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class HREmployeeConfiguration : IEntityTypeConfiguration<HREmployee>
    {
        public void Configure(EntityTypeBuilder<HREmployee> builder)
        {
            builder.ToTable("HREmployee");

            builder.HasIndex(e => e.AccountId, "IX_HREmployee")
                .IsUnique();

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Accomodation).HasMaxLength(50);

            builder.Property(e => e.AccountId).HasColumnName("AccountID");

            builder.Property(e => e.Address1).HasMaxLength(50);

            builder.Property(e => e.Address2).HasMaxLength(50);

            builder.Property(e => e.AgentId)
                .HasMaxLength(50)
                .HasColumnName("AgentID");

            builder.Property(e => e.AirTicketSectorId).HasColumnName("AirTicketSectorID");

            builder.Property(e => e.ApprovedDate).HasColumnType("datetime");

            builder.Property(e => e.ApprovedFor).HasMaxLength(50);

            builder.Property(e => e.ArabicName).HasMaxLength(100);

            builder.Property(e => e.AreaId).HasColumnName("AreaID");

            builder.Property(e => e.BankAccountNo).HasMaxLength(30);

            builder.Property(e => e.BankRefNo).HasMaxLength(30);

            builder.Property(e => e.BasicSalary).HasColumnType("money");

            builder.Property(e => e.BloodGroupId).HasColumnName("BloodGroupID");

            builder.Property(e => e.BranchId).HasColumnName("BranchID");

            builder.Property(e => e.BusinessDivisionId).HasColumnName("BusinessDivisionID");

            builder.Property(e => e.Camp).HasMaxLength(50);

            builder.Property(e => e.CampusId).HasColumnName("CampusID");

            builder.Property(e => e.CategoryId).HasColumnName("CategoryID");

            builder.Property(e => e.ChildTuition).HasMaxLength(50);

            builder.Property(e => e.CityId).HasColumnName("CityID");

            builder.Property(e => e.CompanyPerc).HasColumnType("decimal(10, 2)");

            builder.Property(e => e.ComputerSkillId).HasColumnName("ComputerSkillID");

            builder.Property(e => e.ConPersonAddress).HasMaxLength(50);

            builder.Property(e => e.ConPersonPhone).HasMaxLength(20);

            builder.Property(e => e.ConPersonRelation).HasMaxLength(30);

            builder.Property(e => e.ContactPerson).HasMaxLength(50);

            builder.Property(e => e.ContractTypeId).HasColumnName("ContractTypeID");

            builder.Property(e => e.CountryId).HasColumnName("CountryID");

            builder.Property(e => e.CreditAccountId).HasColumnName("CreditAccountID");

            builder.Property(e => e.DeductionPerc).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.DeductionRate).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

            builder.Property(e => e.DesiginationAtJoiningId).HasColumnName("DesiginationAtJoiningID");

            builder.Property(e => e.DesignationId).HasColumnName("DesignationID");

            builder.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");

            builder.Property(e => e.Email).HasMaxLength(50);

            builder.Property(e => e.EmiratesId).HasColumnName("EmiratesID");

            builder.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");

            builder.Property(e => e.EndDate).HasColumnType("datetime");

            builder.Property(e => e.EnglishSkillId).HasColumnName("EnglishSkillID");

            builder.Property(e => e.Eosdate)
                .HasColumnType("datetime")
                .HasColumnName("EOSDate");

            builder.Property(e => e.Fax).HasMaxLength(20);

            builder.Property(e => e.FirstName).HasMaxLength(100);

            builder.Property(e => e.GenderId).HasColumnName("GenderID");

            builder.Property(e => e.GradeAtJoiningId).HasColumnName("GradeAtJoiningID");

            builder.Property(e => e.GradeId).HasColumnName("GradeID");

            builder.Property(e => e.GratuityAccountId).HasColumnName("GratuityAccountID");

            builder.Property(e => e.HealthInsurance).HasMaxLength(50);

            builder.Property(e => e.HiredFrom).HasMaxLength(100);

            builder.Property(e => e.IncrementDate).HasColumnType("datetime");

            builder.Property(e => e.JoiningDate).HasColumnType("datetime");

            builder.Property(e => e.LabourCardCancelledDate).HasColumnType("datetime");

            builder.Property(e => e.LabourCardStatusId).HasColumnName("LabourCardStatusID");

            builder.Property(e => e.LanguageId).HasColumnName("LanguageID");

            builder.Property(e => e.LastName).HasMaxLength(100);

            builder.Property(e => e.LeavePerYearId).HasColumnName("LeavePerYearID");

            builder.Property(e => e.LeaveSalaryAccountId).HasColumnName("LeaveSalaryAccountID");

            builder.Property(e => e.LeaveSalaryId).HasColumnName("LeaveSalaryID");

            builder.Property(e => e.LoanAccountId).HasColumnName("LoanAccountID");

            builder.Property(e => e.MartialStatusId).HasColumnName("MartialStatusID");

            builder.Property(e => e.Mdate)
                .HasColumnType("datetime")
                .HasColumnName("MDate");

            builder.Property(e => e.Memo).HasMaxLength(300);

            builder.Property(e => e.MinistrySalary).HasColumnType("money");

            builder.Property(e => e.MinistryStatus).HasMaxLength(20);

            builder.Property(e => e.Mobile).HasMaxLength(20);

            builder.Property(e => e.Muser)
                .HasMaxLength(50)
                .HasColumnName("MUser");

            builder.Property(e => e.NationalityId).HasColumnName("NationalityID");

            builder.Property(e => e.NormalHour).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.OnDate).HasColumnType("datetime");

            builder.Property(e => e.Otperc)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("OTPerc");

            builder.Property(e => e.Otrate)
                .HasColumnType("money")
                .HasColumnName("OTRate");

            builder.Property(e => e.PaymentModeId).HasColumnName("PaymentModeID");

            builder.Property(e => e.Phone1).HasMaxLength(20);

            builder.Property(e => e.Phone2).HasMaxLength(20);

            builder.Property(e => e.Pobox)
                .HasMaxLength(20)
                .HasColumnName("POBox");

            builder.Property(e => e.PositionId).HasColumnName("PositionID");

            builder.Property(e => e.ProjectId).HasColumnName("ProjectID");

            builder.Property(e => e.Rate).HasColumnType("money");

            builder.Property(e => e.ReligionId).HasColumnName("ReligionID");

            builder.Property(e => e.Remarks).HasMaxLength(100);

            builder.Property(e => e.RemarksFinancial).HasMaxLength(150);

            builder.Property(e => e.RenewalDate).HasColumnType("datetime");

            builder.Property(e => e.Room).HasMaxLength(50);

            builder.Property(e => e.SalaryMode).HasMaxLength(50);

            builder.Property(e => e.Section).HasMaxLength(50);

            builder.Property(e => e.SpecialRecognition).HasMaxLength(300);

            builder.Property(e => e.Speciality).HasMaxLength(100);

            builder.Property(e => e.Sponsor).HasMaxLength(100);

            builder.Property(e => e.SpouseDesignation).HasMaxLength(50);

            builder.Property(e => e.SpouseEmployment).HasMaxLength(100);

            builder.Property(e => e.SpouseQualification).HasMaxLength(100);

            builder.Property(e => e.StartDate).HasColumnType("datetime");

            builder.Property(e => e.StateId).HasColumnName("StateID");

            builder.Property(e => e.StatusId).HasColumnName("StatusID");

            builder.Property(e => e.Target).HasColumnType("money");

            builder.Property(e => e.TerminalBenifitsNominee).HasMaxLength(200);

            builder.Property(e => e.TicketAccountId).HasColumnName("TicketAccountID");

            builder.Property(e => e.TicketAmount).HasColumnType("money");

            builder.Property(e => e.TicketId).HasColumnName("TicketID");

            builder.Property(e => e.Transportation).HasMaxLength(50);

            builder.Property(e => e.UniqueId)
                .HasMaxLength(50)
                .HasColumnName("UniqueID");

            builder.Property(e => e.VehicleDescription).HasMaxLength(100);

            builder.Property(e => e.VisaCancelledDate).HasColumnType("datetime");

            builder.Property(e => e.VisaDesiginationId).HasColumnName("VisaDesiginationID");

            builder.Property(e => e.VisaTypeId).HasColumnName("VisaTypeID");

            builder.Property(e => e.Website).HasMaxLength(50);

            //builder.HasOne(d => d.Account)
            //    .WithOne(p => p.HremployeeAccount)
            //    .HasForeignKey<HREmployee>(d => d.AccountId);

            //builder.HasOne(d => d.CreditAccount)
            //    .WithMany(p => p.HremployeeCreditAccounts)
            //    .HasForeignKey(d => d.CreditAccountId)
            //    .HasConstraintName("FK_HREmployee_FiMaAccounts_BankID");

            //builder.HasOne(d => d.Department)
            //    .WithMany(p => p.Hremployees)
            //    .HasForeignKey(d => d.DepartmentId)
            //    .HasConstraintName("FK_HREmployee_DepartmentID");

            //builder.HasOne(d => d.DesiginationAtJoining)
            //    .WithMany(p => p.HremployeeDesiginationAtJoinings)
            //    .HasForeignKey(d => d.DesiginationAtJoiningId)
            //    .HasConstraintName("FK_HREmployee_DesiginationAtJoiningID");

            //builder.HasOne(d => d.Designation)
            //    .WithMany(p => p.HremployeeDesignations)
            //    .HasForeignKey(d => d.DesignationId)
            //    .HasConstraintName("FK_HREmployee_DesignationID");

            //builder.HasOne(d => d.GratuityAccount)
            //    .WithMany(p => p.HremployeeGratuityAccounts)
            //    .HasForeignKey(d => d.GratuityAccountId);

            //builder.HasOne(d => d.LeaveSalaryAccount)
            //    .WithMany(p => p.HremployeeLeaveSalaryAccounts)
            //    .HasForeignKey(d => d.LeaveSalaryAccountId);

            //builder.HasOne(d => d.LoanAccount)
            //    .WithMany(p => p.HremployeeLoanAccounts)
            //    .HasForeignKey(d => d.LoanAccountId);

            //builder.HasOne(d => d.PaymentMode)
            //    .WithMany(p => p.HremployeePaymentModes)
            //    .HasForeignKey(d => d.PaymentModeId)
            //    .HasConstraintName("FK_HREmployee_PaymentModeID");

            //builder.HasOne(d => d.Project)
            //    .WithMany(p => p.Hremployees)
            //    .HasForeignKey(d => d.ProjectId)
            //    .HasConstraintName("FK_HREmployee_ProjectID");

            //builder.HasOne(d => d.SpouseNationalityNavigation)
            //    .WithMany(p => p.HremployeeSpouseNationalityNavigations)
            //    .HasForeignKey(d => d.SpouseNationality)
            //    .HasConstraintName("FK_HREmployee_MaMisc");

            //builder.HasOne(d => d.Status)
            //    .WithMany(p => p.HremployeeStatuses)
            //    .HasForeignKey(d => d.StatusId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_HREmployee_StatusID");

            //builder.HasOne(d => d.TicketAccount)
            //    .WithMany(p => p.HremployeeTicketAccounts)
            //    .HasForeignKey(d => d.TicketAccountId);
        }
    }
}

