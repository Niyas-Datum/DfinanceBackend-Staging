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
    public class TransReferenceConfiguration : IEntityTypeConfiguration<TransReference>
    {
        public void Configure(EntityTypeBuilder<TransReference> builder)
        {
            builder.ToTable("TransReferences");
            builder.HasIndex(e => new { e.TransactionId, e.RefTransId }, "IX_InvTransReferences")
                   .IsUnique();

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Description).HasMaxLength(200);

            builder.Property(e => e.RefTransId).HasColumnName("RefTransID");

            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");

            builder.HasOne(d => d.RefTrans)
                .WithMany(p => p.TransReferenceRefTrans)
                .HasForeignKey(d => d.RefTransId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvTransReferences_FiTransactions1");

            builder.HasOne(d => d.Transaction)
                .WithMany(p => p.TransReferenceTransactions)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_InvTransReferences_FiTransactions");
        }
    }
}
