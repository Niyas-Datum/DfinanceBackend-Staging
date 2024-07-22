using Dfinance.Core.Views;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Restaurant.Interface
{
    public interface IRestaurantInvoice
    {       
        CommonResponse FillTables(int sectionId);
        CommonResponse GetLoadData(int voucherId);
        CommonResponse SaveRestaurentInvoice(InventoryTransactionDto RestaurentDto, int PageId, int voucherId, int sectionId, TableView table, string? tokenId, string? deliveryId);
        CommonResponse UpdateRestaurentInvoice(InventoryTransactionDto RestaurentDto, int PageId, int voucherId, int sectionId, TableView table, string? tokenId, string? deliveryId);
        CommonResponse GetKitchenCategory(int transactionId);
        CommonResponse PrintKOT(int transactionId, int kitchenCatId);
        CommonResponse GetProducts(int? categoryId);
        CommonResponse SaveItemOptions(ItemOptionsDto itemOptionsDto);
        CommonResponse UpdateItemOptions(ItemOptionsDto itemOptionsDto);
        CommonResponse FillItemOptionsMaster();
        CommonResponse GetWaiterList();
        CommonResponse GetSections();
        CommonResponse GetCategories();
    }
}
