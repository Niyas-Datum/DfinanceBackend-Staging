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
    public class InvMaCounterConfiguration : IEntityTypeConfiguration<InvMaCounter>
    {
        public void Configure(EntityTypeBuilder<InvMaCounter> builder) 
        {
            builder.HasKey(e => e.MachineName);
            builder.Property(e => e.MachineName).HasMaxLength(50);

            builder.Property(e => e.CounterCode).HasMaxLength(50);

            builder.Property(e => e.CounterName).HasMaxLength(100);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            builder.Property(e => e.MachineIp)
                .HasMaxLength(50)
                .HasColumnName("MachineIP");
        }
    }
}
