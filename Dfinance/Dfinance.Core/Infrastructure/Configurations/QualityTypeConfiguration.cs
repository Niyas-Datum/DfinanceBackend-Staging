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
    public class QualityTypeConfiguration : IEntityTypeConfiguration<InvQualityPrice>
    {
        public void Configure(EntityTypeBuilder<InvQualityPrice> builder)
        {
            builder.HasNoKey();

            builder.ToTable("InvQualityPrice");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            builder.Property(e => e.QualityId).HasColumnName("QualityID");

            builder.Property(e => e.Rate).HasColumnType("decimal(18, 8)");

            builder.HasOne(d => d.Quality)
                .WithMany()
                .HasForeignKey(d => d.QualityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvQualityPrice_MaMisc");
        }
    }
}
