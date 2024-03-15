using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class MaCompany
    {
        //primary key
        public int Id { get; set; }

        public string Company { get; set; } = null!;

        //FK
        public int? ContactPersonId { get; set; }

        public string Nature { get; set; } = null!;

        public string AddressLineOne { get; set; } = null!;

        public string? AddressLineTwo { get; set; }

        public string? City { get; set; }

        public string Country { get; set; } = null!;

        public string? Pobox { get; set; }

        public string? TelephoneNo { get; set; }

        public string? MobileNo { get; set; }

        public string? EmailAddress { get; set; }

        public string? FaxNo { get; set; }

        public string? Remarks { get; set; }

        //FK
        public int? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte ActiveFlag { get; set; }

        //FK
        public int? BranchCompanyId { get; set; }

        public string? SalesTaxNo { get; set; }

        public string? CentralSalesTaxNo { get; set; }

        public string? UniqueId { get; set; }

        public string? Reference { get; set; }

        public string? BankCode { get; set; }

        public string? Dl1 { get; set; }

        public string? Dl2 { get; set; }

        public bool? LockSystem { get; set; }

        public string? ArabicName { get; set; }

        public string? HocompanyName { get; set; }

        public string? HocompanyNameArabic { get; set; }

        public int? AccountBranchId { get; set; }

        public string? BulidingNo { get; set; }

        public string? District { get; set; }

        public string? Province { get; set; }

        public string? CountryCode { get; set; }

        //one to many realtionship self:  BranchCompany ID
        public virtual MaCompany? BranchCompany { get; set; }

        //contact person FK
        public virtual MaEmployee? ContactPerson { get; set; }

        public virtual ICollection<MaEmployee> CreatedBranchEmployees { get; set; }

        public virtual ICollection<MaCompany>? InBranchCompany { get; set; } = new List<MaCompany>();

        public virtual MaEmployee? CreatedByConnect { get; set; }

        public virtual ICollection<MaDepartment>? MaDepartments { get; set; } = new List<MaDepartment>();

        //rel: branch to empoyee details - 
        public virtual ICollection<MaEmployeeDetail>? CompanyEmployeeDetails { get; set; }

        //relationship with CostCategory
        public virtual ICollection<CostCategory>? BranchCostCategory { get; set; }

        //relationship with MaArea
        public virtual ICollection<MaArea>? BranchIdArea { get; set; }
        //relationship with currency
        public virtual ICollection<Currency> Currencies { get; set; }
        //relationship with tblfinyear
        public ICollection<TblMaFinYear> TblMaFinYears { get; set; }

     //relationship with ItemUnits
        public ICollection<ItemUnits> ItemUnit {  get; set; }

        //relationship with BranchItems
        public ICollection<BranchItems> BranchItems { get; set; }

        //relationship with PriceCategory
        public ICollection<MaPriceCategory> PriceCategories { get; set; }
        //relation with FiMaVoucher
        public virtual ICollection<Voucher>? FiMaVouchers { get; set; }
        public virtual ICollection<Locations> Locations { get; set; }
        //relation with Location Types
        public virtual ICollection<LocationTypes> LocationTypes { get; set; }
        //relation with LocationBranchList
        public virtual ICollection<LocationBranchList> LocationBranchList { get; set; }
        public virtual ICollection<FiTransaction> FiTransactions { get; set; }
        
    }
}