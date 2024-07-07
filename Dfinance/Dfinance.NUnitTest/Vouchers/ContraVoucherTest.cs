using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.General;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.NUnitTest.Vouchers
{
    public class ContraVoucherTest
    {
        private Mock<IContraVoucherService> _contraVoucher;
        [SetUp]
        public void Setup()
        {
            _contraVoucher = new Mock<IContraVoucherService>();
        }
        [Test]
        public void SaveContraVoucher()
        {
            ContraDto contraDto = new ContraDto
            {
                Id = 0,
                VoucherNo = "0001",
                VoucherDate = DateTime.Parse("2024-07-01T05:01:57.237Z"),
                Narration = "Sample narration",
                CostCentre = new PopUpDto
                {
                    Id = null,
                    Name = "Sample Cost Centre",
                    Code = "CC001",
                    Description = "Description of Cost Centre"
                },
                ReferenceNo = "Reference001",
                Currency = new DropdownDto
                {
                    Id = 1,
                    Value = "USD"
                },
                ExchangeRate = 1,
                AccountDetails = new List<AccountDetails>
                {
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1538,
                        Name = "Account 1",
                        Code = "ACC001",
                        Description = "Description of Account 1"
                    },
                    Description = "Transaction 1",
                    DueDate = DateTime.Parse("2024-07-01T05:01:57.237Z"),
                    Debit = 20,
                    Credit = 0
                },
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1538,
                        Name = "Account 1",
                        Code = "ACC001",
                        Description = "Description of Account 1"
                    },
                    Description = "Transaction 2",
                    DueDate = DateTime.Parse("2024-07-01T05:01:57.237Z"),
                    Debit = 0,
                    Credit = 20
                }
            }
            };
            _contraVoucher.Setup(x => x.SaveContraVou(contraDto, 71, 1)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });

            //Act
            var result = _contraVoucher.Object.SaveContraVou(contraDto, 71, 1);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void UpdateContraVoucher()
        {
            ContraDto contraDto = new ContraDto
            {
                Id = 3323,
                VoucherNo = "0001",
                VoucherDate = DateTime.Parse("2024-07-01T05:01:57.237Z"),
                Narration = "Sample narration",
                CostCentre = new PopUpDto
                {
                    Id = null,
                    Name = "Sample Cost Centre",
                    Code = "CC001",
                    Description = "Description of Cost Centre"
                },
                ReferenceNo = "Reference001",
                Currency = new DropdownDto
                {
                    Id = 1,
                    Value = "USD"
                },
                ExchangeRate = 1,
                AccountDetails = new List<AccountDetails>
                {
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1538,
                        Name = "Account 1",
                        Code = "ACC001",
                        Description = "Description of Account 1"
                    },
                    Description = "Transaction 1",
                    DueDate = DateTime.Parse("2024-07-01T05:01:57.237Z"),
                    Debit = 20,
                    Credit = 0
                },
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1538,
                        Name = "Account 1",
                        Code = "ACC001",
                        Description = "Description of Account 1"
                    },
                    Description = "Transaction 2",
                    DueDate = DateTime.Parse("2024-07-01T05:01:57.237Z"),
                    Debit = 0,
                    Credit = 20
                }
            }
            };
            _contraVoucher.Setup(x => x.UpdateContraVou(contraDto, 71, 1)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });

            //Act
            var result = _contraVoucher.Object.UpdateContraVou(contraDto, 71, 1);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void DeletepayVoucher()
        {
            int transId = 3323;
            int pageId = 71;
            _contraVoucher.Setup(x => x.DeleteContraVou(transId, pageId)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });
            var result = _contraVoucher.Object.DeleteContraVou(transId, pageId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillAccCode()
        {
            _contraVoucher.Setup(x=>x.FillAccCode()).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });
            var result = _contraVoucher.Object.FillAccCode();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }


    }
}
