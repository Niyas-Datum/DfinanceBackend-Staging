using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.Vouchers
{
    public class PDCClearingTest
    {
        private Mock<IPdcClearingService> _pdc;
        [SetUp]
        public void Setup()
        {
            _pdc = new Mock<IPdcClearingService>();
        }
        [Test]
        public void SavePDC()
        {
            PdcClearingDto pdcClearingDto = new PdcClearingDto
            {
                Id = 0,
                VoucherNo = "0015",
                VoucherDate = DateTime.UtcNow,
                Narration = "test",
                BankName = new AccountNamePopUpDto
                {
                    ID = 1538,
                    Name = "vijal",
                    Alias = "string",

                },
                ChequeDetails = new List<CheqDetailView>
            {
                new CheqDetailView
                {
                    Selection = true,
                    ID = 3244,
                    VEID = 3965,
                    VID = 3424,
                    Posted = 0,
                    ChequeNo = null,
                    ChequeDate = null,
                    VDate = DateTime.Parse("2024-07-08T00:00:00"),
                    BankName = "Bankofbaroda",
                    PartyID = 61,
                    EntryID = null,
                    Party = "Cash In Hand",
                    Description = null,
                    Debit = 100,
                    Credit = null,
                    AccountID = 387,
                    AccountCode = "766901",
                    AccountName = "PDC Issued",
                    Status = "Not Posted"
                }

            }
            };
            _pdc.Setup(x => x.SavePdcClearing(pdcClearingDto, 74, 67)).Returns(new CommonResponse { Exception = null, Data = new PdcClearingDto() });
            var result = _pdc.Object.SavePdcClearing(pdcClearingDto, 74, 67);


            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }


        [Test]
        public void UpdatePDC()
        {
            PdcClearingDto pdcClearingDto = new PdcClearingDto
            {
                Id = 3325,
                VoucherNo = "0015",
                VoucherDate = DateTime.UtcNow,
                Narration = "test",
                BankName = new AccountNamePopUpDto
                {
                    ID = 1538,
                    Name = "vijal",
                    Alias = "string",

                },
                ChequeDetails = new List<CheqDetailView>
            {
                new CheqDetailView
                {
                    Selection = true,
                    ID = 3244,
                    VEID = 3965,
                    VID = 3424,
                    Posted = 0,
                    ChequeNo = null,
                    ChequeDate = null,
                    VDate = DateTime.Parse("2024-07-08T00:00:00"),
                    BankName = "Bankofbaroda",
                    PartyID = 61,
                    EntryID = null,
                    Party = "Cash In Hand",
                    Description = null,
                    Debit = 100,
                    Credit = null,
                    AccountID = 387,
                    AccountCode = "766901",
                    AccountName = "PDC Issued",
                    Status = "Not Posted"
                }

            }
            };
            _pdc.Setup(x => x.UpdatePDCclearing(pdcClearingDto, 74, 67)).Returns(new CommonResponse { Exception = null, Data = new PdcClearingDto() });
            var result = _pdc.Object.UpdatePDCclearing(pdcClearingDto, 74, 67);


            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }


       

        [Test]
        public void FillChqDet()
        {
            
            int bankId = 2538;
            _pdc.Setup(x => x.FillChequeDetails(bankId)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new CheqDetailView() });
            var result = _pdc.Object.FillChequeDetails(bankId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }


















    }
}
