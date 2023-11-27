using Dfinance.Application.Dto.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto
{
    public class BranchDto
    {
        public BranchDto()
        {
            Active = 1;           
        }
        [Required(ErrorMessage = "Company Name is mandatory!!")]        
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "ContactPerson is required.")]        
        public DropDownDtoName ContactPerson{ get; set; }

        [Required(ErrorMessage = "Type is Mandatory!!")]        
        public DropDownDtoNature BranchType { get; set; }

        [Required(ErrorMessage = "Address line 1 is mandatory!!")]       
        public string AddressLineOne { get; set; }
       
        public string? AddressLineTwo { get; set; }       
        public string? City { get; set; }

        [Required(ErrorMessage = "Country is Mandatory!!")]        
        public DropdownDto Country { get; set; }
       
        public string? PObox { get; set; }                   
        public string? Telephone { get; set; }      
        public string? Mobile { get; set; }        
        public string? EmailAddress { get; set; }        
        public string? FaxNo { get; set; }      
        public string? Remarks { get; set; }                    
        public string? VATNo { get; set; }       
        public string? CentralSalesTaxNo { get; set; }        
        public string? UniqueId { get; set; }
        public string? Reference { get; set; }       
        public string? BankCode { get; set; }       
        public string? Dl1 { get; set; }        
        public string? Dl2 { get; set; }       
        public string? ArabicName { get; set; }     
        public string? HocompanyName { get; set; }        
        public string? HocompanyNameArabic { get; set; }      
        public string? BuildingNo { get; set; }        
        public string? District { get; set; }       
        public string? Province { get; set; }       
        public string? CountryCode { get; set; }        
       public byte Active { get; set; }
    }
}
