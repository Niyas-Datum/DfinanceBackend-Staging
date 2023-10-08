using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations.Employee
{
    public class MaEmployeeDetailConfiguration : IEntityTypeConfiguration<MaEmployeeDetail>
    {
        public void Configure(EntityTypeBuilder<MaEmployeeDetail> entity)
        {
            entity.ToTable("MaEmployeeDetails");
            {

                entity.HasKey(e => e.Id).HasName("PK_MaEmployeeRoles");

                entity.ToTable(tb => tb.HasTrigger("IsMainSetTrigger"));

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.MaRoleId).HasColumnName("MaRoleID");
                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.HasOne(d => d.EmployeeBranch).WithMany(p => p.CompanyEmployeeDetails)
                        .HasForeignKey(d => d.BranchId);
                //.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_MaEmployeeRoles_MaCompanies");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmployeeBranchDetCreatedUserList)
                        .HasForeignKey(d => d.CreatedBy)
                        .HasPrincipalKey(x => x.CreatedBy);
                        //.OnDelete(DeleteBehavior.ClientSetNull)
                        //.HasConstraintName("FK_MaEmployeeRoles_MaEmployees");

                entity.HasOne(d => d.Department).WithMany(p => p.MaEmployeeDetails)
                        .HasForeignKey(d => d.DepartmentId)
                        .HasPrincipalKey(x => x.Id);
                       // .OnDelete(DeleteBehavior.ClientSetNull);
                        //.HasConstraintName("FK_MaEmployeeDetails_MaDepartments");

                
                        //.HasConstraintName("FK_MaEmployeeDetails_MaEmployees1");
                /*

                entity.HasOne(d => d.MaRole).WithMany(p => p.MaEmployeeDetails)
                        .HasForeignKey(d => d.MaRoleId)
                        .HasConstraintName("FK_MaEmployeeDetails_MaRoles");

                entity.HasOne(d => d.Supervisor).WithMany(p => p.MaEmployeeDetailSupervisors)
                        .HasForeignKey(d => d.SupervisorId)
                        .HasConstraintName("FK_MaEmployeeDetails_MaEmployees");*/
            }

        }
        }

}
