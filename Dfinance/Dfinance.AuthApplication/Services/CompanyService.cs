using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.AuthAppllication.Services
{

    public class CompanyService : ICompanyService
    {
        private readonly DFCoreContext _cont;
        private readonly AuthCoreContext _context;
        public CompanyService(AuthCoreContext context, DFCoreContext cont)
        {
            _context = context;
            _cont = cont;
        }
        public CommonResponse GetCompanies()
        {
            //sp execution
            var  data = _cont.SpMacompanyFillallbranch.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillallbranch'");
            return CommonResponse.Ok(_context.Companies.ToList());
        }
    }
}