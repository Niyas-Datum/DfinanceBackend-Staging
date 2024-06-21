using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IReceiptVoucherService
    {
        CommonResponse FillMaVoucher(int? VoucherId, int? PageId);
        CommonResponse FillMaster(int? TransId, int? PageId);
        CommonResponse SaveReceiptVou(FinanceTransactionDto receiptVoucherDto, int PageId, int voucherId);
        CommonResponse UpdateReceiptVoucher(FinanceTransactionDto receiptVoucherDto, int PageId, int voucherId);
        CommonResponse DeleteReceiptVoucher(int TransId, int pageId);
    }
}
