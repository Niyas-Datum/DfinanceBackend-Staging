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
    public class FiChequesTranConfiguration : IEntityTypeConfiguration<FiChequesTran>
    {
        public void Configure(EntityTypeBuilder<FiChequesTran> builder)
        {
            builder.ToTable("FiChequesTran");
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.ChequeId).HasColumnName("ChequeID");

            builder.Property(e => e.TranType).HasMaxLength(20);

            builder.Property(e => e.Veid).HasColumnName("VEID");

            builder.Property(e => e.Vid).HasColumnName("VID");

            builder.HasOne(d => d.Ve)
                .WithMany(p => p.FiChequesTrans)
                .HasForeignKey(d => d.Veid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_FiChequesTran_FiChequesTran");
        }
    }
}
