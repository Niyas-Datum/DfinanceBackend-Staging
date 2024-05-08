using Dfinance.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Inventory
{
    public class CommonService
    {
        private readonly DFCoreContext _context;
        public CommonService(DFCoreContext context)
        {
            _context = context;
        }
        public int GetVoucherId(int pageId)
        {
            var check=_context.MaPageMenus.Any(m => m.Id == pageId);
            int voucherId;
            if (check)
                voucherId = _context.MaPageMenus.Where(m => m.Id == pageId).Select(m => m.VoucherId).FirstOrDefault() ?? 0;
            else
                voucherId = 0;
            return voucherId;
        }
    }
}
