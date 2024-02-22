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
    public class DeliveryDetailsConfiguration : IEntityTypeConfiguration<DeliveryDetails>
    {
        public void Configure(EntityTypeBuilder<DeliveryDetails> builder)
        {


            builder.ToTable("DeliveryDetails");
            builder.HasKey(e => e.Id).HasName("PK__Delivery__3214EC277111691E");

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.ContactNo).HasMaxLength(200);
            builder.Property(e => e.ContactPerson).HasMaxLength(200);
            builder.Property(e => e.LocationName).HasMaxLength(200);
            builder.Property(e => e.PartyId).HasColumnName("PartyID");
            builder.Property(e => e.ProjectName).HasMaxLength(200);

            builder.HasOne(d => d.Party).WithMany(p => p.DeliveryDetails)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_DeliveryDetails_Parties");
          //  builder.HasOne(d => d.Party).WithMany(p => p.DeliveryDetails)
           //    .HasForeignKey(d => d.PartyId)
           //    .HasConstraintName("FK_DeliveryDetails_Parties");
        }


        }
}

