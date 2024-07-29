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
        CommonResponse SaveRestaurentInvoice(InventoryTransactionDto RestaurentDto, int PageId, int voucherId, int sectionId, int tableId, string tableName, string? tokenId, string? deliveryId, int salesManId, string chairName);
        CommonResponse UpdateRestaurentInvoice(InventoryTransactionDto RestaurentDto, int PageId, int voucherId, int sectionId, int tableId, string tableName, string? tokenId, string? deliveryId, int salesManId,string chairName);
        CommonResponse GetKitchenCategory(int transactionId);
        CommonResponse PrintKOT(int transactionId, int kitchenCatId);
        CommonResponse GetProducts(int? categoryId);
        CommonResponse SaveItemOptions(ItemOptionsDto itemOptionsDto);
        CommonResponse UpdateItemOptions(ItemOptionsDto itemOptionsDto);
        CommonResponse FillItemOptionsMaster();
        CommonResponse GetWaiterList();
        CommonResponse GetSections();
        CommonResponse GetCategories();
        CommonResponse SaveAndPrintKOT(RestaurentDto restaurentDto, int sectionId, int tableId, string tableName,int salesManId,string chairName);
        CommonResponse GetItemsByTransId(int transId);
    }
}
