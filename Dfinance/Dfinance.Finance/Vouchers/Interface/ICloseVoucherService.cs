using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface ICloseVoucherService
    {
        CommonResponse GetLoadData();
        CommonResponse Fill(CloseVoucherDto closeVoucher);
        CommonResponse CloseVoucherUpdate(int PageId, List<int> Ids);
    }
}
