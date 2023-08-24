using Dfinance.AuthCore.Infrastructure;
using Dfinance.Shared.Domain;

namespace Dfinance.AuthAppllication.Services
{

    public class CompanyService : ICompanyService
    {
        private readonly AuthCoreContext _context;
        public CompanyService(AuthCoreContext context)
        {
            _context = context;
        }
        public CommonResponse GetCompanies()
        {
            return CommonResponse.Ok(_context.Companies.ToList());
        }
    }
}