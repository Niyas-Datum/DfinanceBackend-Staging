using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface ICurrencyService
    {
        CommonResponse FillAllCurrencyCode();
        CommonResponse FillCurrencyCodeById(int Id);
        CommonResponse SaveCurrencyCode(CurrencyCodeDto currencyCodeDto);
        CommonResponse UpdateCurrencyCode(CurrencyCodeDto currencyCodeDto, int Id);
        CommonResponse DeleteCurrencyCode(int Id);
        //**********************************************************************
        CommonResponse FillAllCurrency();
        CommonResponse FillCurrencyById(int Id);
        CommonResponse SaveCurrency(CurrencyDto currencyDto);
        CommonResponse UpdateCurrency(CurrencyDto currencyDto, int Id);
        CommonResponse DeleteCurrency(int Id);
        CommonResponse CurrencyDropdown();
        CommonResponse UpdateExchangeRate(int currencyId, decimal exchRate);
    }
}
