using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dfinance.Application.Services.Finance
{

    public class CurrencyService:ICurrencyService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private ILogger<CurrencyService> _logger;
        public CurrencyService(DFCoreContext context, IAuthService authService, ILogger<CurrencyService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        //*****************************Fill CurrencyCode**************************************************
        //called by CurrencyController/FillAllCurrencyCode
        public CommonResponse FillAllCurrencyCode()
        {
            try
            {
                string criteria = "FillCurrencyCodeMaster";
                var result = _context.FillcurrencyCode.FromSqlRaw($"Exec spCurrency @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);

            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }

        }
        //*****************************FillAllCurrencyCode**************************************************
       
        //called by CurrencyController/FillCurrencyCodeById
        public CommonResponse FillCurrencyCodeById(int Id)
        {
            try
            {
                string criteria = "FillCurrencyCodeWithID";
                var result = _context.FillCurrencyCodeById.FromSqlRaw($"Exec spCurrency @Criteria='{criteria}',@ID={Id}").ToList();
                return CommonResponse.Ok(result);

            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
            
        }
        //*****************************SaveCurrencyCode**************************************************
        //called by CurrencyController/SaveCurrencyCode
        public CommonResponse SaveCurrencyCode(CurrencyCodeDto currencyCodeDto)
        {
            try
            {
                string criteria = "InsertCurrencyCode";
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("Exec spCurrency @Criteria={0},@Code={1},@Name={2},@NewID={3} OUTPUT", criteria, currencyCodeDto.Code, currencyCodeDto.Name, newId);
                var CCNewId = newId.Value;
                _logger.LogInformation("CurrencyCode Successfully ");
                return CommonResponse.Created("CurrencyCode " + currencyCodeDto.Name + " is Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to save currencyCode");
                return CommonResponse.Error("Fail to save currencyCode");
            }
        }
        //*****************************Update CurrencyCode**************************************************
        //called by CurrencyController/UpdateCurrencyCode
        public CommonResponse UpdateCurrencyCode(CurrencyCodeDto currencyCodeDto, int Id) 
            {
                try
                {
                string msg = null;
                var currencyCode = _context.CurrencyCode.Any(c=>c.Id==Id);
                if (!currencyCode)
                {
                    msg = "CurrencyCode Not Found";
                    return CommonResponse.NotFound(msg);
                }
                var criteria = "UpdateCurrencyCode";
                    var currencycode = _context.Database.ExecuteSqlRaw($"Exec spCurrency @Criteria='{criteria}',@ID={Id},@Code='{currencyCodeDto.Code}',@Name='{currencyCodeDto.Name}'");
                    return CommonResponse.Created(currencycode);

                }
                catch (Exception ex)
                {
                _logger.LogError("Fail to update currencyCode");
                return CommonResponse.Error("Fail to update currencyCode");
                }
            }
        //*****************************DeleteCurrencyCode**************************************************
        //called by CurrencyController/DeleteCurrencyCode
        public CommonResponse DeleteCurrencyCode(int Id)
            {
            try
            {
                string msg = null;
                var currency = _context.CurrencyCode
    .Where(i => i.Id == Id)
    .SingleOrDefault();
                if (currency == null)
                {
                    msg = "CurrencyCode Not Found";
                    return CommonResponse.NotFound(msg);
                }

                var criteria = "DeleteCurrencyCode";
                msg = "CurrencyCode is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRaw($"EXEC spCurrency @Criteria='{criteria}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
        
            catch (Exception ex)
            {
                _logger.LogError("Fail to delete currencyCode");
                return CommonResponse.Error(ex);
            }
        }
        //***************************  Currency ***************************************************************
        //*****************************Fill Currency**************************************************
        //called by CurrencyController/FillAllCurrency
        public CommonResponse FillAllCurrency()
        {
            try
            {
                int CreatedBranchId = _authService.GetBranchId().Value;
                string criteria = "FillCurrencyMaster";
                var result = _context.FillCurrency.FromSqlRaw($"Exec spCurrency @Criteria='{criteria}',@CompanyID={CreatedBranchId}").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }

        }
        //*****************************Fill Currency ById**************************************************
        //called by CurrencyController/FillAllCurrencyById
        public CommonResponse FillCurrencyById(int Id)
        {
            try
            {
                string criteria = "FillCurrencyWithID";
                var result = _context.FillCurrencyById.FromSqlRaw($"Exec spCurrency @Criteria='{criteria}',@CurrencyID={Id}").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }

        }
        //*****************************Save Currency**************************************************
        //called by CurrencyController/SaveCurrency
        public CommonResponse SaveCurrency(CurrencyDto currencyDto)
            {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;

              
                string criteria = "InsertCurrency";


                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
             _context.Database.ExecuteSqlRaw("Exec spCurrency @Criteria={0},@Currency={1},@Abbreviation={2},@DefaultCurrency={3},@CurrencyRate={4},@CreatedBy={5},@ActiveFlag={6},@CreatedBranchID={7},@Coin={8},@Precision={9},@Culture={10},@FormatString={11},@Symbol={12},@NewID={13} OUTPUT", criteria, currencyDto.CurrencyName, currencyDto.CurrencyCode.Key, currencyDto.IsDefault, currencyDto.CurrencyRate, CreatedBy,1, CreatedBranchId, currencyDto.Coin, 2, "en - IN", "{0:N2}",currencyDto.Symbol, newId);
                var NewId = newId.Value;
                return CommonResponse.Created(NewId);

            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to save currency");
                return CommonResponse.Error(ex.Message);
            }
        }
        //*****************************Update Currency**************************************************
        //called by CurrencyController/UpdateCurrency
        public CommonResponse UpdateCurrency(CurrencyDto currencyDto, int Id)

            {
            try
            {
                string msg = null;
                var currencyid = _context.Currency.Any(c=>c.CurrencyId==Id);
                if (!currencyid )
                {
                    msg = "Currency Not Found";
                    return CommonResponse.NotFound(msg);
                }
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                var criteria = "UpdateCurrency";
              var currency=  _context.Database.ExecuteSqlRaw("Exec spCurrency @Criteria={0},@Currency={1},@Abbreviation={2},@DefaultCurrency={3},@CurrencyRate={4},@CreatedBy={5},@ActiveFlag={6},@CreatedBranchID={7},@Coin={8},@Precision={9},@Culture={10},@FormatString={11},@CurrencyID={12},@CreatedOn={13},@Symbol={14}", criteria, currencyDto.CurrencyName, currencyDto.CurrencyCode.Key, currencyDto.IsDefault, currencyDto.CurrencyRate, CreatedBy, 1, CreatedBranchId, currencyDto.Coin, 2, "en - IN", "{0:N2}",Id, CreatedOn,currencyDto.Symbol);
                return CommonResponse.Created(currency);

            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to update currency");
                return CommonResponse.Error(ex.Message);
            }
        }
        //*****************************Delete Currency**************************************************
        //called by CurrencyController/DeleteCurrency
        public CommonResponse DeleteCurrency(int Id)
                {
            try
            {
                string msg = null;
                var curr = _context.Currency.Where(i => i.CurrencyId == Id).
                    Select(i => i.Currency1).
                    SingleOrDefault();
                if (curr == null)
                {
                    msg = "Currency Not Found";
                    return CommonResponse.NotFound(msg);
                }

                int criteria = 3;
                msg = "Currency " + curr + " is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRaw($"EXEC spCurrency @Mode='{criteria}', @CurrencyID={Id}");
                return CommonResponse.Ok(msg);
            }

            catch (Exception ex)
            {
                _logger.LogError("Fail to delete currency");
                return CommonResponse.Error(ex);
            }
        }

        //currency dropdown
        //used in purchase,save and other transaction pages
        public CommonResponse CurrencyDropdown()
        {
            var currencies = _context.CurrencyDDView.FromSqlRaw("exec spCurrency @Criteria= 'FillCurrency'").ToList();
            return CommonResponse.Ok(currencies);
        }
        //exchange rate updation in purchase, sales and other transaction pages
        public CommonResponse UpdateExchangeRate(int currencyId, decimal exchRate)
        {
            string criteria = "UpdateExchangeRate";
            _context.Database.ExecuteSqlRaw("Exec spCurrency @Criteria={0},@CurrencyID={1},@CurrencyRate={2}", criteria, currencyId, exchRate);
            return CommonResponse.Ok();
        }
    }
}
