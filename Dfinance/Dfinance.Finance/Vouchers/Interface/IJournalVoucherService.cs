using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;

namespace Dfinance.Finance.Vouchers.Interface
{
    public interface IJournalVoucherService
    {
        CommonResponse FillAccounts(string? criteria = null);
        CommonResponse SaveJournalVoucher(int pageId, int voucherId, JournalDto journalDto);
        CommonResponse UpdateJournalVoucher(int pageId, int voucherId, JournalDto journalDto);
    }
}
