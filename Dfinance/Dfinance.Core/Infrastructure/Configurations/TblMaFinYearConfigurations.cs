using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class TblMaFinYearConfigurations : IEntityTypeConfiguration<TblMaFinYear>
    {
        public void Configure(EntityTypeBuilder<TblMaFinYear> builder)
        {
            builder.HasKey(e => e.FinYearId);

            builder.ToTable("tblMaFinYear");

            builder.HasIndex(e => e.FinYearCode, "IX_FinancialYear_Code").IsUnique();

            builder.Property(e => e.FinYearId).HasColumnName("FinYearID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.EndDate).HasColumnType("datetime");
            builder.Property(e => e.FinYearCode).HasMaxLength(50);
            builder.Property(e => e.LockTillDate).HasColumnType("datetime");
            builder.Property(e => e.StartDate).HasColumnType("datetime");
            builder.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.TblMaFinYears)
                .HasForeignKey(d => d.CreatedBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblMaFinYear_MaCompanies");

            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblMaFinYears)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblMaFinYear_MaEmployees");
       
        }
    }
}
