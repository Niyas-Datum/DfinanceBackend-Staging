using Dfinance.Core.Views.Common;
using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.Finance
{
    public class BudgetingTest
    {
        private Mock<IBudgetingService> _budgetingServiceMock;
        [SetUp]
        public void Setup()
        {
            _budgetingServiceMock = new Mock<IBudgetingService>();
        }
        [Test]
        public void FillMasterTest()
        {
            int transid = 4000;
            _budgetingServiceMock.Setup(x => x.FillMaster(transid, 153,70)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new VoucherView() });
            var result = _budgetingServiceMock.Object.FillMaster(transid, 153, 70);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillProfitLossBalsheetTest()
        {
            bool? profitLoss = true;
            bool? balsheet = false;
            _budgetingServiceMock.Setup(x => x.FillProfitLossBalsheet(profitLoss, balsheet)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new ReadViewAlias() });
            var result = _budgetingServiceMock.Object.FillProfitLossBalsheet(profitLoss, balsheet);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SaveBudgetTest()
        {
            var budgetingDto = new BudgetingDto
            {
                TransactionId = 1,
                VoucherNo = "0001",
                VoucherDate = DateTime.Now,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 12, 31),
                Name = "Annual Budget",
                Type = new DropdownDto
                {
                    Id = 1,
                    Value = "Annual"
                },
                Narration = "Annual budgeting for the year 2024",
                ProfitAndLoss = true,
                BalanceSheet = true,
                BudgetAccounts = new List<BudgetAccount>
            {
                new BudgetAccount
                {
                    Account = new AccountDto
                    {
                        ID = 1001,
                        AccountName = "Sales"
                    },
                    Amount = 100000,
                    January = 8000,
                    February = 8500,
                    March = 9000,
                    April = 9500,
                    May = 10000,
                    June = 10500,
                    July = 11000,
                    August = 11500,
                    September = 12000,
                    October = 12500,
                    November = 13000,
                    December = 13500
                },
                new BudgetAccount
                {
                    Account = new AccountDto
                    {
                        ID = 1002,
                        AccountName = "Marketing",
                        AccountCode=""
                    },
                    Amount = 50000,
                    January = 4000,
                    February = 4200,
                    March = 4400,
                    April = 4600,
                    May = 4800,
                    June = 5000,
                    July = 5200,
                    August = 5400,
                    September = 5600,
                    October = 5800,
                    November = 6000,
                    December = 6200
                }
            }
            };
            _budgetingServiceMock.Setup(x => x.SaveBudget(budgetingDto, 153,70)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new BudgetingDto() });
            var result = _budgetingServiceMock.Object.SaveBudget(budgetingDto, 153, 70);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteBudgetTest()
        {
            var budgetingDto = new BudgetingDto
            {
                TransactionId = 1,
                VoucherNo = "0001",
                VoucherDate = DateTime.Now,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 12, 31),
                Name = "Annual Budget",
                Type = new DropdownDto
                {
                    Id = 1,
                    Value = "Annual"
                },
                Narration = "Annual budgeting for the year 2024",
                ProfitAndLoss = true,
                BalanceSheet = true,
                BudgetAccounts = new List<BudgetAccount>
            {
                new BudgetAccount
                {
                    Account = new AccountDto
                    {
                        ID = 1001,
                        AccountName = "Sales"
                    },
                    Amount = 100000,
                    January = 8000,
                    February = 8500,
                    March = 9000,
                    April = 9500,
                    May = 10000,
                    June = 10500,
                    July = 11000,
                    August = 11500,
                    September = 12000,
                    October = 12500,
                    November = 13000,
                    December = 13500
                },
                new BudgetAccount
                {
                    Account = new AccountDto
                    {
                        ID = 1002,
                        AccountName = "Marketing",
                        AccountCode=""
                    },
                    Amount = 50000,
                    January = 4000,
                    February = 4200,
                    March = 4400,
                    April = 4600,
                    May = 4800,
                    June = 5000,
                    July = 5200,
                    August = 5400,
                    September = 5600,
                    October = 5800,
                    November = 6000,
                    December = 6200
                }
            }
            };
            _budgetingServiceMock.Setup(x => x.DeleteBudget(budgetingDto, 153, 70,true)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new BudgetingDto() });
            var result = _budgetingServiceMock.Object.DeleteBudget(budgetingDto, 153, 70,true);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
