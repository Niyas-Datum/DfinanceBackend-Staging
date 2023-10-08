using Dfinance.Core.Domain.Roles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations.Roles
{
    public class MaUserRightConfiguration : IEntityTypeConfiguration<MaUserRight>
    {
        public void Configure(EntityTypeBuilder<MaUserRight> entity)
        {
            entity.ToTable("MaUserRights");
            entity.HasIndex(e => new { e.UserDetailsId, e.PageMenuId }, "IX_MaUserRights").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsEmail).HasColumnName("IsEMail");
            entity.Property(e => e.PageMenuId).HasColumnName("PageMenuID");
            entity.Property(e => e.UserDetailsId).HasColumnName("UserDetailsID");

            entity.HasOne(d => d.PageMenu).WithMany(p => p.MaUserRights)
                .HasForeignKey(d => d.PageMenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaUserRights_MaPageMenu");

            entity.HasOne(d => d.UserDetails).WithMany(p => p.MaUserPagePermisions)
                .HasForeignKey(d => d.UserDetailsId)
                .HasConstraintName("FK_MaUserRights_MaEmployeeDetails");
        }
    }
}
