using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Item.Services.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Item.Services
{
    public class ItemMappingService: IItemMappingService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<IItemMappingService> _logger;
        public ItemMappingService(DFCoreContext context, IAuthService authService, ILogger<IItemMappingService> logger)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
        }
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
        public CommonResponse FillMasterItems()
        {
            var itemsList = _context.ItemMappingView.FromSqlRaw("exec InvRelatedItemSP @Criteria=6").ToList();
            return CommonResponse.Ok(itemsList);
        }
        public CommonResponse FillItemDetails(int itemId)
        {
            var itemDetails = _context.ItemDetailsView.FromSqlRaw("exec InvRelatedItemSP @Criteria={0},@ItemID={1}", 2, itemId).ToList();
            return CommonResponse.Ok(itemDetails);
        }
        public CommonResponse SaveItemMapping(ItemMappingDto itemMappingDto, int pageId)
        {
           
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Update ItemMapping");
                }
                if (itemMappingDto.Items.Count == 0 || itemMappingDto.Items == null)
                    return CommonResponse.NoContent();
                var remove = _context.InvRelatedItems.Where(i=>i.ItemId1==itemMappingDto.ItemId).ToList();
                if (remove.Count>0)
                {
                    _context.InvRelatedItems.RemoveRange(remove);
                    _context.SaveChanges();
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                foreach (var item in itemMappingDto.Items)
                {
                    _context.Database.ExecuteSqlRaw("Exec InvRelatedItemSP @Criteria={0},@ItemID={1},@RelatedItemID={2},@NewID={3} OUTPUT",
                   3, itemMappingDto.ItemId, item.Id, newId);
                }               
            }
            _logger.LogInformation("Item Mapping Updated Successfully");
            return CommonResponse.Ok("Item Mapping Updated Successfully");
        }       
    }
}
