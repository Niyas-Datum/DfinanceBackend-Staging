using Dfinance.Application.Services.General;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.General
{
    public class RecallVoucherTest
    {
        private Mock<IRecallVoucherService> _recallVoucher;
        [SetUp]
        public void Setup()
        {
            _recallVoucher = new Mock<IRecallVoucherService>();
        }
        [Test]
        public void GetDataTest()

        {
            _recallVoucher.Setup(x => x.GetData()).Returns(new CommonResponse { Exception = null, Data = new RecallVoucherView() });
            var result = _recallVoucher.Object.GetData();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void GetCancelVoucher()

        {
            int accountId = 1536;
            int vtypeId = 1;
            DateTime fromdate= DateTime.Now;
            DateTime todate= DateTime.Now;
            string transNo = "3010";
            _recallVoucher.Setup(x => x.FillCancelledVouchers(accountId,vtypeId,fromdate,todate,transNo)).Returns(new CommonResponse { Exception = null, Data = new RecallVoucherView() });
            var result = _recallVoucher.Object.FillCancelledVouchers(accountId, vtypeId, fromdate, todate, transNo);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void ApplyUpdateVoucher()

        {
            string reason = "test1";
            int[] id = new int[] { 1, 2 };
            _recallVoucher.Setup(x => x.ApplyUpdateVoucher(reason,id)).Returns(new CommonResponse { Exception = null, Data = new RecallVoucherView() });
            var result = _recallVoucher.Object.ApplyUpdateVoucher(reason, id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
