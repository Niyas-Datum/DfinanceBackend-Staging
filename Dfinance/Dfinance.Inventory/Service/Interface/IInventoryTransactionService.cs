﻿using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{

    public interface IInventoryTransactionService
    {
        //Emplo
        CommonResponse GetSalesman();
        CommonResponse GetAutoVoucherNo(int voucherid);
        CommonResponse GetReference(int voucherno, DateTime? date = null);
        CommonResponse SaveTransaction(InventoryTransactionDto transactionDto,int PageId,int VoucherId, string Status);
        CommonResponse SaveTransactionPayment(InventoryTransactionDto transactionDto, int TransId, string Status, int VoucherId);
        CommonResponse SaveTransReference(int transId, List<int?> referIds);
        CommonResponse UpdateTransReference(int? transId, List<int?> referIds);
        CommonResponse SaveVoucherAllocation(int transId, int TransEntId,int? accountid,decimal? amount, List<int> referTransIds);
        CommonResponse UpdateVoucherAllocation(int transId, int TransEntId, int? accountid, decimal? amount, List<int> referTransIds);
        CommonResponse DeletePurchase(int TransId);

        CommonResponse FillVoucherType(int voucherId);
        CommonResponse FillRefItems(int transId);







    }
}
