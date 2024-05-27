using Dfinance.Core.Views.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Common
{
    public class VoucherNo
    {
        public string Code { get; set; }
        public List<AccountCodeView> Result { get; set; }
    }
}
