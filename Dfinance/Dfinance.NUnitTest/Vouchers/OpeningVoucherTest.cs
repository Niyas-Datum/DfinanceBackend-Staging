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
    public class OpeningVoucherTest
    {
        private Mock<IOpeningVoucherService> _openingVoucher;
        [SetUp]
        public void Setup()
        {
            _openingVoucher = new Mock<IOpeningVoucherService>();
        }
        [Test]
        public void SaveOpeningVoucher()
        {
            OpeningVoucherDto openDto = new OpeningVoucherDto
            {
                Id = 0,
                VoucherNo = "0003",
                VoucherDate = DateTime.UtcNow, // Replace with your desired DateTime
                Narration = "testopenvou",
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
                    DueDate = DateTime.UtcNow, // Replace with your desired DateTime
                    Amount = 0,
                    Debit = 110,
                    Credit = 0
                },
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1537,
                        Name = "nith",
                        Code = "string",
                        Description = "string"
                    },
                    Description = "string",
                    DueDate = DateTime.UtcNow, // Replace with your desired DateTime
                    Amount = 0,
                    Debit = 0,
                    Credit = 120
                }
            }
            };
            _openingVoucher.Setup(x => x.SaveOpeningVoucher(openDto, 73, 26)).Returns(new CommonResponse { Exception = null, Data = new OpeningVoucherDto() });

            //Act
            var result = _openingVoucher.Object.SaveOpeningVoucher(openDto, 73, 26);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateOpeningVoucher()
        {
            OpeningVoucherDto openDto = new OpeningVoucherDto
            {
                Id = 3557,
                VoucherNo = "0003",
                VoucherDate = DateTime.UtcNow, // Replace with your desired DateTime
                Narration = "testopenvou",
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
                    DueDate = DateTime.UtcNow, // Replace with your desired DateTime
                    Amount = 0,
                    Debit = 110,
                    Credit = 0
                },
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1537,
                        Name = "nith",
                        Code = "string",
                        Description = "string"
                    },
                    Description = "string",
                    DueDate = DateTime.UtcNow, // Replace with your desired DateTime
                    Amount = 0,
                    Debit = 0,
                    Credit = 120
                }
            }
            };
            _openingVoucher.Setup(x => x.UpdateOpeningVoucher(openDto, 73, 26)).Returns(new CommonResponse { Exception = null, Data = new OpeningVoucherDto() });

            //Act
            var result = _openingVoucher.Object.UpdateOpeningVoucher(openDto, 73, 26);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteOpeningVoucher()
        {

            OpeningVoucherDto openDto = new OpeningVoucherDto
            {
                Id = 3557,
                VoucherNo = "0003",
                VoucherDate = DateTime.UtcNow, 
                Narration = "testopenvou",
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
                    DueDate = DateTime.UtcNow, 
                    Amount = 0,
                    Debit = 110,
                    Credit = 0
                },
                new AccountDetails
                {
                    AccountCode = new PopUpDto
                    {
                        Id = 1537,
                        Name = "nith",
                        Code = "string",
                        Description = "string"
                    },
                    Description = "string",
                    DueDate = DateTime.UtcNow,
                    Amount = 0,
                    Debit = 0,
                    Credit = 120
                }
            }
            };

            _openingVoucher.Setup(x => x.DeleteOpeningVou(openDto, 73)).Returns(new CommonResponse { Exception = null, Data = new OpeningVoucherDto() });
            var result = _openingVoucher.Object.DeleteOpeningVou(openDto, 73);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }




    }
}

