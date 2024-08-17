using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class DocTypeConfiguration : IEntityTypeConfiguration<DocType>
    {
        public void Configure(EntityTypeBuilder<DocType> builder)
        {
            builder.ToTable("DocTypes");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.Description).HasMaxLength(150);
            builder.Property(e => e.Directory).HasMaxLength(75);
            builder.Property(e => e.IsLcdoc).HasColumnName("IsLCDoc");
            builder.Property(e => e.IsLcmandatoryDoc).HasColumnName("IsLCMandatoryDoc");
            builder.Property(e => e.IsPodoc).HasColumnName("IsPODoc");

            builder.Property(e => e.Pirdesc)
                .HasMaxLength(500)
                .HasColumnName("PIRDesc");
            builder.HasOne(d => d.CreatedBranch)
                   .WithMany(p => p.DocTypes)
                   .HasForeignKey(d => d.CreatedBranchId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_DocTypes_MaCompanies");

            builder.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.DocTypes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocTypes_MaEmployees");
        }
    }
}
