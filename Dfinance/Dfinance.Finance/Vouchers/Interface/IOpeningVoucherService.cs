using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IOpeningVoucherService
    {
        CommonResponse SaveOpeningVoucher(OpeningVoucherDto openVouDto, int PageId, int VoucherId);
        CommonResponse UpdateOpeningVoucher(OpeningVoucherDto openVouDto, int PageId, int VoucherId);
        CommonResponse DeleteOpeningVou(OpeningVoucherDto openVouDto, int pageId);
    }
}
