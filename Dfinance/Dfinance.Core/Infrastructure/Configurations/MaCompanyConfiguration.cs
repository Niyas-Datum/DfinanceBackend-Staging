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

    // this configuration used for Ma-company configuration
    public class MaCompanyConfiguration : IEntityTypeConfiguration<MaCompany>
    {
        public void Configure(EntityTypeBuilder<MaCompany> builder)
        {
            builder.ToTable("MaCompanies");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountBranchId).HasColumnName("AccountBranchID");
            builder.Property(e => e.AddressLineOne).HasMaxLength(150);
            builder.Property(e => e.AddressLineTwo).HasMaxLength(50);
            builder.Property(e => e.ArabicName).HasMaxLength(100);
            builder.Property(e => e.BankCode).HasMaxLength(30);
            builder.Property(e => e.BranchCompanyId).HasColumnName("BranchCompanyID");
            builder.Property(e => e.BulidingNo).HasMaxLength(50);
            builder.Property(e => e.CentralSalesTaxNo).HasMaxLength(50);
            builder.Property(e => e.City).HasMaxLength(20);
            builder.Property(e => e.Company).HasMaxLength(50);
            builder.Property(e => e.ContactPersonId).HasColumnName("ContactPersonID");
            builder.Property(e => e.Country).HasMaxLength(50);
            builder.Property(e => e.CountryCode).HasMaxLength(50);
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.District).HasMaxLength(50);
            builder.Property(e => e.Dl1)
                .HasMaxLength(50)
                .HasColumnName("DL1");
            builder.Property(e => e.Dl2)
                .HasMaxLength(50)
                .HasColumnName("DL2");
            builder.Property(e => e.EmailAddress).HasMaxLength(100);
            builder.Property(e => e.FaxNo).HasMaxLength(20);
            builder.Property(e => e.HocompanyName)
                .HasMaxLength(50)
                .HasColumnName("HOCompanyName");
            builder.Property(e => e.HocompanyNameArabic)
                .HasMaxLength(50)
                .HasColumnName("HOCompanyNameArabic");
            builder.Property(e => e.MobileNo).HasMaxLength(20);
            builder.Property(e => e.Nature)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.Pobox)
                .HasMaxLength(20)
                .HasColumnName("POBox");
            builder.Property(e => e.Province).HasMaxLength(50);
            builder.Property(e => e.Reference).HasMaxLength(30);
            builder.Property(e => e.SalesTaxNo).HasMaxLength(50);
            builder.Property(e => e.TelephoneNo).HasMaxLength(20);
            builder.Property(e => e.UniqueId)
                .HasMaxLength(30)
                .HasColumnName("UniqueID");
            /**
             * @relation: company -> company  relation  
             * @use: Branch company 
             * @connection: one to many 
             * **/
            builder.HasOne(d => d.BranchCompany).WithMany(p => p.InBranchCompany)
                .HasForeignKey(d => d.BranchCompanyId)
                .HasConstraintName("FK_MaCompanies_MaCompanies1");
            /**
             * @relation: company -> employee relation  
             * @use: contact person 
             * @connection: one to many 
             * **/
            
            builder.HasOne(d => d.ContactPerson).WithMany(p => p.MaCompanyContactPeople)
                .HasForeignKey(d => d.ContactPersonId)
                .HasConstraintName("FK_MaCompanies_MaEmployees");
            /**
             * @relation: company -> employee relation  
             * @use: creted by
             * @connection: one to many 
             * **/
            builder.HasOne(d => d.CreatedByConnect).WithMany(p => p.MaCompanyCreatedByConnection)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_MaCompanies_MaEmployees1");
        }
    }
}
