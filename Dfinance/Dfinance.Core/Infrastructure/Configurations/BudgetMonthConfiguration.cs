using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class BudgetMonthConfiguration:IEntityTypeConfiguration<BudgetMonth>
    {
        public void Configure(EntityTypeBuilder<BudgetMonth> builder)
        {
            builder.ToTable("BudgetMonth");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
           
        }
    }
}
