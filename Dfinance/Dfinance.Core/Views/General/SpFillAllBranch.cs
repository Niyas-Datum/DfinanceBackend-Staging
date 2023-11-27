using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class SpFillAllBranchByIdG
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public string Nature { get; set; }
        public int? ContactPersonID { get; set; }
        public string? AddressLineOne { get; set; }
        public string? AddressLineTwo { get; set; }
        public string? City { get; set; }
        public string Country { get; set; }
        public string? POBox { get; set; }
        public string? TelephoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? FaxNo { get; set; }
        public string? Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }

        public string? SalesTaxNo { get; set; }
        public string? CentralSalesTaxNo { get; set; }
        public string? UniqueID { get; set; }
        public string? Reference { get; set; }
        public string? BankCode { get; set; }
        public string? ImagePath { get; set; }
        public string? DL1 { get; set; }
        public string? DL2 { get; set; }
        public string? ArabicName { get; set; }
        public string? HoCompanyName { get; set; }
        public string? HoCompanyNameArabic { get; set; }
        public string? BulidingNo { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? CountryCode { get; set; }
    }

    public class SpFillAllBranchG
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Nature { get; set; }
    }
}
