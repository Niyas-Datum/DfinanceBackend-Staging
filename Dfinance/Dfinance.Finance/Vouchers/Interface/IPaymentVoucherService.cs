using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IPaymentVoucherService
    {
        CommonResponse FillAccountCode();
        CommonResponse SavePayVou(PaymentVoucherDto paymentVoucherDto, int PageId, int voucherId);
        CommonResponse UpdatePayVoucher(PaymentVoucherDto paymentVoucherDto, int PageId, int voucherId);
        CommonResponse DeletePayVoucher(int TransId, int pageId);
        CommonResponse GetPayVocherSettings();
    }
}
