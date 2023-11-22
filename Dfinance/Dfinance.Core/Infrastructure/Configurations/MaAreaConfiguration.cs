using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class MaAreaConfiguration:IEntityTypeConfiguration<MaArea>
    {
        public void Configure(EntityTypeBuilder <MaArea> builder)
        {
            builder.ToTable("MaArea");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.ParentId).HasColumnName("ParentID");

            /* relation:MaArea->MaCompany
               connection:one to many */
            builder.HasOne(d => d.maCompany).WithMany(p => p.BranchIdArea)
                .HasForeignKey(d => d.CreatedBranchId)
                .HasConstraintName("FK_MaArea_MaCompanies");

            /* relation:MaArea->MaEmployee
             connection:one to many */
            builder.HasOne(d => d.maEmployee).WithMany(p => p.AreaCreatedBy)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_MaArea_MaEmployees");
        }
    }
}
