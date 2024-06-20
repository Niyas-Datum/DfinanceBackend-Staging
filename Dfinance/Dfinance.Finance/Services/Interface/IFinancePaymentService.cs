﻿using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services.Interface
{
    public interface IFinancePaymentService
    {
        //CommonResponse FillTax();
        //CommonResponse FillAddCharge();
        CommonResponse FillCash();
        CommonResponse FillCard();
        CommonResponse FillEpay();
        CommonResponse FillAdvance(int AccountId, string Drcr, DateTime? date);
        CommonResponse FillCheque();
        CommonResponse FillBankName();
        CommonResponse SaveCheque(ChequeDto chequeDto, int VEId, int? PartyId);
        CommonResponse SaveTransactionEntries(PaymentVoucherDto paymentVoucherDto, int pageId, int transactionId, int transPayId);
       // CommonResponse SaveTransactionExpenses(List<InvAccountDetailsDto> accountDetailsDtos, int transactionId, string tranType);
       // CommonResponse UpdateTransactionExpenses(List<InvAccountDetailsDto> accountDetailsDtos, int transactionId, string tranType);
       // CommonResponse SaveTransCollections(InvTransactionEntriesDto transactionEntries, int transactionId);
      //  CommonResponse SaveTransCollectionsAllocation(InvTransactionEntriesDto transactionEntries, int transCollectionId, int vAllocId);

    }
}
