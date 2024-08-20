using Dfinance.api.Authorization;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Application.Services.Finance;
using Dfinance.Finance.Masters.Interface;
using Dfinance.Finance.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Dfinance.Finance.Services;
using Dfinance.Shared.Routes.v1;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.api.Framework;
using Azure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Channels;
using System;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class AccountSortOrderController : BaseController
    {
        private readonly IAccountSortOrder _AccountSortOrderService;
        public AccountSortOrderController(IAccountSortOrder accountSortOrderService)
        {
            _AccountSortOrderService = accountSortOrderService;
        }
        [HttpGet(FinRoute.AccountSortOrder.FillAccsort)]
        public IActionResult FillAccountSortOrder(int pageId)
        {
            try
            {
                var view = _AccountSortOrderService.FillAccountSortOrder(pageId);

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(FinRoute.AccountSortOrder.UpdateAccsort)]
        public IActionResult UpdateAccountSortOrder([FromBody] AccountSortOrderDto accountSortOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var data = _AccountSortOrderService.UpdateAccountSortOrder(accountSortOrderDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }



    }

