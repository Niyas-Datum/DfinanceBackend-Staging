using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Dfinance.Core.Infrastructure.Configurations
{
    public class CostCentreConfiguration : IEntityTypeConfiguration<CostCentre>
    {
        public void Configure(EntityTypeBuilder<CostCentre> builder)
        {
            builder.ToTable("CostCentre");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.SupplierId).HasColumnName("SupplierID");
            builder.Property(e => e.ClientId).HasColumnName("ClientID");
            builder.Property(e => e.StaffId).HasColumnName("StaffID");
            builder.Property(e => e.StaffId1).HasColumnName("StaffID1");
            builder.Property(e => e.CostCategoryId).HasColumnName("CostCategoryID");
            builder.Property(e => e.ParentId).HasColumnName("ParentID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CraetedBranchID");

            /* relation:CostCenter->FiMaAccount for Client
             * connection:one to many */


            //builder.HasOne(c => c.FimaAccountClient)
            //    .WithMany(f => f.CostCentreClientAccount)
            //    .HasForeignKey(c => c.ClientId)
            //    .HasConstraintName("FK_CostCentre_FiMaAccounts");


            /* relation:CostCenter->FiMaAccount for Supplier
            * connection:one to many */

            //builder.HasOne(c => c.FiMaAccountSupplier)
            //    .WithMany(f => f.CostCentreSupplierAccount)
            //    .HasForeignKey(c => c.SupplierId)
            //    .HasConstraintName("FK_CostCentre_FiMaAccounts1");
        }
    }
}
