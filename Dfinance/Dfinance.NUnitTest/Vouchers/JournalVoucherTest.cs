using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.NUnitTest.Vouchers
{
    public class JournalVoucherTest
    {
        private Mock<IJournalVoucherService> _journal;
        [SetUp]
        public void Setup()
        {
            _journal = new Mock<IJournalVoucherService>();
        }
        [Test]
        public void SaveJournalTest()
        {
            var journalDto = new JournalDto
            {
                Id = 1,
                VoucherNo = "VN123",
                VoucherDate = DateTime.Now,
                Narration = "Sample narration",
                CostCentre = new PopUpDto { Id = 1, Name = "" },
                ReferenceNo = "REF123",
                Currency = new DropdownDto { Id = 1, Value = "" },
                ExchangeRate = 1.2m,
                AccountData = new List<AccountData>
            {
                new AccountData
                {
                    AccountCode = new PopUpDto { Id = 1536, Name = "" },
                    Description = "Account description",
                    DueDate = DateTime.Now.AddDays(30),
                    Amount = 100.50m,
                    Debit = 50.25m,
                    Credit = 50.25m,
                    BillandRef = new List<BillandRef>
                    {
                        new BillandRef
                        {
                            Selection = true,
                            InvoiceNo = "INV123",
                            InvoiceDate = DateTime.Now,
                            PartyInvNo = 456,
                            PartyInvDate = DateTime.Now.AddDays(-10),
                            Description = "Bill description",
                            Account = "Account 1",
                            InvoiceAmount = 200.75m,
                            Allocated = 100.75m,
                            Amount = 50.25m,
                            Balance = 150.50m,
                            VID = 789,
                            VEID = 101112,
                            AccountID = 13
                        }
                    }
                }
            }
            };
            _journal.Setup(x => x.SaveJournalVoucher(68,6,journalDto)).Returns(new CommonResponse { Exception = null, Data = new JournalDto() });           
            var result = _journal.Object.SaveJournalVoucher(68, 6, journalDto);
            Assert.That(result, Is.Not.Null);     
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateJournalTest()
        {
            var journalDto = new JournalDto
            {
                Id = 1,
                VoucherNo = "VN123",
                VoucherDate = DateTime.Now,
                Narration = "Sample narration",
                CostCentre = new PopUpDto { Id = 1, Name = "" },
                ReferenceNo = "REF123",
                Currency = new DropdownDto { Id = 1, Value = "" },
                ExchangeRate = 1.2m,
                AccountData = new List<AccountData>
            {
                new AccountData
                {
                    AccountCode = new PopUpDto { Id = 1536, Name = "" },
                    Description = "Account description",
                    DueDate = DateTime.Now.AddDays(30),
                    Amount = 100.50m,
                    Debit = 50.25m,
                    Credit = 50.25m,
                    BillandRef = new List<BillandRef>
                    {
                        new BillandRef
                        {
                            Selection = true,
                            InvoiceNo = "INV123",
                            InvoiceDate = DateTime.Now,
                            PartyInvNo = 456,
                            PartyInvDate = DateTime.Now.AddDays(-10),
                            Description = "Bill description",
                            Account = "Account 1",
                            InvoiceAmount = 200.75m,
                            Allocated = 100.75m,
                            Amount = 50.25m,
                            Balance = 150.50m,
                            VID = 789,
                            VEID = 101112,
                            AccountID = 13
                        }
                    }
                }
            }
            };
            _journal.Setup(x => x.UpdateJournalVoucher(68, 6, journalDto)).Returns(new CommonResponse { Exception = null, Data = new JournalDto() });
            var result = _journal.Object.UpdateJournalVoucher(68, 6, journalDto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
