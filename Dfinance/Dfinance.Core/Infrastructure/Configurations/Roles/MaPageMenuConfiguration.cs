using Dfinance.Core.Domain.Roles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Dfinance.Core.Infrastructure.Configurations.Roles
{
    public class MaPageMenuConfiguration : IEntityTypeConfiguration<MaPageMenu>
    {
        public void Configure(EntityTypeBuilder<MaPageMenu> entity)
        {

            entity.ToTable("MaPageMenu");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ArabicName).HasMaxLength(50);
            entity.Property(e => e.AssemblyName).HasMaxLength(50);
            entity.Property(e => e.FormName).HasMaxLength(50);
            entity.Property(e => e.HelpId).HasColumnName("HelpID");
            entity.Property(e => e.Mdiparent).HasColumnName("MDIParent");
            entity.Property(e => e.MenuPermission)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MenuText)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuValue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.PageTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.ProcedureName).HasMaxLength(30);
            entity.Property(e => e.RefPageId)
                .HasComment("To set the page ID of form, that has to be drilled down or traversed from this main page")
                .HasColumnName("RefPageID");
            entity.Property(e => e.ShortcutKey).HasMaxLength(15);
            entity.Property(e => e.Url)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UrlId)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UrlID");
            //entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

            //entity.HasOne(d => d.RefPage).WithMany(p => p.InverseRefPage)
            //    .HasForeignKey(d => d.RefPageId)
            //    .HasConstraintName("FK_MaPageMenu_MaPageMenu1");

            //entity.HasOne(d => d.Voucher).WithMany(p => p.MaPageMenus)
            //    .HasForeignKey(d => d.VoucherId)
            //    .HasConstraintName("FK_MaPageMenu_FiMaVouchers");
        }
        }
    }

