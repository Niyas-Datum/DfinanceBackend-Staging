using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Views;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Application.Dto;

namespace Dfinance.Application.General.Services
{
    public class BranchService : IBranchService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ICountryDropDownService _countryDropDownService;
        public BranchService(DFCoreContext context, IAuthService authService/*,ICountryDropDownService countryDropDownService*/)
        {
            _context = context;
            _authService = authService;
           // _countryDropDownService = countryDropDownService;
        }
        //GetBranches for Login
        public CommonResponse GetBranches()
        {
            try
            {
                var companies = _context.MaBranches
                    .Select(c => new BranchDropdownDto
                    {
                        Id = c.Id,
                        BranchName = c.Company,

                    })
                    .ToList();

                return CommonResponse.Ok(companies);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //Fill all branch

        public CommonResponse Fillallbranch()
        {
            // Execute the stored procedure
            var data = _context.SpMacompanyFillallbranch.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillallbranch'").ToList();

            // Extract Id and Name from the result of the stored procedure
            var maBranches = data.Select(item => new SpMacompanyFillallbranch
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();

            // Return the result as a CommonResponse
            return CommonResponse.Ok(maBranches);
        }
        public CommonResponse SaveBranch(MaCompanyDto companyDto)
        {
            try
            {
                companyDto.CreatedBy = _authService.GetId().Value;
                MaCompany companies = new MaCompany
                {
                    Company = companyDto.Company,
                    ContactPersonId = 118,
                    Nature = companyDto.Nature,
                    AddressLineOne = companyDto.AddressLineOne,
                    AddressLineTwo = companyDto.AddressLineTwo,
                    City = companyDto.City,
                    Country = companyDto.Country,
                    Pobox = companyDto.Pobox,
                    TelephoneNo = companyDto.TelephoneNo,
                    MobileNo = companyDto.MobileNo,
                    EmailAddress = companyDto.EmailAddress,
                    FaxNo = companyDto.FaxNo,
                    Remarks = companyDto.Remarks,
                    CreatedBy = companyDto.CreatedBy,
                    BranchCompanyId = 1,
                    SalesTaxNo = companyDto.SalesTaxNo,
                    CentralSalesTaxNo = companyDto.CentralSalesTaxNo,
                    UniqueId = companyDto.UniqueId,
                    Reference = companyDto.Reference,
                    BankCode = companyDto.BankCode,
                    Dl1 = companyDto.Dl1,
                    Dl2 = companyDto.Dl2,
                    ArabicName = companyDto.ArabicName,
                    HocompanyName = companyDto.HocompanyName,
                    HocompanyNameArabic = companyDto.HocompanyNameArabic,
                    BulidingNo = companyDto.BulidingNo,
                    District = companyDto.District,
                    Province = companyDto.Province,
                    CountryCode = companyDto.CountryCode,
                    ActiveFlag = companyDto.ActiveFlag,
                    CreatedOn= companyDto.CreatedOn
                };
                             
                string criteria = "InsertMaCompanies";
                var result = _context.SpMaCompanyC.FromSqlRaw($"EXEC spCompany @Criteria='{criteria}',@BranchID='{companies.BranchCompanyId}',@Company='{companies.Company}',@ContactPersonID='{companies.ContactPersonId}',@Nature='{companies.Nature}',@AddressLineOne='{companies.AddressLineOne}',@AddressLineTwo='{companies.AddressLineTwo}',@City='{companies.City}',@Country='{companies.Country}',@POBox='{companies.Pobox}',@TelephoneNo='{companies.TelephoneNo}',@MobileNo='{companies.MobileNo}',@EmailAddress='{companies.EmailAddress}',@FaxNo='{companies.FaxNo}',@Remarks='{companies.Remarks}',@CreatedBy='{companies.CreatedBy}',@CompanyID='{1}',@SalesTaxNo='{companies.SalesTaxNo}',@CentralSalesTaxNo='{companies.CentralSalesTaxNo}',@UniqueID='{companies.UniqueId}',@Reference='{companies.Reference}',@BankCode='{companies.BankCode}',@DL1='{companies.Dl1}',@DL2='{companies.Dl2}',@ArabicName='{companies.ArabicName}',@HOCompanyName='{companies.HocompanyName}',@HOCompanyNameArabic='{companies.HocompanyNameArabic}',@BulidingNo='{companies.BulidingNo}',@District='{companies.District}',@Province='{companies.Province}',@CountryCode='{companies.CountryCode}',@ActiveFlag='{companies.ActiveFlag}'").ToList();
                var NewId = result.Select(i => new SpMaCompanyC
                {
                    NewID = i.NewID
                }).ToList();
                return CommonResponse.Created(companies);
               
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse UpdateBranch(MaCompanyDto companyDto,int Id)
        {
            try
            {
                companyDto.CreatedBy = _authService.GetId().Value;
                if (Id == 0)
                    return CommonResponse.Error("Branch Not Found");

                else
                {
                    MaCompany companies = new MaCompany
                    {
                        Company = companyDto.Company,
                        ContactPersonId = 118,
                        Nature = companyDto.Nature,
                        AddressLineOne = companyDto.AddressLineOne,
                        AddressLineTwo = companyDto.AddressLineTwo,
                        City = companyDto.City,
                        Country = companyDto.Country,
                        Pobox = companyDto.Pobox,
                        TelephoneNo = companyDto.TelephoneNo,
                        MobileNo = companyDto.MobileNo,
                        EmailAddress = companyDto.EmailAddress,
                        FaxNo = companyDto.FaxNo,
                        Remarks = companyDto.Remarks,
                        CreatedBy = companyDto.CreatedBy,
                        //BranchCompanyId = 1,
                        SalesTaxNo = companyDto.SalesTaxNo,
                        CentralSalesTaxNo = companyDto.CentralSalesTaxNo,
                        UniqueId = companyDto.UniqueId,
                        Reference = companyDto.Reference,
                        BankCode = companyDto.BankCode,
                        Dl1 = companyDto.Dl1,
                        Dl2 = companyDto.Dl2,
                        ArabicName = companyDto.ArabicName,
                        HocompanyName = companyDto.HocompanyName,
                        HocompanyNameArabic = companyDto.HocompanyNameArabic,
                        BulidingNo = companyDto.BulidingNo,
                        District = companyDto.District,
                        Province = companyDto.Province,
                        CountryCode = companyDto.CountryCode,
                        CreatedOn=companyDto.CreatedOn,
                        ActiveFlag=companyDto.ActiveFlag
                        
                    };
                    string criteria = "UpdateMaCompanies";
                    var result = _context.Database.ExecuteSqlRawAsync($"EXEC spCompany @ID='{Id}',@Criteria='{criteria}',@Company='{companies.Company}',@ContactPersonID='{companies.ContactPersonId}',@Nature='{companies.Nature}',@AddressLineOne='{companies.AddressLineOne}',@AddressLineTwo='{companies.AddressLineTwo}',@City='{companies.City}',@Country='{companies.Country}',@POBox='{companies.Pobox}',@TelephoneNo='{companies.TelephoneNo}',@MobileNo='{companies.MobileNo}',@EmailAddress='{companies.EmailAddress}',@FaxNo='{companies.FaxNo}',@Remarks='{companies.Remarks}',@CreatedBy='{companies.CreatedBy}',@SalesTaxNo='{companies.SalesTaxNo}',@CentralSalesTaxNo='{companies.CentralSalesTaxNo}',@UniqueID='{companies.UniqueId}',@Reference='{companies.Reference}',@BankCode='{companies.BankCode}',@DL1='{companies.Dl1}',@DL2='{companies.Dl2}',@ArabicName='{companies.ArabicName}',@HOCompanyName='{companies.HocompanyName}',@HOCompanyNameArabic='{companies.HocompanyNameArabic}',@BulidingNo='{companies.BulidingNo}',@District='{companies.District}',@Province='{companies.Province}',@CountryCode='{companies.CountryCode}',@CreatedOn='{companies.CreatedOn}',@ActiveFlag='{companyDto.ActiveFlag}'");

                    return CommonResponse.Ok(companies);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
       public CommonResponse DeleteBranch(int Id)
        {
            try
            {
                int Mode = 6;
                string msg = "Branch is Suspended";
                var result = _context.Database.ExecuteSqlRawAsync($"EXEC spCompany @Mode='{Mode}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
               return CommonResponse.Error(ex);
            }
        }
    }
}
