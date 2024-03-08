using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Dfinance.Core.Infrastructure.Configurations
{
    public class UnitMasterConfiguration : IEntityTypeConfiguration<UnitMaster>
    {
        public void Configure(EntityTypeBuilder<UnitMaster> builder)
        {
            builder.HasKey(e => e.Unit).HasName("PK_InvUnitMaster");

        }
    }
}
