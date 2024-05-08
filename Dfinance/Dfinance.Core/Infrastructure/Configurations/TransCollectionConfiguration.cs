using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class TransCollectionConfiguration : IEntityTypeConfiguration<TransCollection>
    {
             

        public void Configure(EntityTypeBuilder<TransCollection> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.Description).HasMaxLength(200);

            builder.Property(e => e.DueDate).HasColumnType("datetime");

            builder.Property(e => e.InstrumentBank).HasMaxLength(100);

            builder.Property(e => e.InstrumentDate).HasColumnType("datetime");

            builder.Property(e => e.InstrumentNo).HasMaxLength(50);

            builder.Property(e => e.InstrumentTypeId).HasColumnName("InstrumentTypeID");

            builder.Property(e => e.PayTypeId).HasColumnName("PayTypeID");

            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");

            builder.Property(e => e.Veid).HasColumnName("VEID");

            builder.HasOne(d => d.Transaction)
                        .WithMany(p => p.TransCollections)
                        .HasForeignKey(d => d.TransactionId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_TransCollections_FiTransactions");

            builder.HasOne(d => d.Ve)
                        .WithMany(p => p.TransCollections)
                        .HasForeignKey(d => d.Veid)
                        .HasConstraintName("FK_TransCollections_FiTransactionEntries");
        }
    }
}
