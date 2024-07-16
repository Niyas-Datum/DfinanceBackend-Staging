﻿using Dfinance.Core.Views;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Restaurant.Interface;
using Dfinance.Sales;
using Dfinance.Shared.Routes;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Dfinance.Shared.Routes.InvRoute;
using static System.Collections.Specialized.BitVector32;

namespace Dfinance.api.Controllers.v1.DMain.Rastaurent
{
    public class RastaurentController : Controller
    {
        private readonly IRestaurantInvoice _restaurantInvoice;
        public RastaurentController(IRestaurantInvoice restaurant)
        {
            _restaurantInvoice = restaurant;
        }
        [HttpPost(InvRestRoute.Restaurant.SaveRest)]

        public IActionResult Save([FromBody] InventoryTransactionDto rastaurantDto, int PageId, int voucherId,int sectionId,TableView table,string? tokenId = null,string? delivaryId = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.SaveRestaurentInvoice(rastaurantDto, PageId, voucherId,sectionId,table,tokenId,delivaryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRestRoute.Restaurant.UpdateRest)]

        public IActionResult Update([FromBody] InventoryTransactionDto rastaurantDto, int PageId, int voucherId, int sectionId, TableView table, string? tokenId = null, string? delivaryId = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.UpdateRestaurentInvoice(rastaurantDto, PageId, voucherId, sectionId, table, tokenId, delivaryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(InvRestRoute.Restaurant.GetLoadData)]
        public IActionResult GetLaodData(int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.GetLoadData(voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRestRoute.Restaurant.FillTable)]
        public IActionResult GetTableData(int sectionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.FillTables(sectionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRestRoute.Restaurant.GetKitchenCategory)]
        public IActionResult GetKitchenCategory(int transactionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.GetKitchenCategory(transactionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRestRoute.Restaurant.GetProducts)]
        public IActionResult GetProducts(int? categoryId=null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.GetProducts(categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRestRoute.Restaurant.PrintKOT)]
        public IActionResult PrintKOT(int transactionId,int kitCatId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.PrintKOT(transactionId,kitCatId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRestRoute.Restaurant.SaveOption)]
        public IActionResult SaveItemOptions([FromBody] ItemOptionsDto itemOptionsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.SaveItemOptions(itemOptionsDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRestRoute.Restaurant.UpdateOption)]
        public IActionResult UpdateItemOptions([FromBody] ItemOptionsDto itemOptionsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _restaurantInvoice.UpdateItemOptions(itemOptionsDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
