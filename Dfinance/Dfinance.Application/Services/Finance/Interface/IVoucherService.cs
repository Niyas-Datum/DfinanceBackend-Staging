using Dfinance.Application.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IVoucherService
    {
        CommonResponse FillVoucher();
       // CommonResponse SaveVoucher(VoucherDto voucherDto);
       // CommonResponse Deletevoucher(int ID);
        CommonResponse FillPrimaryVoucherName(int PrimaryVoucherId);
        CommonResponse UpdateVouchers(List<VoucherDto> voucherDtos);
        CommonResponse SaveVoucherNumbering(NumberingDto numberingDto);
        CommonResponse UpdateVoucherNumbering(NumberingDto numberingDto, int Id);
        CommonResponse DeleteVoucherNumbering(int Id);


    }
}
