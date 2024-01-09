using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class FiMaAccountGroupConfiguration : IEntityTypeConfiguration<FiMaAccountGroup>
    {
        public void Configure(EntityTypeBuilder<FiMaAccountGroup> builder)
        {
           
            builder.ToTable("FiMaAccountGroup");

           
            builder.Property(e => e.Id).HasColumnName("ID");

           
            builder.Property(e => e.Description).HasMaxLength(50);

            // Define many-to-many relationship between FiMaAccountGroup and FiMaSubGroup
            builder.HasMany<FiMaSubGroup>(g => g.Ids) // Assuming FiMaAccountGroup has a property Ids of type ICollection<FiMaSubGroup>
                .WithMany(s => s.Lists) // Assuming FiMaSubGroup has a property Lists of type ICollection<FiMaAccountGroup>
                .UsingEntity<Dictionary<string, object>>(
                    "FiSubGroupList",
                    l => l.HasOne<FiMaSubGroup>().WithMany().HasForeignKey("Id").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FiSubGroupList_FiMaSubGroup"),
                    r => r.HasOne<FiMaAccountGroup>().WithMany().HasForeignKey("ListId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubGroupList_AccountGroupMast"),
                    j =>
                    {
                        j.HasKey("ListId", "Id").HasName("PK_SubGroupList");

                        j.ToTable("FiSubGroupList");

                        j.IndexerProperty<int>("ListId").HasColumnName("ListID");
                        j.IndexerProperty<int>("Id").HasColumnName("ID");
                    });
        }
    }
}
