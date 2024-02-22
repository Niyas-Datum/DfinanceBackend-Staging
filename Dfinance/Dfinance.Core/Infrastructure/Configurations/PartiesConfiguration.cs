using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class PartiesConfiguration : IEntityTypeConfiguration<Parties>
    {
        public void Configure(EntityTypeBuilder<Parties> builder)
        {
            builder.ToTable("Parties");
            builder.HasIndex(e => e.Code, "IX_Parties").IsUnique();

            builder.HasIndex(e => e.AccountId, "IX_Parties_1").IsUnique();

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
            builder.Property(e => e.AddressLineOne).HasMaxLength(150);
            builder.Property(e => e.AddressLineTwo).HasMaxLength(150);
            builder.Property(e => e.ArabicName).HasMaxLength(200);
            builder.Property(e => e.AreaId).HasColumnName("AreaID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");
            builder.Property(e => e.BulidingNo).HasMaxLength(50);
            builder.Property(e => e.CentralSalesTaxNo).HasMaxLength(50);
            builder.Property(e => e.City).HasMaxLength(50);
            builder.Property(e => e.Code).HasMaxLength(50);
            builder.Property(e => e.CompanyId).HasColumnName("CompanyID");
            builder.Property(e => e.ContactPerson).HasMaxLength(100);
            builder.Property(e => e.ContactPerson2).HasMaxLength(200);
            builder.Property(e => e.CountryCode).HasMaxLength(50);
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.CreditLimit).HasColumnType("money");
            builder.Property(e => e.District).HasMaxLength(50);
            builder.Property(e => e.Dl1)
                .HasMaxLength(50)
                .HasColumnName("DL1");
            builder.Property(e => e.Dl2)
                .HasMaxLength(50)
                .HasColumnName("DL2");
            builder.Property(e => e.EmailAddress).HasMaxLength(50);
            builder.Property(e => e.FaxNo).HasMaxLength(20);
            builder.Property(e => e.ImagePath)
                .HasMaxLength(200)
                  .IsUnicode(false);
            builder.Property(e => e.MobileNo).HasMaxLength(15);
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Nature)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.PANNo)
                .HasMaxLength(50)
                .HasColumnName("PANNo");
            builder.Property(e => e.PartyCategoryId).HasColumnName("PartyCategoryID");
            builder.Property(e => e.PlaceOfSupply).HasMaxLength(50);
            builder.Property(e => e.Pobox)
                .HasMaxLength(10)
                .HasColumnName("POBox");
            builder.Property(e => e.PriceCategoryId).HasColumnName("PriceCategoryID");
            builder.Property(e => e.Province).HasMaxLength(50);
            builder.Property(e => e.Remarks).HasMaxLength(200);
            builder.Property(e => e.SalesManId).HasColumnName("SalesManID");
            builder.Property(e => e.SalesTaxNo).HasMaxLength(50);
            builder.Property(e => e.Salutation).HasMaxLength(5);
            builder.Property(e => e.TelephoneNo).HasMaxLength(20);
            builder.Property(e => e.TelephoneNo2).HasMaxLength(50);
            builder.HasOne(p => p.FiMaAccount)
             .WithOne(f => f.Parties)
             .HasForeignKey<FiMaAccount>(f => f.Id)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.PriceCategory).WithMany(p => p.Parties)
              .HasForeignKey(d => d.PriceCategoryId)
             .HasConstraintName("FK_Parties_MaPriceCategory");




        }

    }
}


