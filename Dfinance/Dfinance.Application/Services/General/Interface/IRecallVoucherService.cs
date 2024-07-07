using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General
{
    public interface IRecallVoucherService
    {
        CommonResponse GetData();
        CommonResponse FillCancelledVouchers(int? accountId, int? vTypeId, DateTime? dateFrom, DateTime? dateUpTo, string? transactionNo);
        CommonResponse ApplyUpdateVoucher(string? Reason, int[] voucherID);
    }
}
