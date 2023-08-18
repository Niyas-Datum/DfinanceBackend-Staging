using Dfinance.AuthCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.AuthCore.Infrastructure.Configurations
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Updated: Niyas
   **/
    public class UserCompanyConfiguration : IEntityTypeConfiguration<UserCompany>
    {
        public void Configure(EntityTypeBuilder<UserCompany> builder)
        {
            builder.ToTable("UserCompany");
            builder.HasIndex(x => new { x.CompanyId, x.UserId }, "IX_UserCompany").IsUnique();
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.CompanyId).HasColumnName("CompanyID");
            builder.Property(x => x.UserId).HasColumnName("UserID");
            builder.HasOne(q => q.Company).WithMany(r => r.UserCompanies)
                    .HasForeignKey(q => q.CompanyId).OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCompany_Company");
            builder.HasOne(d => d.User).WithMany(x => x.UserCompanies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCompany_Users");
        }
    }
}