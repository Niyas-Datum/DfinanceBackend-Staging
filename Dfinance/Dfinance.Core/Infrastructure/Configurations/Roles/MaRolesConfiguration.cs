using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfinance.Core.Domain.Roles;

namespace Dfinance.Core.Infrastructure.Configurations.Roles
{
    public class MaRolesConfiguration : IEntityTypeConfiguration<MaRoles>
    {
        public void Configure(EntityTypeBuilder<MaRoles> entity)
        {
            entity.ToTable("MaRoles");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(20);

            //entity.HasOne(d => d.CreatedBranch).WithMany(p => p.MaRoles)
            //    .HasForeignKey(d => d.CreatedBranchId)
            //    .HasConstraintName("FK_MaRoles_MaCompanies");

            //entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaRoles)
            //    .HasForeignKey(d => d.CreatedBy)
            //    .HasConstraintName("FK_MaRoles_MaEmployees");
        }
    }
}
