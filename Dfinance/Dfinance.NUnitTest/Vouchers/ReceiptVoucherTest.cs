using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.Vouchers
{
    public class ReceiptVoucherTest
    {
        private Mock<IReceiptVoucherService> _receiptVoucher;
        [SetUp]
        public void Setup()
        {
            _receiptVoucher = new Mock<IReceiptVoucherService>();
        }
        [Test]
        public void SavereceipVoucher()
        {
            FinanceTransactionDto receiptvoucher = new FinanceTransactionDto
            {
                Id = 0,
                VoucherNo = "0043",
                VoucherDate = DateTime.Parse("2024-06-12T12:36:53.933Z"),
                Narration = "string",
                CostCentre = new PopUpDto
                {
                    Id = null,
                    Name = "string",
                    Code = "string",
                    Description = "string"
                },
                Department = new PopUpDto
                {
                    Id = 93,
                    Name = "string",
                    Code = "string",
                    Description = "string"
                },
                ReferenceNo = "string",
                Currency = new DropdownDto
                {
                    Id = 1,
                    Value = "string"
                },
                ExchangeRate = 1,
                AccountDetails = new List<AccountDetails>
        {
            new AccountDetails
            {

                AccountCode = new PopUpDto
                {
                    Id = 1538,
                    Name = "vijal",
                    Code = "string",
                    Description = "string"
                },

                Description = "string",
                Amount = 500,

                DueDate = DateTime.Parse("2024-06-12T12:36:53.933Z")
            }
        },
                BillandRef = new List<BillandRef>
        {
            new BillandRef
            {
                        Selection = true,
                        InvoiceNo = "string",
                        InvoiceDate = DateTime.Parse("2024-06-20T10:09:27.821Z"),
                        PartyInvNo = 0,
                        PartyInvDate = DateTime.Parse("2024-06-20T10:09:27.821Z"),
                        Description = "string",
                        Account = "string",
                        InvoiceAmount = 0,
                        Allocated = 0,
                        Amount = 100,
                        Balance = 0,
                        VID = 0,
                        VEID = 2103,
                        AccountID = 1538
            }
        },
                Cash = new List<PaymentType>
        {
            new PaymentType
            {

                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },

                Description = "string",
                Amount = 200,
                TransType = "cash",


            }
        },
                Card = new List<PaymentType>
        {
            new PaymentType
            {

                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },

                Description = "string",
                Amount = 200,
                 TransType = "card",

            }
        },
                Epay = new List<PaymentType>
        {
            new PaymentType
            {

                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 868
                },

                Description = "string",
                Amount = 100,
                TransType = "Online",

            }
        },
                Cheque = new List<ChequeDto>
        {
            new ChequeDto
            {
                PDCPayable = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
                VEID = 0,
                CardType = "string",
                Commission = 0,
                ChequeNo = "string",
                ChequeDate = DateTime.Parse("2024-06-12T12:36:53.933Z"),
                ClearingDays = 0,
                BankID = 0,
                BankName = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
                Status = "string",
                PartyID = 0,
                Description = "string",
                Amount = 0,
                 TransType = "Cheque",
            }
        }
            };
            _receiptVoucher.Setup(x => x.SaveReceiptVou(receiptvoucher, 70, 7)).Returns(new CommonResponse { Exception = null, Data = new FinanceTransactionDto() });

            //Act
            var result = _receiptVoucher.Object.SaveReceiptVou(receiptvoucher, 70, 7);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]

        public void UpdatereceiptVoucher()
        {
            FinanceTransactionDto receiptvoucher = new FinanceTransactionDto
            {
                Id = 3216,
                VoucherNo = "0043",
                VoucherDate = DateTime.Parse("2024-06-12T12:36:53.933Z"),
                Narration = "string",
                CostCentre = new PopUpDto
                {
                    Id = null,
                    Name = "string",
                    Code = "string",
                    Description = "string"
                },
                Department = new PopUpDto
                {
                    Id = 93,
                    Name = "string",
                    Code = "string",
                    Description = "string"
                },
                ReferenceNo = "string",
                Currency = new DropdownDto
                {
                    Id = 1,
                    Value = "string"
                },
                ExchangeRate = 1,
                AccountDetails = new List<AccountDetails>
        {
            new AccountDetails
            {

                AccountCode = new PopUpDto
                {
                    Id = 1538,
                    Name = "vijal",
                    Code = "string",
                    Description = "string"
                },

                Description = "string",
                Amount = 500,

                DueDate = DateTime.Parse("2024-06-12T12:36:53.933Z")
            }
        },
                BillandRef = new List<BillandRef>
        {
            new BillandRef
            {
                        Selection = true,
                        InvoiceNo = "string",
                        InvoiceDate = DateTime.Parse("2024-06-20T10:09:27.821Z"),
                        PartyInvNo = 0,
                        PartyInvDate = DateTime.Parse("2024-06-20T10:09:27.821Z"),
                        Description = "string",
                        Account = "string",
                        InvoiceAmount = 0,
                        Allocated = 0,
                        Amount = 100,
                        Balance = 0,
                        VID = 0,
                        VEID = 2103,
                        AccountID = 1538
            }
        },
                Cash = new List<PaymentType>
        {
            new PaymentType
            {

                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },

                Description = "string",
                Amount = 200,


            }
        },
                Card = new List<PaymentType>
        {
            new PaymentType
            {

                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },

                Description = "string",
                Amount = 200,


            }
        },
                Epay = new List<PaymentType>
        {
            new PaymentType
            {

                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 868
                },

                Description = "string",
                Amount = 100,


            }
        },
                Cheque = new List<ChequeDto>
        {
            new ChequeDto
            {
                PDCPayable = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
                VEID = 0,
                CardType = "string",
                Commission = 0,
                ChequeNo = "string",
                ChequeDate = DateTime.Parse("2024-06-12T12:36:53.933Z"),
                ClearingDays = 0,
                BankID = 0,
                BankName = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
                Status = "string",
                PartyID = 0,
                Description = "string",
                Amount = 0
            }
        }
            };

            _receiptVoucher.Setup(x => x.UpdateReceiptVoucher(receiptvoucher, 70, 7)).Returns(new CommonResponse { Exception = null, Data = new FinanceTransactionDto() });

            //Act
            var result = _receiptVoucher.Object.UpdateReceiptVoucher(receiptvoucher, 70, 7);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void DeletepayVoucher()
        {
            int transId = 3164;
            int pageId = 70;
            _receiptVoucher.Setup(x => x.DeleteReceiptVoucher(transId, pageId)).Returns(new CommonResponse { Exception = null, Data = new FinanceTransactionDto() });
            var result = _receiptVoucher.Object.DeleteReceiptVoucher(transId, pageId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }




    }
}