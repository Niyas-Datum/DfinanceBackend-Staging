using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Stakeholder.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(DFCoreContext context, IAuthService authService, ILogger<CustomerService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        } /// <summary>
          /// 
          /// </summary>
          /// <returns></returns>
        public CommonResponse FillCustomer()
        {
            int branchId = _authService.GetBranchId().Value;

            var Supp = _context.Parties
                .Where(md => md.CompanyId == branchId && md.Nature == "C")
                .Select(x => new
                {
                    Id = x.Id,
                    Vatno = x.SalesTaxNo,
                    Mobilenumber = x.MobileNo,
                    Accountcode = x.Code,
                    Accountname = x.Name,
                    AccountId = x.AccountId
                })
                .ToList();
            return CommonResponse.Ok(Supp);
        }


        /// <summary>
        /// Fill Price Category (DropDown)
        /// </summary>
        /// <returns>Id,Name</returns>
        public CommonResponse FillPriceCategory()
        {
            string criteria = "FillPriceCategory";
            var result = _context.DropDownViewName.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
            return CommonResponse.Ok(result);
        }
        /// <summary>
        /// Fill CategoryRecommednded &Category Fixed(DropDown)
        /// </summary>
        /// <returns>Id,name</returns>
        public CommonResponse FillCustomerCategories()
        {
            string criteria = "FillMaCustomerCategories";
            var CustomerCategories = _context.DropDownViewName.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
            return CommonResponse.Ok(CustomerCategories);
        }

        /// <summary>
        /// Fill CategoryRecommednded &Category Fixed(DropDown)
        /// </summary>
        /// <returns>Id,name</returns>
        public CommonResponse CrdtCollDropdown()
        {
            string criteria = "CreditCollectionType";
            var CustomerCategories = _context.CrdtCollView.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
            return CommonResponse.Ok(CustomerCategories);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerDetailsDto"></param>
        /// <param name="CreditPeriod"></param>
        /// <param name="PartyId"></param>
        /// <returns></returns>
        public bool SaveCustomDetails(CustomerDetailsDto customerDetailsDto, decimal? CreditPeriod, int PartyId)
        {
            try
            {
                var CustmId = _context.MaCustomerDetails
                                      .Where(cd => cd.PartyId == PartyId)
                                      .Select(cd => cd.Id)
                                      .SingleOrDefault();
                var custItems = new List<int>();
                if (customerDetailsDto.CommoditySought.Count > 0)
                {
                    foreach (var c in customerDetailsDto.CommoditySought)
                        custItems.Add(c.Id.Value);
                }

                if (CustmId == 0)
                {
                    string criteria1 = "InsertCustomerDetails";
                    SqlParameter newIdCust = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    _context.Database.ExecuteSqlRaw(
                       "EXEC PartyMasterSP @Criteria={0},@PartyID={1},@PlannedPcs={2},@PlannedCFT={3},@CashCreditType={4},@CreditPeriod={5}," +
                       "@CreditCollnThru={6},@BusPrimaryType={7},@BusRetailType={8},@BusYears={9},@BusYearTurnover={10},@IsLoanAvailed={11}," +
                       "@MainMerchants={12},@AddressOwned={13},@ValueofProperty={14},@MarketReputation={15},@BandByImportID={16}," +
                       "@SalesLimitByImport={17},@BandByHOID={18},@SalesLimitByHO={19},@CreditPeriodByHO={20},@OverdueLimitPerc={21}," +
                       "@OverduePeriodLimit={22},@ChequeBounceLimit={23},@SalesPriceLowVarPerc={24},@SalesPriceUpVarPerc={25},@NewID={26} OUTPUT",
                       criteria1,
                      PartyId,
                      customerDetailsDto.QuantityPlanned, //int
                      customerDetailsDto.BasicUnit,
                      customerDetailsDto.SalesType.Key,
                      CreditPeriod,
                      customerDetailsDto.CreditCollectionType.Key,
                      customerDetailsDto.BusinessType.Key,
                      customerDetailsDto.BusinessNature.Key,
                      customerDetailsDto.YearsOfBusiness,
                      customerDetailsDto.YearlyTurnover,
                      customerDetailsDto.AvailedAnyLoanLimits.Key,
                       customerDetailsDto.OtherMerchantsOfCustomer,
                       customerDetailsDto.BusinessAddress.Key,
                       customerDetailsDto.ValueOfProperty,
                       customerDetailsDto.MarketReputation.Key,
                       customerDetailsDto.CategoryRecommended.Id,
                       customerDetailsDto.LimitRecommended,
                       customerDetailsDto.CategoryFixed.Id,
                       customerDetailsDto.LimitFixedForCustomer,
                       customerDetailsDto.CreditPeriodPermitted,
                       customerDetailsDto.OverdueAmountLimit,
                       customerDetailsDto.OverduePeriodLimit,
                       customerDetailsDto.ChequeBounceCountLimit,
                       customerDetailsDto.SalesPriceLowVarLimit,
                       customerDetailsDto.SalesPriceUpVarLimit,
                        newIdCust
                    );

                    SaveMaCustomerItems(PartyId, custItems);
                    _logger.LogInformation("Customer Details Saved Successfully");
                    return true;

                }
                else
                {
                    string criteria1 = "UpdateCustomerDetails";
                    _context.Database.ExecuteSqlRaw(
                      "EXEC PartyMasterSP @Criteria={0},@PartyID={1},@PlannedPcs={2},@PlannedCFT={3},@CashCreditType={4},@CreditPeriod={5},@CreditCollnThru={6},@BusPrimaryType={7},@BusRetailType={8},@BusYears={9},@BusYearTurnover={10},@IsLoanAvailed={11},@MainMerchants={12},@AddressOwned={13},@ValueofProperty={14},@MarketReputation={15},@BandByImportID={16},@SalesLimitByImport={17},@BandByHOID={18},@SalesLimitByHO={19},@CreditPeriodByHO={20},@OverdueLimitPerc={21},@OverduePeriodLimit={22},@ChequeBounceLimit={23},@SalesPriceLowVarPerc={24},@SalesPriceUpVarPerc={25},@ID={26}",
                       criteria1,
                      PartyId,
                      customerDetailsDto.QuantityPlanned,
                      customerDetailsDto.BasicUnit,
                      customerDetailsDto.SalesType.Key,
                      CreditPeriod,
                      customerDetailsDto.CreditCollectionType.Key,
                      customerDetailsDto.BusinessType.Key,
                      customerDetailsDto.BusinessNature.Key,
                      customerDetailsDto.YearsOfBusiness,
                      customerDetailsDto.YearlyTurnover,
                      customerDetailsDto.AvailedAnyLoanLimits.Key,
                       customerDetailsDto.OtherMerchantsOfCustomer,
                       customerDetailsDto.BusinessAddress.Key,
                       customerDetailsDto.ValueOfProperty,
                       customerDetailsDto.MarketReputation.Key,
                       customerDetailsDto.CategoryRecommended.Id,
                       customerDetailsDto.LimitRecommended,
                       customerDetailsDto.CategoryFixed.Id,
                       customerDetailsDto.LimitFixedForCustomer,
                       customerDetailsDto.CreditPeriodPermitted,
                       customerDetailsDto.OverdueAmountLimit,
                       customerDetailsDto.OverduePeriodLimit,
                       customerDetailsDto.ChequeBounceCountLimit,
                       customerDetailsDto.SalesPriceLowVarLimit,
                       customerDetailsDto.SalesPriceUpVarLimit,
                       CustmId
                   );

                    if (customerDetailsDto.CommoditySought.Count > 0)
                    {
                        var remove = _context.MaCustomerItems.Where(u => u.PartyId == PartyId).ToList();
                        _context.MaCustomerItems.RemoveRange(remove);
                        _context.SaveChanges();
                        SaveMaCustomerItems(PartyId, custItems);
                    }
                    _logger.LogInformation("Customer Details Updated Successfully");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Saving Customer Details");
                return false;
            }
        }
        private CommonResponse SaveMaCustomerItems(int PartyId, List<int> CommodityId)
        {
            SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            string criteria = "InsertCustomerItems";
            foreach (var c in CommodityId)
            {
                var data = _context.Database.ExecuteSqlRaw("Exec PartyMasterSP @Criteria={0},@PartyID={1},@CommodityID={2},@NewID={3} OUTPUT",
                    criteria, PartyId, c, newId);
            }
            _logger.LogInformation("MaCustomerItems Saved Successfully");
            return CommonResponse.Ok();
        }
    }
}
