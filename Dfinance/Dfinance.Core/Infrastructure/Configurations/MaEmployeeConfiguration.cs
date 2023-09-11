
using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    /**
   # Created On: sun 27 Aug 2023
   # Updated: Niyas
   **/
    public class MaEmployeeConfiguration : IEntityTypeConfiguration<MaEmployee>
    {
        public void Configure(EntityTypeBuilder<MaEmployee> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Employees");
            builder.HasIndex(e => e.UserName, "IX_MaEmployees").IsUnique();
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
        }
    }
}