using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dfinance.api.Controllers.v1.DMain.Miscellaneous
{
    
    [ApiController]
    [Authorize]
    public class CountryDropDownController : BaseController
    {
        private readonly ICountryDropDownService countryService;

        public CountryDropDownController(ICountryDropDownService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet(ApiRoutes.Country.GetAll)]
        public IActionResult FillCountry()
        {
            try
            {
                // Calls the FillCountry method of a service (probably CountryDropDownService) and returns its result.
                var countries = countryService.FillCountry();
                return Ok(countries);
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }
        }
    }
}
