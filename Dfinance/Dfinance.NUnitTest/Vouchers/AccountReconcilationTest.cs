using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.NUnitTest.Vouchers
{
    public class AccountReconcilationTest
    {
        private Mock<IAccountReconciliationService> _accountreco;
        [SetUp]
        public void Setup()
        {
            _accountreco = new Mock<IAccountReconciliationService>();
        }
        [Test]
        public void FillAccountReconcilation()
        {
            DateOnly FromDate = new DateOnly(2024, 4, 7);
            DateOnly ToDate = new DateOnly(2024, 7, 3);
            int AccountID = 1537;
            _accountreco.Setup(x => x.FillAccountReconcilation(FromDate, ToDate, AccountID)).Returns(new CommonResponse { Exception = null, Data = new AccountReconcilationView() });
            var result = _accountreco.Object.FillAccountReconcilation(FromDate, ToDate, AccountID);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
        [Test]
        public void UpdateAccountReconcilation()
        {
            int TranEntryId = 3755;
            DateTime BankDate = new DateTime(2024, 7, 3, 10, 30, 0);
            _accountreco.Setup(x => x.UpdateAccountReconcilation(TranEntryId, BankDate))
                        .Returns(new CommonResponse { Exception = null, Data = new AccountReconcilationView() });
            var result = _accountreco.Object.UpdateAccountReconcilation(TranEntryId, BankDate);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }



    }
}

