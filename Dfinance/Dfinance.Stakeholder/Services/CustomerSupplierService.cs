using Azure;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Stakeholder.Services
{
    public class CustomerSupplierService : ICustomerSupplierService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;       
        private readonly IChartOfAccountsService _accountService;
        private readonly IConfiguration _configuration;
        private readonly ICustomerService _customerService;
        private readonly ICsDeliveryService _deliveryService;
        private readonly ILogger<CustomerSupplierService> _logger;
       
        public CustomerSupplierService(DFCoreContext context, IAuthService authService, IChartOfAccountsService accountService, ICustomerService customerService,
                                        ICsDeliveryService deliveryService, IConfiguration configuration, ILogger<CustomerSupplierService> logger)
        {
            _context = context;
            _authService = authService;
            _accountService = accountService;
            _customerService = customerService;
            _deliveryService = deliveryService;
            _configuration = configuration;
            _logger = logger;
            PartyImage = _configuration["AppSettings:PartyImage"];
        }
        private string base64RemoveData = "data:image/png;base64,";
       private string? PartyImage = null;
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
       
        /// </summary>
        /// <param name="branchid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommonResponse FillParty()
        {
            int branchid = _authService.GetBranchId().Value;
            string criteria = "FillParty";
            var result = _context.FillSideBars.FromSqlRaw($"EXEC PartyMasterSP @Criteria='{criteria}',@CompanyID='{branchid}'").ToList();
            return CommonResponse.Ok(result);
        }
       
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommonResponse FillPartyWithID(int Id,int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill Customer/Supplier");
            }
            string criteria = "FillPartyWithID";
            string Criteria = "FillCustomerDetails";
            string cri = "FillDeliveryDetails";
            var result = _context.FillPartyById.FromSqlRaw($"EXEC PartyMasterSP @Criteria='{criteria}',@ID='{Id}'").AsEnumerable().FirstOrDefault();
            var custdetails = _context.FillCustdetails.FromSqlRaw($"EXEC PartyMasterSP @Criteria='{Criteria}',@PartyID='{Id}'").ToList();
            var deldetails = _context.FillDeldetails.FromSqlRaw($"EXEC PartyMasterSP @Criteria='{cri}',@DDPartyID='{Id}'").ToList();
            var accountId = result.AccountID;
            var parent = from f1 in _context.FiMaAccounts
                         where (from f2 in _context.FiMaAccounts
                                where f2.Id == accountId
                                select f2.Parent)
                              .Contains(f1.Id)
                         select new { f1.Id, f1.Name };
            var commsought = from c in _context.Category
                             join m in _context.MaCustomerItems on c.Id equals m.CommodityId
                             where m.PartyId == Id
                             select new { c.Id, c.Description };
            string? imagePath = null;
           
            if (result?.ImagePath !=null && result.ImagePath != "")
                 imagePath = PartyImage +"\\"+ result?.ImagePath;
            string? imageBase64 = null;
            if (!string.IsNullOrEmpty(imagePath) || File.Exists(imagePath))
            {
                // Read image data
                byte[] imageData = File.ReadAllBytes(imagePath);
                // Convert  to Base64 string
                imageBase64 = Convert.ToBase64String(imageData);
                imageBase64 = base64RemoveData + imageBase64;
            }
            _logger.LogInformation("CustomerSupplier details Loaded Successfully");
            return CommonResponse.Ok(new { Result = result, CustDetails = custdetails, img = imageBase64, DelDetails = deldetails, AccountGroup = parent, CommoditySought = commsought });
        }

        /// <summary>
        /// Fill SUPPLIER(Purchase Form)
        /// </summary>
        /// <returns>acccode, accname, address, id, mobileno, vatno</returns>
        public CommonResponse FillSupplier(int locId, int pageId, int voucherId)
        {
            int? userId = _authService.GetId();
            int? branchId = _authService.GetBranchId();
            object PrimaryVoucherID = null, ItemID = null, ModeID = null, TransactionID = null, partyId = null;
            bool IsSizeItem = false, IsMargin = false, ISTransitLoc = false, IsFinishedGood = false, IsRawMaterial = false;

            DateTime? VoucherDate = null;
            string criteria = "";
            if (voucherId == 17)
                criteria = "SUPPLIER";
            else if (voucherId == 23)
                criteria = "CUSTOMER";
            else
                criteria = "PARTY";

            var result = _context.CommandTextView.FromSqlRaw($"select dbo.GetCommandText('{criteria}','{PrimaryVoucherID}','{branchId}','{partyId}','{locId}','{IsSizeItem}','{IsMargin}','{voucherId}','{ItemID}','{ISTransitLoc}','{IsFinishedGood}','{IsRawMaterial}','{ModeID}','{pageId}','{VoucherDate}','{TransactionID}','{userId}')")
                         .ToList();
            // var result = _context.CommandTextView.FromSqlRaw($"select dbo.GetCommandText('{criteria}',null,'{branchId}',null,'{locId}',null,'False','{voucherId}',null,'False',null,null,null,null,null,null,'{userId}')").ToList();
            var res = result.FirstOrDefault();
            var data = _context.FillPartyView.FromSqlRaw(res.commandText).ToList();
            return CommonResponse.Ok(data);
        }
        /// <summary>
        /// load customer supplier stype 
		/// O/P =>  [ customer, supplier] 
        /// </summary>
        /// <returns></returns>
        public CommonResponse FillPartyType()
        {
            string criteria = "FillPartyType";
            var result = _context.DropDownViewDesc.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
            return CommonResponse.Ok(result);
        }

        /// <summary>
        /// generate code - (new form)
        /// </summary>
        /// <returns></returns>

        public CommonResponse GetCode()
        {
            string criteria = "FillNextCode";
            var result = _context.NextCodeView
                .FromSqlRaw($"EXEC PartyMasterSP @Criteria='{criteria}'")
                 .AsEnumerable()
                 .FirstOrDefault();
            return CommonResponse.Ok(result);
        }
        /// <summary>
        /// Fill Category - (General form) 
		/// customer supplier category 
        /// </summary>
        /// <returns>Id,Value</returns>
        public CommonResponse FillCategory()
        {
            string criteria = "PartyCategory";
            var result = _context.DropDownViewValue.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
            return CommonResponse.Ok(result);
        }
        /// <summary>
        /// Add/Update Customer supplier General Data
        /// </summary>
        /// <param name="generalDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CommonResponse SaveGen(GeneralDto generalDto, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Customer/Supplier");
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int createdBy = _authService.GetId().Value;
                int createdBranchId = _authService.GetBranchId().Value;
                DateTime createdOn = DateTime.Now;
                var nature = "S";
                string? path = null;
                if (generalDto.Type.Id == 1)
                {
                    nature = "C";
                }
                //PHOTO UPLOAD
                if (generalDto.Image != null)
                {
                    path = UploadImage(generalDto.Image, generalDto.Code);
                }

                if (generalDto.CustomerDetails.PriceCategory.Id == 0)
                {
                    generalDto.CustomerDetails.PriceCategory.Id = null;
                }

                //New Account Create 
                if (generalDto.LetSystemGenNewAccForParty == true)
                {
                    var accountCat = _context.FiMaAccountCategory
                        .Where(x => x.Id == generalDto.Type.Id)
                        .Select(x => new DropDownDtoName { Id = x.Id })
                        .FirstOrDefault();

                    var accGroup = _context.FiMaAccounts
                        .Where(x => x.Id == generalDto.AccountGroup.Id)
                        .Select(x => new DropDownDtoName { Id = x.AccountGroup })
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

                if (generalDto.Id == null || generalDto.Id == 0)
                {

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
                       generalDto.Country.Id,
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
                        {
                            transaction.Rollback();
                            _logger.LogInformation("Error in Saving Customer Details");
                            return CommonResponse.Error("AddCustomerInfo Error");
                        }
                    }
                    //ADD DELIVERY INFO
                    if (generalDto.DeliveryDetails != null && generalDto.DeliveryDetails.Count > 0)
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
                            _logger.LogInformation("Error in Saving Delivery Details");
                            transaction.Rollback(); return CommonResponse.Error("Delivery Details Error");
                        }
                    }

                    transaction.Commit();
                    _logger.LogInformation("Party Saved Successfully");
                    return CommonResponse.Created($"Saved with PartyId: {PartyId}");
                }

                else
                {
                    if (!_authService.IsPageValid(pageId))
                    {
                        return PageNotValid(pageId);
                    }
                    if (!_authService.UserPermCheck(pageId, 3))
                    {
                        return PermissionDenied("Update Customer/Supplier");
                    }
                    var party = _context.Parties.Any(p => p.Id == generalDto.Id && p.Code == generalDto.Code);
                    if (party == true)
                    {
                        string criteria = "UpdateWeb";
                        _context.Database.ExecuteSqlRaw("EXEC PartyMasterSP @Criteria={0},@Name={1},@ContactPerson={2},@Nature={3},@AddressLineOne={4}," +
                               "@AddressLineTwo={5},@City ={6},@Country={7},@POBox={8},@Remarks={9},@TelephoneNo={10},@MobileNo={11},@EmailAddress={12}," +
                               "@FaxNo={13},@Active={14},@PANNo={15},@SalesTaxNo={16},@CentralSalesTaxNo={17},@Salutation={18},@ContactPerson2={19}," +
                               "@TelephoneNo2={20},@CompanyID={21},@BranchID={22},@CreatedBy={23},@CreatedOn={24},@IsMultiNature={25},@Code={26},@AccountID={27}," +
                               "@ImagePath={28},@DL1={29},@DL2={30},@AreaID={31},@CreditLimit={32},@PriceCategoryID={33},@PlaceOfSupply={34}," +
                               "@ArabicName={35},@SalesManID={36},@PartyCategoryID={37},@BulidingNo={38},@District={39},@Province={40}," +
                               "@CountryCode={41},@DistrictArabic={42},@CityArabic={43},@ProvinceArabic={44},@ID={45}",
                             criteria,//0
                          generalDto.Name,//1
                          generalDto.ContactPersonName,//2
                          nature,//3
                          generalDto.AddressLineOne,//4
                          generalDto.AddressArabic,//5
                          generalDto.City,//6
                          generalDto.Country.Id,//7
                          generalDto.POBox,//8
                          generalDto.Remarks,//9
                          generalDto.TelephoneNo,//10
                          generalDto.MobileNo,//11
                          generalDto.EmailAddress,//12
                          generalDto.FaxNo,//13
                          generalDto.Active,//14
                          generalDto.PANNo,//15
                          generalDto.VATNO,//16
                          generalDto.CentralSalesTaxNo,//17
                          generalDto.Salutation,//18
                          generalDto.ContactPerson2,//19
                          generalDto.TelephoneNo2,//20
                          createdBranchId,//21
                          createdBranchId,//22
                          createdBy,//23
                          createdOn,//24
                          generalDto.ActasSupplierAlso,//25
                          generalDto.Code,//26
                          generalDto.Account.Id,//27
                          path,//28
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
                        if (generalDto.Id > 0 && generalDto.Type.Id == 1)
                        {
                            if (!AddCustomerInfo(generalDto.CustomerDetails, generalDto.CreditPeriod, (int)generalDto.Id))
                            {
                                transaction.Rollback();
                                _logger.LogInformation("Error in Saving Customer Details");
                                return CommonResponse.Error("AddCustomerInfo Error");
                            }
                        }

                        //ADD DELIVERY INFO
                        if (generalDto.DeliveryDetails != null && generalDto.DeliveryDetails.Count > 0)
                        {
                            bool flag = false;
                            foreach (var deliverydetail in generalDto.DeliveryDetails)
                            {
                                if (!AddDeliveryDetails(deliverydetail, (int)generalDto.Id))
                                {
                                    flag = true;
                                }
                            }

                            if (flag)
                            {
                                _logger.LogInformation("Error in Saving Delivery Details");
                                transaction.Rollback(); return CommonResponse.Error("Delivery Details Error");
                            }
                        }
                        transaction.Commit();
                        _logger.LogInformation("Party Updated Successfully");
                        return CommonResponse.Created("Update");

                    }
                    else
                    {
                        _logger.LogInformation(" PartyId Provided for Updation is Invalid");
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

        private string UploadImage(string base64EncodedData, string Code)
        {           
            base64EncodedData = base64EncodedData.Replace(base64RemoveData, "");
            byte[] imageData = Convert.FromBase64String(base64EncodedData);

            // Get upload path from app settings
           

            if (!Directory.Exists(PartyImage))
                Directory.CreateDirectory(PartyImage);

            // Construct image paths
            string imagePath = Path.Combine(PartyImage, $"{Code}.jpg");
            string imagePathDb = Code+".jpg";

            // Write image data to file
            File.WriteAllBytes(imagePath, imageData);
            _logger.LogInformation(" Image uploaded Successfully");
            return imagePathDb;
        }

        private bool AddCustomerInfo(CustomerDetailsDto Data, decimal? CreditPeriod, int PartyId)
        {
            return _customerService.SaveCustomDetails(Data, CreditPeriod, PartyId);
        }

        private bool AddDeliveryDetails(DeliveryDetailsDto Data, int PartyId)
        {
            return _deliveryService.Save(Data, PartyId);

        }

        public CommonResponse Delete(int Id,int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Delete Customer/Supplier");
            }
            string msg = null;
            var party = _context.Parties.Where(i => i.Id == Id).Select(i => new {id=i.Id,acc=i.AccountId}).SingleOrDefault();
            if (party.id == null)
            {
                msg = "Id Not Found";
                return CommonResponse.NotFound(msg);
            }
            if (party.acc!=null)
            {
                try 
                {
                   var del= _context.FiMaAccounts.Where(i => i.Id == party.acc).ExecuteDelete();
                }
                catch(Exception ex)
                {
                    return CommonResponse.Error("Account Cannot be deleted");
                }
            }
            string Criteria = "DeleteParties";
            var result = _context.Database.ExecuteSqlRaw($"EXEC PartyMasterSP @Criteria='{Criteria}',@Id='{Id}'");
            _logger.LogInformation("Party Deleted successfullt");
            return CommonResponse.Ok("Parties deleted Successfully!");
        }
    }
}
