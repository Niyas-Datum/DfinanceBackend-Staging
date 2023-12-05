using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IBranchService
    {
        CommonResponse GetBranchesDropDown();
        CommonResponse SaveBranch(BranchDto companyDto);
        CommonResponse UpdateBranch(BranchDto companyDto, int Id);
        CommonResponse DeleteBranch(int Id);
        CommonResponse FillAllBranch();
        CommonResponse FillBranchByID(int Id);



    }
}
