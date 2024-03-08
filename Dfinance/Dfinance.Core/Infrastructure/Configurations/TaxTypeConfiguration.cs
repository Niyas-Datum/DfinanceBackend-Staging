using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class TaxTypeConfiguration: IEntityTypeConfiguration<TaxType>
    {
        public void Configure(EntityTypeBuilder<TaxType> builder)
        {
            builder.ToTable("MaTaxType");
            builder.HasKey(e => e.Id).HasName("PK_MaTaxType");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.SalePurchaseModeId).HasColumnName("SalePurchaseModeID");
            builder.Property(e => e.TaxAccountId).HasColumnName("TaxAccountID");
            builder.Property(e => e.TaxMiscId).HasColumnName("TaxMiscID");
            builder.Property(e => e.ReceivableAccountId).HasColumnName("ReceivableAccountID");
            builder.Property(e => e.PayableAccountId).HasColumnName("PayableAccountID");
            builder.Property(e => e.SGSTReceivableAccountId).HasColumnName("SGSTReceivableAccountID");
            builder.Property(e => e.SGSTPayableAccountId).HasColumnName("SGSTPayableAccountID");
            builder.Property(e => e.CGSTReceivableAccountId).HasColumnName("CGSTReceivableAccountID");
            builder.Property(e => e.CGSTPayableAccountId).HasColumnName("CGSTPayableAccountID");
        }
    }
}
