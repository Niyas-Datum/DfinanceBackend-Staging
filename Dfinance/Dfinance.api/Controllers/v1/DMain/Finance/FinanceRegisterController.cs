using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Masters;
using Dfinance.Finance.Masters.Interface;
using Dfinance.Finance.Services;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using static Azure.Core.HttpHeader;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]

    public class FinanceRegisterController : BaseController
    {
        private readonly IFinanceRegister _financeregisterService;
        public FinanceRegisterController(IFinanceRegister financeregisterService)
        {
            _financeregisterService = financeregisterService;
        }
        [HttpGet(FinRoute.FinanceRegister.Fillfinanceregisgter)]
        public IActionResult FillFinanceRegister(DateTime DateFrom, DateTime DateUpto, int BranchID, int? BasicVTypeID = null, int? VTypeID = null, bool Detailed = false, bool Inventory = false, bool Columnar = false, bool GroupItem = false, string Criteria = "", int? AccountID = null, int? PaymentTypeID = null, int? ItemID = null, int? CounterID = null, string PartyInvNo = "", string BatchNo = "", int? UserID = null, int? StaffID = null, int? AreaID = null, int? pageId = null)

        {
            try
            {
                // Call the service method and get the result
                var view = _financeregisterService.FillFinanceRegister(
                    DateFrom, DateUpto, BranchID, BasicVTypeID, VTypeID,
                    Detailed, Inventory, Columnar, GroupItem,
                    Criteria, AccountID, PaymentTypeID, ItemID,
                    CounterID, PartyInvNo, BatchNo, UserID, StaffID, AreaID, pageId);

                // Return the result as an OK response
                return Ok(view);
            }
            catch (Exception ex)
            {
                // Return a BadRequest with the exception message
                return BadRequest(ex.Message);
            }
        }



        [HttpGet(FinRoute.FinanceRegister.FillAccountPopup)]
        public IActionResult FillAccountPopup()

        {
            try
            {

                var view = _financeregisterService.FillAccountPopup();


                return Ok(view);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet(FinRoute.FinanceRegister.FillVoucherType)]
        public IActionResult FillVoucherType(int primaryVoucherId)

        {
            try
            {

                var view = _financeregisterService.FillVoucherType(primaryVoucherId);


                return Ok(view);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.FinanceRegister.FillBasicType)]
        public IActionResult FillBasicType()

        {
            try
            {

                var view = _financeregisterService.FillBasicType();


                return Ok(view);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}







