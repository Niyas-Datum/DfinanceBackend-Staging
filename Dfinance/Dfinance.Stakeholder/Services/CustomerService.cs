using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Common;
using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Stakeholder.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;

        public CustomerService(DFCoreContext context, IAuthService authService )
        {
            _context = context;
            _authService = authService;
        }
     /// <summary>
     /// Fill Commodity Sought(Text Area(Customer Form))
     /// </summary>
     /// <returns>Id,PartyId,CommodityId</returns>
        public CommonResponse FillCommodity()
        {
            try
            {
                string criteria = "FillCustomerItems";
                var result = _context.FillCustomeritem.FromSqlRaw($"EXEC PartyMasterSP @Criteria='{criteria}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Fill Price Category (DropDown)
        /// </summary>
        /// <returns>Id,Name</returns>
        public CommonResponse FillPriceCategory()
        {
            try
            {
                string criteria = "FillPriceCategory";
                var result = _context.DropDownViewName.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
                //var res = result.Select(x => new DropDownViewName
                //{
                //    ID = x.ID,
                //    Name = x.Name,
                //}).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Fill CategoryRecommednded &Category Fixed(DropDown)
        /// </summary>
        /// <returns>Id,name</returns>
        public CommonResponse FillCustomerCategories()
        {
            try
            {
                string criteria = "FillMaCustomerCategories";
                var CustomerCategories = _context.DropDownViewName.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(CustomerCategories);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
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
                //party  == CustomerSupplierId
                var CustmId = _context.MaCustomerDetails
                                      .Where(cd => cd.PartyId == PartyId)
                                      .Select(cd => cd.Id)
                                      .SingleOrDefault();


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
                      customerDetailsDto.SalesType.Id,
                      CreditPeriod,
                      customerDetailsDto.CreditCollectionType.Id,
                      customerDetailsDto.BusinessType.Id,
                      customerDetailsDto.BusinessNature.Id,
                      customerDetailsDto.YearsOfBusiness,
                      customerDetailsDto.YearlyTurnover,
                      customerDetailsDto.AvailedAnyLoanLimits.Value, //int
                       customerDetailsDto.OtherMerchantsOfCustomer,
                       customerDetailsDto.BusinessAddress.Id,
                       customerDetailsDto.ValueOfProperty,
                       customerDetailsDto.MarketReputation,
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
                   // int NewCustId = (int)newIdCust.Value;
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
                      customerDetailsDto.SalesType.Id,
                      CreditPeriod,
                      customerDetailsDto.CreditCollectionType.Id,
                      customerDetailsDto.BusinessType.Id,
                      customerDetailsDto.BusinessNature.Id,
                      customerDetailsDto.YearsOfBusiness,
                      customerDetailsDto.YearlyTurnover,
                      customerDetailsDto.AvailedAnyLoanLimits.Value,
                       customerDetailsDto.OtherMerchantsOfCustomer,
                       customerDetailsDto.BusinessAddress.Id,
                       customerDetailsDto.ValueOfProperty,
                       customerDetailsDto.MarketReputation,
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
                    return true;

                }
            }
            catch (Exception ex)
            {

                return false;

            }
        }
    }
}
