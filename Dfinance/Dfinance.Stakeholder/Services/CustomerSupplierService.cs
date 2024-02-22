using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace Dfinance.Stakeholder.Services
{
    public class CustomerSupplierService : ICustomerSupplierService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IChartOfAccountsService _accountService;

        private readonly ICustomerService _customerService;
        private readonly ICsDeliveryService _deliveryService;
        //private readonly IChartOfAccountsService _chartOfAccountsService;
        public CustomerSupplierService(DFCoreContext context, IAuthService authService, IChartOfAccountsService accountService, ICustomerService customerService,
                                        ICsDeliveryService deliveryService)
        {
            _context = context;
            _authService = authService;
           _accountService = accountService;   
           _customerService =customerService;
            _deliveryService = deliveryService;
    }
        /// <summary>
        /// load customer supplier stype 
		/// O/P =>  [ customer, supplier] 
        /// </summary>
        /// <returns></returns>
        public CommonResponse FillPartyType()
        {
            try
            {
                string criteria = "FillPartyType";
                var result = _context.DropDownViewDesc.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
       

        
        /// <summary>
        /// generate code - (new form)
        /// </summary>
        /// <returns></returns>

        public CommonResponse GetCode()
        {
            try
            {
                string criteria = "FillNextCode";
                var result = _context.NextCodeView
                    .FromSqlRaw($"EXEC PartyMasterSP @Criteria='{criteria}'")
                     .AsEnumerable()
                     .FirstOrDefault();
                return CommonResponse.Ok(result);
            }catch (Exception ex)
            {   return CommonResponse.Error(ex);      }
        }
        /// <summary>
        /// Fill Category - (General form) 
		/// customer supplier category 
        /// </summary>
        /// <returns>Id,Value</returns>
        public CommonResponse FillCategory()
        {
            try
            {
                string criteria = "PartyCategory";
                var result = _context.DropDownViewValue.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Add/Update Customer supplier General Data
        /// </summary>
        /// <param name="generalDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CommonResponse SaveGen(GeneralDto generalDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;
                var nature = "S";
                var path = "";
                if (generalDto.Type.Id == 1)
                {
                    nature = "C";
                }

                //PHOTO UPLOAD
                if (generalDto.Image != null)
                {
                    //path = UploadImage(generalDto.Name, generalDto.Image);
                }

              
                if (generalDto.CustomerDetails.PriceCategory.Id == 0) 
                { 
                    generalDto.CustomerDetails.PriceCategory.Id = null; 
                }
               
                
                if (generalDto.Id == null || generalDto.Id == 0)
                {
                    if (generalDto.LetSystemGenNewAccForParty == true)
                    {
                        var accountCat = _context.FiMaAccountCategory
                            .Where(x => x.Id == generalDto.Type.Id)
                            .Select(x => new DropDownDtoName { Id = x.Id })
                            .FirstOrDefault();

                        var accGroup = _context.FiMaAccounts
                            .Where(x => x.Id == generalDto.AccountGroup.Id)
                            .Select(x => new DropDownDtoName { Id = x.Id })
                            .FirstOrDefault();

                        if (accountCat == null || accGroup == null)
                        {

                            return CommonResponse.Error("Error: Unable to retrieve account category or account group.");
                        }

                        ChartOfAccountsDto accountsDto = new ChartOfAccountsDto
                        {
                            AccountCode = generalDto.Code,
                            AccountName = generalDto.Name,
                            Active = true,
                            IsGroup = false,
                            MaintainBillwise = true,
                            Group = accGroup,
                            AccountCategory = accountCat,
                            AlternateName = generalDto.ArabicName,
                            SubGroup = new DropDownDtoName { Id = 1, Name = "Null" },
                            TrackCollection = false,
                            PreventExtraPay = false,
                            MaintainIteamWise = false,
                            Narration = null,
                            MaintainCostCentre = null,

                        };
                        var res = _accountService.SaveAccount(accountsDto);
                        int? accoId = res.Data?.GetType().GetProperty("Id")?.GetValue(res.Data, null) as int?;
                        if (accoId.HasValue)
                        {

                            generalDto.Account.Id = accoId.Value;
                        }
                    }


                    string criteria = "InsertPertiesWeb";
                    SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    _context.Database.ExecuteSqlRaw("EXEC PartyMasterSP @Criteria={0},@Name={1},@ContactPerson={2},@Nature={3},@AddressLineOne={4}," +
                        "@AddressLineTwo={5},@City ={6},@Country={7},@POBox={8},@Remarks={9},@TelephoneNo={10},@MobileNo={11},@EmailAddress={12}," +
                        "@FaxNo={13},@Active={14},@PANNo={15},@SalesTaxNo={16},@CentralSalesTaxNo={17},@Salutation={18},@ContactPerson2={19}," +


                        "@TelephoneNo2={20},@CompanyID={21},@BranchID={22},@CreatedBy={23},@CreatedOn={24},@IsMultiNature={25},@Code={26},@AccountID={27}," +
                        "@ImagePath={28},@DL1={29},@DL2={30},@AreaID={31},@CreditLimit={32},@PriceCategoryID={33},@PlaceOfSupply={34}," +
                        "@ArabicName={35},@SalesManID={36},@PartyCategoryID={37},@BulidingNo={38},@District={39},@Province={40}," +
                        "@CountryCode={41},@DistrictArabic={42}, @CityArabic={43},@ProvinceArabic={44} ,@NewID={45} OUTPUT",
                      criteria,
                       generalDto.Name,
                       generalDto.ContactPersonName,
                       nature,
                       generalDto.AddressLineOne,
                       generalDto.AddressArabic,
                       generalDto.City,
                       generalDto.Country,
                       generalDto.POBox,
                       generalDto.Remarks,
                       generalDto.TelephoneNo,
                       generalDto.MobileNo,
                       generalDto.EmailAddress,
                       generalDto.FaxNo,
                       generalDto.Active,
                       generalDto.PANNo,
                       generalDto.VATNO,
                       generalDto.CentralSalesTaxNo,
                       generalDto.Salutation,
                       generalDto.ContactPerson2,
                       generalDto.TelephoneNo2,
                       createdBranchId,
                       createdBranchId,
                       createdBy,
                       createdOn,
                       generalDto.ActasSupplierAlso,
                       generalDto.Code,
                       generalDto.Account.Id,
                       path,
                       generalDto.CustomerDetails.DL1,
                       generalDto.CustomerDetails.DL2,
                       generalDto.Area.Id,
                       generalDto.CreditLimit,
                       generalDto.CustomerDetails.PriceCategory.Id,                       
                       generalDto.CustomerDetails.PlaceOfSupply.Value,
                       generalDto.ArabicName,
                       generalDto.SalesMan,
                       generalDto.Category.Id,
                       generalDto.BulidingNo,
                       generalDto.District,
                       generalDto.Province,
                       generalDto.CountryCode,
                       generalDto.DistrictArabic,
                       generalDto.CityArabic,
                       generalDto.ProvinceArabic,
                       newIdparam
                   );
                    int PartyId = (int)newIdparam.Value;

                    //CUSTOMER CEHCKING 
                    if (PartyId > 0 && generalDto.Type.Id == 1)
                    {
                        if (!AddCustomerInfo(generalDto.CustomerDetails, generalDto.CreditPeriod, PartyId))
                        { transaction.Rollback(); return CommonResponse.Error("AddCustomerInfo Error"); }
                    
                    }
                    //ADD DELIVERY INFO
                            if(generalDto.DeliveryDetails != null && generalDto.DeliveryDetails.Count > 0)
                        {
                            bool flag = false;
                            foreach (var deliverydetail in generalDto.DeliveryDetails)
                            {
                                if (!AddDeliveryDetails(deliverydetail, PartyId))
                                {
                                    flag = true;
                                }
                            }

                            if (flag)
                            {
                                transaction.Rollback(); return CommonResponse.Error("Delivery Details Error");
                            }
                        }
                      
                    
                    //if (generalDto.DeliveryDetails != null && generalDto.DeliveryDetails.Count > 0)
                    //{
                    //    foreach (var deliverydetails in generalDto.DeliveryDetails)
                    //    {
                    //       // _deliveryDetailsService.SaveDeliveryDetails(deliverydetails, PartyId);

                    //    }
                    //}
                    transaction.Commit();
                    return CommonResponse.Created($"Saved with PartyId: {PartyId}");
                }

                else
                {
                   
                    var party = _context.Parties.Find(generalDto.Id);

                   if (party != null)
                    {
                        string criteria = "UpdateWeb";

                     _context.Database.ExecuteSqlRaw("EXEC PartyMasterSP @Criteria={0},@Name={1},@ContactPerson={2},@Nature={3},@AddressLineOne={4}," +
                            "@AddressLineTwo={5},@City ={6},@Country={7},@POBox={8},@Remarks={9},@TelephoneNo={10},@MobileNo={11},@EmailAddress={12}," +
                            "@FaxNo={13},@Active={14},@PANNo={15},@SalesTaxNo={16},@CentralSalesTaxNo={17},@Salutation={18},@ContactPerson2={19}," +
                            "@TelephoneNo2={20},@CompanyID={21},@BranchID={22},@CreatedBy={23},@CreatedOn={24},@IsMultiNature={25},@Code={26}," +
                            "@ImagePath={28},@DL1={29},@DL2={30},@AreaID={31},@CreditLimit={32},@PriceCategoryID={33},@PlaceOfSupply={34}," +
                            "@ArabicName={35},@SalesManID={36},@PartyCategoryID={37},@BulidingNo={38},@District={39},@Province={40}," +
                            "@CountryCode={41},@DistrictArabic={42},@CityArabic={43},@ProvinceArabic={44},@ID={45}",
                          criteria,
                       generalDto.Name,
                       generalDto.ContactPersonName,
                       nature,
                       generalDto.AddressLineOne,
                       generalDto.AddressArabic,
                       generalDto.City,
                       generalDto.Country,
                       generalDto.POBox,
                       generalDto.Remarks,
                       generalDto.TelephoneNo,
                       generalDto.MobileNo,
                       generalDto.EmailAddress,
                       generalDto.FaxNo,
                       generalDto.Active,
                       generalDto.PANNo,
                       generalDto.VATNO,
                       generalDto.CentralSalesTaxNo,
                       generalDto.Salutation,
                       generalDto.ContactPerson2,
                       generalDto.TelephoneNo2,
                       createdBranchId,
                       createdBranchId,
                       createdBy,
                       createdOn,
                       generalDto.ActasSupplierAlso,
                       generalDto.Code,
                       generalDto.Account.Id,
                       path,
                       generalDto.CustomerDetails.DL1,
                       generalDto.CustomerDetails.DL2,
                       generalDto.Area.Id,
                       generalDto.CreditLimit,
                       generalDto.CustomerDetails.PriceCategory.Id,
                       generalDto.CustomerDetails.PlaceOfSupply.Value,
                       generalDto.ArabicName,
                       generalDto.SalesMan,
                       generalDto.Category.Id,
                       generalDto.BulidingNo,
                       generalDto.District,
                       generalDto.Province,
                       generalDto.CountryCode,
                       generalDto.DistrictArabic,
                        generalDto.CityArabic,
                       generalDto.ProvinceArabic,
                        generalDto.Id
                       );

                        //CUSTOMER CEHCKING 
                        if (party.Id > 0 && generalDto.Type.Id == 1)
                        {
                            if (!AddCustomerInfo(generalDto.CustomerDetails, generalDto.CreditPeriod, party.Id))
                            { transaction.Rollback(); return CommonResponse.Error("AddCustomerInfo Error"); }
                        }

                        //ADD DELIVERY INFO
                        if (generalDto.DeliveryDetails != null && generalDto.DeliveryDetails.Count > 0)
                        {
                            bool flag = false;
                            foreach (var deliverydetail in generalDto.DeliveryDetails)
                            {
                                if (!AddDeliveryDetails(deliverydetail, party.Id))
                                {
                                    flag = true;
                                }
                            }

                            if (flag)
                            {
                                transaction.Rollback(); return CommonResponse.Error("Delivery Details Error");
                            }
                        }
                        transaction.Commit();
                        return CommonResponse.Created("Update");

                    }
                    else
                    {
                        return CommonResponse.NotFound("Given Id is invaild");
                    }

                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return CommonResponse.Error(ex.Message);
            }
            return CommonResponse.Error();
        }

            private string UploadImage(string general, IFormFile imageFile)
            {
                var path = "";

                var maxSizeInBytes = 5 * 1024 * 1024;

                if (general != null && imageFile != null)
                {
                    if (imageFile.Length > maxSizeInBytes)
                    {
                        throw new Exception("File size exceeds the maximum allowed size.");
                    }

                    path = Path.Combine(hostEnvironment.ContentRootPath, "BindData", "Images", "CustSupp", general + ".jpg");

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }

                return path;
            }
        
        /// Private functions 
        /// 1. create new account 
        ///

        private bool AddCustomerInfo(CustomerDetailsDto Data , decimal? CreditPeriod, int PartyId)
        {
            return _customerService.SaveCustomDetails(Data, CreditPeriod, PartyId);
        }

        private bool AddDeliveryDetails(DeliveryDetailsDto Data, int PartyId)
        {
            return _deliveryService.Save(Data, PartyId);
          
        }


    }
}
