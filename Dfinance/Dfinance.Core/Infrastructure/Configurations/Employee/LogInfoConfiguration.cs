
using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations.Employee
{
    public class LogInfoConfiguration : IEntityTypeConfiguration<LogInfo>
    {
        public void Configure(EntityTypeBuilder<LogInfo> builder)
        {
            builder.ToTable("LogInfo"); // Make sure it matches the actual table name in the database

            // Primary key
            builder.HasKey(li => li.ID);

            // Foreign key relationship with MaEmployees
            builder.HasOne(li => li.MaEmployee)
                .WithMany(me => me.LogInfos)
                .HasForeignKey(li => li.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict); // or another desired behavior

            // Other configurations for LogInfo entity
            builder.Property(li => li.SessionID).HasMaxLength(50); // Example: Set max length for SessionID
            builder.Property(li => li.LogOutMode).HasMaxLength(1); // Example: Set max length for LogOutMode
        }
    }
}
