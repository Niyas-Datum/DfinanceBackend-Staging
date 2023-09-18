using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.AuthAppllication.Services
{

    public class CompanyService : ICompanyService
    {
        private readonly DFCoreContext _context1;
        private readonly AuthCoreContext _context;
        public CompanyService(AuthCoreContext context, DFCoreContext context1)
        {
            _context = context;
            _context1 = context1;
        }
        public CommonResponse GetCompanies()
        {
            var d = _context1.SpMacompanyFillallbranch.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillallbranch'");
            return CommonResponse.Ok(d);
        }
    }
}