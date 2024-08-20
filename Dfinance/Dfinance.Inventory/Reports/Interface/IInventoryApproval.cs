using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Inventory.Reports.Interface
{
    public interface IInventoryApproval
    {
        CommonResponse FillInventoryApproval(DateTime FromDate, DateTime ToDate, int BranchID, int ApprovalStatus,int pageId, int? ModeID = null, string? MachineName = null, int? VTypeID = null, int? Detailed = null, int? UserID = null, int? VoucherID = null, bool? AutoEntry = null, int? TransactionID = null);




    }
}
