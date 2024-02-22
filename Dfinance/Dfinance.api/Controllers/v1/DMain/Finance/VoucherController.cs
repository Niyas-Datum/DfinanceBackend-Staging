using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Core.Domain;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{

    [ApiController]
    [Authorize]

    public class VoucherController : BaseController
    {
        private readonly IVoucherService _VoucherService;

        public VoucherController(IVoucherService VoucherService)
        {
            _VoucherService = VoucherService;
        }


        /// <summary>
        /// @windows: -Finance/masters  
        /// @Form : VoucherType
        /// </summary>
        ///  <returns>Vouchers Details</returns>
        [HttpGet(FinRoute.Voucher.FillVouchers)]
        public IActionResult FillVoucher()
        {
            try
            {
                var view = _VoucherService.FillVoucher();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters   
        /// @Form : VoucherType
        /// </summary>
        /// <param name="VoucherDto"> </param> 

        //[HttpPost(FinRoute.Voucher.SaveVouchers)]
        //public IActionResult SaveVoucher(VoucherDto voucherDto)
        //{
        //    try
        //    {
        //        var save = _VoucherService.SaveVoucher(voucherDto);
        //        return Ok(save);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// @windows: -Finance/masters   
        /// @Form : VoucherType
        /// </summary>
        /// <param name="VoucherDto"> </param> 
        [HttpPatch(FinRoute.Voucher.UpdateVouchers)]
        public IActionResult UpdateVouchers([FromBody] List<VoucherDto> voucherDtos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var update = _VoucherService.UpdateVouchers(voucherDtos);

                return Ok(update);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters     
        /// @Form : VoucherType
        /// </summary>
        /// <param> [ID] </param> 
        //[HttpDelete(FinRoute.Voucher.DeleteVouchers)]
        //public IActionResult Deletevoucher(int ID)
        //{
        //    try
        //    {
        //        var delete = _VoucherService.Deletevoucher(ID);
        //        return Ok(delete);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// @windows: -Finance/masters   
        /// @Form : VoucherType
        /// </summary>
        /// <param> [PrimaryVoucherId] </param> 
        [HttpGet(FinRoute.Voucher.FillPrimaryVoucherName)]
        public IActionResult FillPrimaryVoucherName([FromQuery] int PrimaryVoucherId)
        {
            try
            {
                var fill = _VoucherService.FillPrimaryVoucherName(PrimaryVoucherId);
                return Ok(fill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters   
        /// @Form : VoucherType
        /// </summary>
        /// <param name="NumberingDto"> </param> 

        [HttpPost(FinRoute.Voucher.SaveVoucherNumbering)]
        public IActionResult SaveVoucherNumbering(NumberingDto numberingDto)
        {
            try
            {
                var save = _VoucherService.SaveVoucherNumbering(numberingDto);
                return Ok(save);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///<summary>
        /// @windows: -Finance/masters   
        /// @Form : VoucherType
        /// </summary>
        /// <param name="NumberingDto"> </param> 
        [HttpPatch(FinRoute.Voucher.UpdateVoucherNumbering)]
        public IActionResult UpdateVoucherNumbering(NumberingDto numberingDto, int Id)
        {
            try
            {
                var edit = _VoucherService.UpdateVoucherNumbering(numberingDto, Id);
                return Ok(edit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// @windows: -Finance/masters     
        /// @Form : VoucherType
        /// </summary>
        /// <param> [Id] </param> 
        [HttpDelete(FinRoute.Voucher.DeleteVoucherNumbering)]
        public IActionResult DeleteVoucherNumbering(int Id)
        {
            try
            {
                var delete = _VoucherService.DeleteVoucherNumbering(Id);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}



