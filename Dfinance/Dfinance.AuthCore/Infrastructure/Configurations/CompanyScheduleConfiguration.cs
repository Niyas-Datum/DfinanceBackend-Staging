using Dfinance.AuthCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.AuthCore.Infrastructure.Configurations
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Updated: Niyas
   **/
    class CompanyScheduleConfiguration : IEntityTypeConfiguration<CompanySchedule>
    {
        public void Configure(EntityTypeBuilder<CompanySchedule> builder)
        {
            builder.ToTable("CompanySchedule");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CompanyId).HasColumnName("CompanyID");
            builder.HasOne(d => d.Company).WithMany(p => p.CompanySchedules)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanySchedule_Company");
        }
    }
}