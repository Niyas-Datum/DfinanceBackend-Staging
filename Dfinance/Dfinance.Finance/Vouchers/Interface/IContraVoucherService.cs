using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IContraVoucherService
    {
        CommonResponse FillAccCode();
        CommonResponse SaveContraVou(ContraDto contraDto, int PageId, int voucherId);
        CommonResponse UpdateContraVou(ContraDto contraDto, int PageId, int voucherId);
        CommonResponse DeleteContraVou(int TransId, int pageId);

    }
}
