using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IBatchEditService
    {
        CommonResponse GetLoadData();
        CommonResponse BatchDetailsForUpdate(BatchEditDto batchEditDto);
        CommonResponse UpdateBatchNo(int pageId, string? vNo, string? batchNo = null, string? newBatchNo = null, DateTime? expiryDate = null);
    }
}
