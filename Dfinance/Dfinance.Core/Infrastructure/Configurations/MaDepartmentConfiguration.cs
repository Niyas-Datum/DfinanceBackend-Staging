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
    public class MaDepartmentConfiguration : IEntityTypeConfiguration<MaDepartment>
    {
        public void Configure(EntityTypeBuilder<MaDepartment> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CompanyId).HasColumnName("CompanyID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.DepartmentTypeId).HasColumnName("DepartmentTypeID");

            /**
           * @relation: Branch -> company  MaDepartment
           * @use: Connect Branch  - Depertment
           * @connection: one to many 
           * **/
            builder.HasOne(d => d.Branch).WithMany(p => p.MaDepartments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaDepartments_MaCompanies");

            /**
            * @relation: Deptype -> company  Ma DepType  
            * @use: connect Depratment type
            * @connection: one to many 
            * **/
            builder.HasOne(k=> k.DepartmentType).WithMany(p=> p.MaDepartments)
                .HasForeignKey(k => k.DepartmentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaDepartments_MaDepartments");
        }
    }
}
