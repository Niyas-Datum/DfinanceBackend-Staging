using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.WareHouse.Services.Interface
{
    public interface IInvMatTransService
    {
        CommonResponse SaveMatTransaction(MaterialTransferDto materialDto, int pageId, int voucherId);
        CommonResponse SaveMatTransAdditional(MaterialTransAddDto materialTransAddDto, int transId, int voucherId);
        CommonResponse SaveMaterialReference(int transId, List<int?> referIds);
        CommonResponse UpdateMaterialReference(int transId, List<int?> referIds);
        CommonResponse SaveMaterialTransItems(List<InvTransItemDto> Items, int voucherId, int transId, int? toWh);
        CommonResponse UpdateMaterialTransItems(List<InvTransItemDto> Items, int voucherId, int transId, int? toWh);
        CommonResponse SaveMatTransEntries(int transId, int? account, int? branchAccount, decimal? amount, int voucherId);
        CommonResponse UpdateMatTransEntries(int transId, int? account, int? branchAccount, decimal? amount, int voucherId);
    }
}
