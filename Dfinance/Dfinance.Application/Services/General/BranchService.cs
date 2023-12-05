using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Application.Dto;
using Microsoft.Data.SqlClient;
using Dfinance.Application.Services.General.Interface;

namespace Dfinance.Application.Services.General
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

        /******Fill all branch dropdown******/

        public CommonResponse GetBranchesDropDown()
        {

            var data = _context.SpMacompanyFillallbranch.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillallbranch'").ToList();

            return CommonResponse.Ok(data);
        }

        /*****************  Fill All Branch Details   *********************/
        public CommonResponse FillAllBranch()
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

        /*************************  Fill Branch Details by ID   ****************************/
        public CommonResponse FillBranchByID(int Id)
        {
            try
            {
                var company = _context.MaBranches.Where(i => i.Id == Id).
                    Select(i => i.Id).
                    SingleOrDefault();
                if (company == null)
                {
                    return CommonResponse.NotFound("This Branch is Not Found");
                }
                string criteria = "FillCompanyWithID";
                var result = _context.SpFillAllBranchByIdG.FromSqlRaw($"EXEC spCompany @Criteria='{criteria}',@ID='{Id}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        /**************************  Save Branch *************************/
        public CommonResponse SaveBranch(BranchDto branchDto)
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
                    branchDto.CompanyName,
                    branchDto.ContactPerson.Id,
                    branchDto.BranchType.Key,
                    branchDto.AddressLineOne,
                    branchDto.AddressLineTwo,
                    branchDto.City,
                    branchDto.Country.Value,
                    branchDto.PObox,
                    branchDto.Telephone,
                    branchDto.Mobile,
                    branchDto.EmailAddress,
                    branchDto.FaxNo,
                    branchDto.Remarks,
                    CreatedBy,
                    BranchCompanyId,
                    branchDto.VATNo,
                    branchDto.CentralSalesTaxNo,
                    branchDto.UniqueId,
                    branchDto.Reference,
                    branchDto.BankCode,
                    branchDto.Dl1,
                    branchDto.Dl2,
                    branchDto.ArabicName,
                    branchDto.HocompanyName,
                    branchDto.HocompanyNameArabic,
                    branchDto.BuildingNo,
                    branchDto.District,
                    branchDto.Province,
                    branchDto.CountryCode,
                    branchDto.Active,
                    newIdParam);

                var NewId = newIdParam.Value;
                return CommonResponse.Created("Branch " + branchDto.CompanyName + " is Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        /*************************  Update Branch  ************************/
        public CommonResponse UpdateBranch(BranchDto branchDto, int Id)
        {
            try
            {                              
                var company = _context.MaBranches.Where(i => i.Id == Id).
                    Select(i => i.Id).
                    SingleOrDefault();
                if (company == null)
                {                   
                    return CommonResponse.NotFound("This Branch is Not Found");
                }
                else
                {
                    int CreatedBy = _authService.GetId().Value;
                    string criteria = "UpdateMaCompanies";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spCompany @ID='{Id}',@Criteria='{criteria}',@Company='{branchDto.CompanyName}',@ContactPersonID='{branchDto.ContactPerson.Id}',@Nature='{branchDto.BranchType.Key}',@AddressLineOne='{branchDto.AddressLineOne}',@AddressLineTwo='{branchDto.AddressLineTwo}',@City='{branchDto.City}',@Country='{branchDto.Country.Value}',@POBox='{branchDto.PObox}',@TelephoneNo='{branchDto.Telephone}',@MobileNo='{branchDto.Mobile}',@EmailAddress='{branchDto.EmailAddress}',@FaxNo='{branchDto.FaxNo}',@Remarks='{branchDto.Remarks}',@CreatedBy='{CreatedBy}',@SalesTaxNo='{branchDto.VATNo}',@CentralSalesTaxNo='{branchDto.CentralSalesTaxNo}',@UniqueID='{branchDto.UniqueId}',@Reference='{branchDto.Reference}',@BankCode='{branchDto.BankCode}',@DL1='{branchDto.Dl1}',@DL2='{branchDto.Dl2}',@ArabicName='{branchDto.ArabicName}',@HOCompanyName='{branchDto.HocompanyName}',@HOCompanyNameArabic='{branchDto.HocompanyNameArabic}',@BulidingNo='{branchDto.BuildingNo}',@District='{branchDto.District}',@Province='{branchDto.Province}',@CountryCode='{branchDto.CountryCode}',@CreatedOn='{DateTime.Now}',@ActiveFlag='{branchDto.Active}'");
                    return CommonResponse.Ok("Branch Updated Successfully");
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        /*******************************   Delete Branch   *************************/
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
                        msg = "Branch " + company + " Is Deleted";
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

       
    }
}
