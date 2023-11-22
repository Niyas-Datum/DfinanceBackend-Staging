using System;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto
{
    public class MaCompanyDto
    {
        public MaCompanyDto()
        {
            ActiveFlag = 1;           
        }
        [Required(ErrorMessage = "Name is mandatory!!")]        
        public string Company { get; set; }

        [Required(ErrorMessage = "ContactPersonId is required.")]        
        public int? ContactPersonId { get; set; }

        [Required(ErrorMessage = "Type is Mandatory!!")]
        
        public string Nature { get; set; }

        [Required(ErrorMessage = "Address line 1 is mandatory!!")]       
        public string AddressLineOne { get; set; }
       
        public string? AddressLineTwo { get; set; }       
        public string? City { get; set; }

        [Required(ErrorMessage = "Country is Mandatory!!")]        
        public string Country { get; set; }
       
        public string? Pobox { get; set; }                   
        public string? TelephoneNo { get; set; }      
        public string? MobileNo { get; set; }        
        public string? EmailAddress { get; set; }        
        public string? FaxNo { get; set; }      
        public string? Remarks { get; set; }     
               
        public string? SalesTaxNo { get; set; }       
        public string? CentralSalesTaxNo { get; set; }        
        public string? UniqueId { get; set; }
        public string? Reference { get; set; }       
        public string? BankCode { get; set; }       
        public string? Dl1 { get; set; }        
        public string? Dl2 { get; set; }       
        public string? ArabicName { get; set; }     
        public string? HocompanyName { get; set; }        
        public string? HocompanyNameArabic { get; set; }      
        public string? BulidingNo { get; set; }        
        public string? District { get; set; }       
        public string? Province { get; set; }       
        public string? CountryCode { get; set; }        
       public byte ActiveFlag { get; set; }
    }
}
