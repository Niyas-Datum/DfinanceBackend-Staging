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
    public class BudgetMonthwiseTest
    {
        private Mock<IBudgetMonthwiseService> _budgetingServiceMock;
        [SetUp]
        public void Setup()
        {
            _budgetingServiceMock = new Mock<IBudgetMonthwiseService>();
        }
        [Test]
        public void FillBudgetPopupTest()
        {
            
            _budgetingServiceMock.Setup(x => x.FillBudgetMonthwise(153,"BalanceSheet")).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new VoucherView() });
            var result = _budgetingServiceMock.Object.FillBudgetMonthwise(153, "BalanceSheet");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
