using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.Vouchers
{
    public class CreditDebitTest
    {
        private Mock<ICreditDebitNoteService> _creditdebit;
        [SetUp]
        public void Setup()
        {
            _creditdebit = new Mock<ICreditDebitNoteService>();
        }
        [Test]
        public void SaveCreditDebit()
        {
            DebitCreditDto CDNoteDto = new DebitCreditDto
            {
                Id = 0,
                VoucherNo = "0002",
                VoucherDate = DateTime.Parse("2024-07-09T09:51:53.375Z"),
                Reference = null,
                Party = new PopUpDto
                {
                    Id = 2540,
                    Name = "ahalya",
                    Code = "string",
                    Description = ""
                },
                Particulars = new DropdownDto
                {
                    Id = 990,
                    Value = "purchasereturn"
                },
                Narration = "test@9",
                accountDetails = new List<AccountDetail>
        {
            new AccountDetail
            {
                AccountCode = new PopUpDto
                {
                    Id = 35,
                    Name = "sales a/c",
                    Code = "string",
                    Description = "string"
                },
                Description = "string",
                DueDate = DateTime.Parse("2024-07-09T09:51:53.375Z"),
                Debit = 100,
                Credit = 0
            }
        },
                billandRef = new List<FillAdvanceView>
        {
            new FillAdvanceView
            {
                VID = 0,
                VEID = 4055,
                VNo = "string",
                VDate = DateTime.Parse("2024-07-10T04:54:51.316Z"),
                Description = "string",
                BillAmount = 0,
                Amount = 20,
                AccountID = 2540,
                Selection = true,
                Allocated = 0,
                Account = "string",
                DrCr = "string",
                PartyInvNo = 0,
                PartyInvDate = DateTime.Parse("2024-07-10T04:54:51.316Z")
            }
        }
            };
            _creditdebit.Setup(x => x.SaveCreditDebitNote(CDNoteDto, 251, 77)).Returns(new CommonResponse { Exception = null, Data = new DebitCreditDto() });
            var result = _creditdebit.Object.SaveCreditDebitNote(CDNoteDto, 251, 77);


            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);

        }

        [Test]

        public void UpdateCreditDebit()
        {
            DebitCreditDto CDNoteDto = new DebitCreditDto
            {
                Id = 3457,
                VoucherNo = "0002",
                VoucherDate = DateTime.Parse("2024-07-09T09:51:53.375Z"),
                Reference = null,
                Party = new PopUpDto
                {
                    Id = 2540,
                    Name = "ahalya",
                    Code = "string",
                    Description = ""
                },
                Particulars = new DropdownDto
                {
                    Id = 990,
                    Value = "purchasereturn"
                },
                Narration = "test@9",
                accountDetails = new List<AccountDetail>
        {
            new AccountDetail
            {
                AccountCode = new PopUpDto
                {
                    Id = 35,
                    Name = "sales a/c",
                    Code = "string",
                    Description = "string"
                },
                Description = "string",
                DueDate = DateTime.Parse("2024-07-09T09:51:53.375Z"),
                Debit = 100,
                Credit = 0
            }
        },
                billandRef = new List<FillAdvanceView>
        {
            new FillAdvanceView
            {
                VID = 0,
                VEID = 4055,
                VNo = "string",
                VDate = DateTime.Parse("2024-07-10T04:54:51.316Z"),
                Description = "string",
                BillAmount = 0,
                Amount = 20,
                AccountID = 2540,
                Selection = true,
                Allocated = 0,
                Account = "string",
                DrCr = "string",
                PartyInvNo = 0,
                PartyInvDate = DateTime.Parse("2024-07-10T04:54:51.316Z")
            }
        }
            };
            _creditdebit.Setup(x => x.UpdateCreditDebitNote(CDNoteDto, 251, 77)).Returns(new CommonResponse { Exception = null, Data = new DebitCreditDto() });
            var result = _creditdebit.Object.UpdateCreditDebitNote(CDNoteDto, 251, 77);


            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);

        }
        [Test]
        public void Deletecreditdebit()
        {
            int transId = 3164;
            int pageId = 69;
            _creditdebit.Setup(x => x.DeleteDebitCreditNote(transId, pageId)).Returns(new CommonResponse { Exception = null, Data = new DebitCreditDto() });
            var result = _creditdebit.Object.DeleteDebitCreditNote(transId, pageId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void Cancelcreditdebit()
        {
            int transId = 3164;
            string reason = "cancel";
            _creditdebit.Setup(x => x.CancelDebitCreditNote(transId, reason)).Returns(new CommonResponse { Exception = null, Data = new DebitCreditDto() });
            var result = _creditdebit.Object.CancelDebitCreditNote(transId, reason);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }




    }
}
