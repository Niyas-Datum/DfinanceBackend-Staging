using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Finance.Interface
{
    public interface IVoucherService
    {
        CommonResponse VoucherPopup();
        CommonResponse FillVoucher();
       // CommonResponse SaveVoucher(VoucherDto voucherDto);
       // CommonResponse Deletevoucher(int ID);
        CommonResponse FillPrimaryVoucherName(int PrimaryVoucherId);
        CommonResponse UpdateVouchers(List<VoucherDto> voucherDtos);
        CommonResponse SaveVoucherNumbering(NumberingDto numberingDto);
        CommonResponse UpdateVoucherNumbering(NumberingDto numberingDto, int Id);
        CommonResponse DeleteVoucherNumbering(int Id);

        CommonResponse GetVoucherHistory(DateTime DateFrom, DateTime DateUpto, int branchid,
           bool Detailed , string transactionno, int? customersupplier ,
           int? item, int? voucher , int? PreVoucherID);


        CommonResponse VoucherDropDown();


    }
}
