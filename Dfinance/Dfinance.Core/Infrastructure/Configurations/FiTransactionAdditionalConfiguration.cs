using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core
{
    public class FiTransactionAdditionalConfiguration : IEntityTypeConfiguration<FiTransactionAdditionals>
    {
        public void Configure(EntityTypeBuilder<FiTransactionAdditionals> builder)
        {
            builder.ToTable("FiTransactionAdditionals");
            builder.HasKey(e => e.TransactionId).HasName("PK_FiTransactionAdditionals");             
            builder.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("TransactionID");

            builder.Property(e => e.AcceptDate).HasColumnType("datetime");

            builder.Property(e => e.AccountId).HasColumnName("AccountID");

            builder.Property(e => e.AccountId2).HasColumnName("AccountID2");

            builder.Property(e => e.Address1).HasMaxLength(100);

            builder.Property(e => e.Address2).HasMaxLength(100);

            builder.Property(e => e.AdvanceExRate).HasColumnType("money");

            builder.Property(e => e.AllocationPerc).HasColumnType("decimal(18, 4)");

            builder.Property(e => e.Amount).HasColumnType("money");

            builder.Property(e => e.ApplicationCode).HasMaxLength(100);

            builder.Property(e => e.AreaId).HasColumnName("AreaID");

            builder.Property(e => e.AuditNote).HasMaxLength(200);

            builder.Property(e => e.AvailableAmt).HasColumnType("money");

            builder.Property(e => e.AvailableLcamt)
                .HasColumnType("money")
                .HasColumnName("AvailableLCAmt");

            builder.Property(e => e.BankAddress).HasMaxLength(200);

            builder.Property(e => e.BemaxDays).HasColumnName("BEMaxDays");

            builder.Property(e => e.ClearDate).HasColumnType("datetime");

            builder.Property(e => e.CloseDate).HasColumnType("datetime");

            builder.Property(e => e.Code).HasMaxLength(100);

            builder.Property(e => e.ConsignTermId).HasColumnName("ConsignTermID");

            builder.Property(e => e.CountryId).HasColumnName("CountryID");

            builder.Property(e => e.CountryOfOriginId).HasColumnName("CountryOfOriginID");

            builder.Property(e => e.CreditAmt).HasColumnType("money");

            builder.Property(e => e.CustomsExRate).HasColumnType("money");

            builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

            builder.Property(e => e.DocumentDate).HasColumnType("datetime");

            builder.Property(e => e.DocumentNo).HasMaxLength(50);

            builder.Property(e => e.DueDate).HasColumnType("datetime");

            builder.Property(e => e.EndDate).HasColumnType("datetime");

            builder.Property(e => e.EndTime).HasColumnType("datetime");

            builder.Property(e => e.EntryDate).HasColumnType("datetime");

            builder.Property(e => e.EntryNo).HasMaxLength(50);

            builder.Property(e => e.ExchangeRate1).HasColumnType("money");

            builder.Property(e => e.ExchangeRate2).HasColumnType("decimal(18, 4)");

            builder.Property(e => e.ExpiryDate).HasColumnType("datetime");

            builder.Property(e => e.FirmId).HasColumnName("FirmID");

            builder.Property(e => e.FromLocationId).HasColumnName("FromLocationID");

            builder.Property(e => e.HandOverTime).HasColumnType("datetime");

            builder.Property(e => e.Hours).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.InLocId).HasColumnName("InLocID");

            builder.Property(e => e.InterestAmt).HasColumnType("money");

            builder.Property(e => e.InterestPerc).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.ItemId).HasColumnName("ItemID");

            builder.Property(e => e.Lcamt)
                .HasColumnType("money")
                .HasColumnName("LCAmt");

            builder.Property(e => e.LcapplnTransId).HasColumnName("LCApplnTransID");

            builder.Property(e => e.Lcno)
                .HasMaxLength(50)
                .HasColumnName("LCNo");

            builder.Property(e => e.LcoptionId).HasColumnName("LCOptionID");

            builder.Property(e => e.LoadMeasureTypeId).HasColumnName("LoadMeasureTypeID");

            builder.Property(e => e.LorryHireRate).HasColumnType("money");

            builder.Property(e => e.MarginAmt).HasColumnType("money");

            builder.Property(e => e.MeasureTypeId).HasColumnName("MeasureTypeID");

            builder.Property(e => e.ModeId).HasColumnName("ModeID");

            builder.Property(e => e.Name).HasMaxLength(100);

            builder.Property(e => e.OpenDate).HasColumnType("datetime");

            builder.Property(e => e.OtherBranchId).HasColumnName("OtherBranchID");

            builder.Property(e => e.OutLocId).HasColumnName("OutLocID");

            builder.Property(e => e.PartyName).HasMaxLength(50);

            builder.Property(e => e.PassNo).HasMaxLength(50);

            builder.Property(e => e.PostedBranchId).HasColumnName("PostedBranchID");

            builder.Property(e => e.PriceCategoryId).HasColumnName("PriceCategoryID");

            builder.Property(e => e.QtyPerLoad).HasColumnType("decimal(18, 4)");

            builder.Property(e => e.Rate).HasColumnType("money");

            builder.Property(e => e.ReceiveDate).HasColumnType("datetime");

            builder.Property(e => e.RecommendById).HasColumnName("RecommendByID");

            builder.Property(e => e.RecommendDate).HasColumnType("datetime");

            builder.Property(e => e.RecommendNote).HasMaxLength(200);

            builder.Property(e => e.RecommendStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.RefTransId1).HasColumnName("RefTransID1");

            builder.Property(e => e.RefTransId2).HasColumnName("RefTransID2");

            builder.Property(e => e.ReferenceDate).HasColumnType("datetime");

            builder.Property(e => e.ReferenceNo).HasMaxLength(50);

            builder.Property(e => e.RouteId).HasColumnName("RouteID");

            builder.Property(e => e.ShipBerthDate).HasColumnType("datetime");

            builder.Property(e => e.StartDate).HasColumnType("datetime");

            builder.Property(e => e.SubmitDate).HasColumnType("datetime");

            builder.Property(e => e.SystemRate).HasColumnType("money");

            builder.Property(e => e.TaxFormId).HasColumnName("TaxFormID");

            builder.Property(e => e.ToLocationId).HasColumnName("ToLocationID");

            builder.Property(e => e.TolerencePerc).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.TypeId).HasColumnName("TypeID");

            builder.Property(e => e.Unit).HasMaxLength(10);

            builder.Property(e => e.Vatno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VATNo");

            builder.Property(e => e.VehicleId).HasColumnName("VehicleID");

            //builder.HasOne(d => d.Account)
            //    .WithMany(p => p.FiTransactionAdditionalAccounts)
            //    .HasForeignKey(d => d.AccountId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_FiMaAccounts");

            //builder.HasOne(d => d.AccountId2Navigation)
            //    .WithMany(p => p.FiTransactionAdditionalAccountId2Navigations)
            //    .HasForeignKey(d => d.AccountId2)
            //    .HasConstraintName("FK_FiTransactionAdditionals_FiMaAccounts1");

            //builder.HasOne(d => d.Area)
            //    .WithMany(p => p.FiTransactionAdditionals)
            //    .HasForeignKey(d => d.AreaId)
            //    .HasConstraintName("FK__FiTransac__AreaI__4479F5BF");

            builder.HasOne(d => d.CountryOfOrigin)
                .WithMany(p => p.FiTransactionAdditionalCountryOfOrigins)
                .HasForeignKey(d => d.CountryOfOriginId)
                .HasConstraintName("FK_FiTransactionAdditionals_MaMisc5");

            //builder.HasOne(d => d.Department)
            //    .WithMany(p => p.FiTransactionAdditionals)
            //    .HasForeignKey(d => d.DepartmentId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MaDepartments");

            //builder.HasOne(d => d.Firm)
            //    .WithMany(p => p.FiTransactionAdditionals)
            //    .HasForeignKey(d => d.FirmId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_Firms");

            //builder.HasOne(d => d.FromLocation)
            //    .WithMany(p => p.FiTransactionAdditionalFromLocations)
            //    .HasForeignKey(d => d.FromLocationId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MaMisc");

            //builder.HasOne(d => d.InLoc)
            //    .WithMany(p => p.FiTransactionAdditionalInLocs)
            //    .HasForeignKey(d => d.InLocId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_Locations1");

            builder.HasOne(d => d.Mode)
                .WithMany(p => p.FiTransactionAdditionalModes)
                .HasForeignKey(d => d.ModeId)
                .HasConstraintName("FK_FiTransactionAdditionals_MaMisc4");

            //builder.HasOne(d => d.OtherBranch)
            //    .WithMany(p => p.FiTransactionAdditionalOtherBranches)
            //    .HasForeignKey(d => d.OtherBranchId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MaCompanies1");

            //builder.HasOne(d => d.OutLoc)
            //    .WithMany(p => p.FiTransactionAdditionalOutLocs)
            //    .HasForeignKey(d => d.OutLocId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_Locations2");

            //builder.HasOne(d => d.PostedBranch)
            //    .WithMany(p => p.FiTransactionAdditionalPostedBranches)
            //    .HasForeignKey(d => d.PostedBranchId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MaCompanies");

            //builder.HasOne(d => d.PriceCategory)
            //    .WithMany(p => p.FiTransactionAdditionals)
            //    .HasForeignKey(d => d.PriceCategoryId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MAPriceCategory");

            //builder.HasOne(d => d.RecommendBy)
            //    .WithMany(p => p.FiTransactionAdditionals)
            //    .HasForeignKey(d => d.RecommendById)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MaEmployees");

            //builder.HasOne(d => d.RefTransId1Navigation)
            //    .WithMany(p => p.FiTransactionAdditionalRefTransId1Navigations)
            //    .HasForeignKey(d => d.RefTransId1)
            //    .HasConstraintName("FK_FiTransactionAdditionals_FiTransactions1");

            //builder.HasOne(d => d.Route)
            //    .WithMany(p => p.FiTransactionAdditionals)
            //    .HasForeignKey(d => d.RouteId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_MaRoutes");

            //builder.HasOne(d => d.ToLocation)
            //    .WithMany(p => p.FiTransactionAdditionalToLocations)
            //    .HasForeignKey(d => d.ToLocationId)
            //    .HasConstraintName("FK_FiTransactionAdditionals_Locations");

            builder.HasOne(d => d.Transaction)
                .WithMany(p => p.FiTransactionAdditionalRefTransId1Navigations)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_FiTransactionAdditionals_FiTransactions");

            builder.HasOne(d => d.Type)
                .WithMany(p => p.FiTransactionAdditionalTypes)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_FiTransactionAdditionals_MaMisc1");
        }
    }
}
