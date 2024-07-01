using Dfinance.Core.Views.Finance;
using Dfinance.Finance.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.Finance
{
    public class BudgetRegisterTest
    {
        private Mock<IBudgetRegisterService> _budgetingServiceMock;
        [SetUp]
        public void Setup()
        {
            _budgetingServiceMock = new Mock<IBudgetRegisterService>();
        }
        [Test]
        public void FillBudgetPopupTest()
        {
            int transid = 4000;
            _budgetingServiceMock.Setup(x => x.FillBudgetPopup(70)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new VoucherView() });
            var result = _budgetingServiceMock.Object.FillBudgetPopup(70);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillBudgetReportTest()
        {
            _budgetingServiceMock.Setup(x => x.FillBudgetReport(155,"ProfitAndLoss")).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new VoucherView() });
            var result = _budgetingServiceMock.Object.FillBudgetReport(155, "ProfitAndLoss");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
