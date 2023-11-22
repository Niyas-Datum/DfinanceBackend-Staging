using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Views;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Application.Dto;
using Dfinance.Core.Views.General;
using Microsoft.Data.SqlClient;
using Dfinance.Application.Dto.Common;

namespace Dfinance.Application.General.Services
{
    public class BranchService : IBranchService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
       
        public BranchService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
         
        }
        //GetBranches for Login
        public CommonResponse GetBranches()
        {
            try
            {
                var companies = _context.MaBranches
                    .Select(c => new DropdownDto
                    {
                        Id = c.Id,
                        Value = c.Company,

                    })
                    .ToList();

                return CommonResponse.Ok(companies);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //Fill all branch dropdown
        public CommonResponse Fillallbranch()
        {         
            var data = _context.SpMacompanyFillallbranch.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillallbranch'").ToList();

            var maBranches = data.Select(item => new SpMacompanyFillallbranch
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
           
            return CommonResponse.Ok(maBranches);
        }
        public CommonResponse SaveBranch(MaCompanyDto companyDto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int BranchCompanyId = _authService.GetBranchId().Value;
                string criteria = "InsertMaCompanies";
                SqlParameter newIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC spCompany @Criteria={0},@BranchID={1},@Company={2}," +
                    "@ContactPersonID={3},@Nature={4},@AddressLineOne={5},@AddressLineTwo={6},@City={7},@Country={8}," +
                    "@POBox={9},@TelephoneNo={10},@MobileNo={11},@EmailAddress={12},@FaxNo={13},@Remarks={14},@CreatedBy={15}," +
                    "@CompanyID={16},@SalesTaxNo={17},@CentralSalesTaxNo={18},@UniqueID={19},@Reference={20},@BankCode={21}," +
                    "@DL1={22},@DL2={23},@ArabicName={24},@HOCompanyName={25},@HOCompanyNameArabic={26},@BulidingNo={27}," +
                    "@District={28},@Province={29},@CountryCode={30},@ActiveFlag={31},@NewID={32} OUTPUT",
                    criteria,
                    BranchCompanyId,
                    companyDto.Company,
                    companyDto.ContactPersonId,
                    companyDto.Nature,
                    companyDto.AddressLineOne,
                    companyDto.AddressLineTwo,
                    companyDto.City,
                    companyDto.Country,
                    companyDto.Pobox,
                    companyDto.TelephoneNo,
                    companyDto.MobileNo,
                    companyDto.EmailAddress,
                    companyDto.FaxNo,
                    companyDto.Remarks,
                    CreatedBy,
                    BranchCompanyId,
                    companyDto.SalesTaxNo,
                    companyDto.CentralSalesTaxNo,
                    companyDto.UniqueId,
                    companyDto.Reference,
                    companyDto.BankCode,
                    companyDto.Dl1,
                    companyDto.Dl2,
                    companyDto.ArabicName,
                    companyDto.HocompanyName,
                    companyDto.HocompanyNameArabic,
                    companyDto.BulidingNo,
                    companyDto.District,
                    companyDto.Province,
                    companyDto.CountryCode,
                    companyDto.ActiveFlag,
                    newIdParam);

                var NewId = newIdParam.Value;
                return CommonResponse.Created("Branch "+companyDto.Company + " is Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse UpdateBranch(MaCompanyDto companyDto, int Id)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                if (Id == 0)
                    return CommonResponse.Error("Branch Not Found");
                else
                {
                    string criteria = "UpdateMaCompanies";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spCompany @ID='{Id}',@Criteria='{criteria}',@Company='{companyDto.Company}',@ContactPersonID='{companyDto.ContactPersonId}',@Nature='{companyDto.Nature}',@AddressLineOne='{companyDto.AddressLineOne}',@AddressLineTwo='{companyDto.AddressLineTwo}',@City='{companyDto.City}',@Country='{companyDto.Country}',@POBox='{companyDto.Pobox}',@TelephoneNo='{companyDto.TelephoneNo}',@MobileNo='{companyDto.MobileNo}',@EmailAddress='{companyDto.EmailAddress}',@FaxNo='{companyDto.FaxNo}',@Remarks='{companyDto.Remarks}',@CreatedBy='{CreatedBy}',@SalesTaxNo='{companyDto.SalesTaxNo}',@CentralSalesTaxNo='{companyDto.CentralSalesTaxNo}',@UniqueID='{companyDto.UniqueId}',@Reference='{companyDto.Reference}',@BankCode='{companyDto.BankCode}',@DL1='{companyDto.Dl1}',@DL2='{companyDto.Dl2}',@ArabicName='{companyDto.ArabicName}',@HOCompanyName='{companyDto.HocompanyName}',@HOCompanyNameArabic='{companyDto.HocompanyNameArabic}',@BulidingNo='{companyDto.BulidingNo}',@District='{companyDto.District}',@Province='{companyDto.Province}',@CountryCode='{companyDto.CountryCode}',@CreatedOn='{DateTime.Now}',@ActiveFlag='{companyDto.ActiveFlag}'");
                    return CommonResponse.Ok("Branch Updated Successfully");
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
                try
                {
                    string msg = null;
                    var company = _context.MaBranches.Where(i => i.Id == Id).
                        Select(i => i.Company).
                        SingleOrDefault();
                    if (company == null)
                    {
                        msg = "This Branch is Not Found";
                    }
                    else
                    {
                        int Mode = 3;
                        var result = _context.Database.ExecuteSqlRaw($"EXEC spCompany @Mode='{Mode}',@ID='{Id}'");
                        msg = "Branch "+company + " Is Deleted";
                    }
                    return CommonResponse.Ok(msg);
                }
                catch (Exception ex)
                {
                    return CommonResponse.Error(ex);
                }

               
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetAllBranch()
        {
            try
            {
                string criteria = "FillCompanyMaster";               
                var result = _context.SpFillAllBranch.FromSqlRaw($"EXEC spCompany @Criteria='{criteria}'").ToList();               
                return CommonResponse.Ok(result);                        
               
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse GetBranchByID(int Id)
        {
            try
            {
                string criteria = "FillCompanyWithID";                
                var result = _context.SpFillAllBranchByIdG.FromSqlRaw($"EXEC spCompany @Criteria='{criteria}',@ID='{Id}'").ToList();
                
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
