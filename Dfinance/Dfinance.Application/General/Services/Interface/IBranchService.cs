using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.General.Services.Interface
{
    public interface IBranchService
    {
        CommonResponse GetBranches();
        CommonResponse Fillallbranch();
        CommonResponse SaveBranch(MaCompanyDto companyDto);
        CommonResponse UpdateBranch(MaCompanyDto companyDto,int Id);
        CommonResponse DeleteBranch(int Id);

    }
}
