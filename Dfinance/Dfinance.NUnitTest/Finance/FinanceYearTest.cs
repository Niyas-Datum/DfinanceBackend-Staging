using Dfinance.Application.Services.Finance.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.NUnitTest.Finance
{
    public class FinanceYearTest
    {
        private Mock<IFinanceYearService> _iFinanveYear;
        [SetUp]
        public void Setup()
        {
            _iFinanveYear = new Mock<IFinanceYearService>();
        }
        [Test]
        public void FillAllFinanceYear()
        {
            _iFinanveYear.Setup(x => x.FillAllFinanceYear()).Returns(new CommonResponse { Exception = null, Data = new FinanceYearDto() });
            var result = _iFinanveYear.Object.FillAllFinanceYear();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillFinanceYearById()
        {
            int id = 1;
            _iFinanveYear.Setup(x => x.FillFinanceYearById(id)).Returns(new CommonResponse { Exception = null, Data = new FinanceYearDto() });
            var result = _iFinanveYear.Object.FillFinanceYearById(id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SaveFinanceYear()
        {
            FinanceYearDto financeYearDto = new FinanceYearDto
            { EndDate = DateTime.Now,
                StartDate=DateTime.Now,
             FinanceYear="2024-25",
             LockTillDate=DateTime.Now,
             Status="R"};
            _iFinanveYear.Setup(x => x.SaveFinanceYear(financeYearDto)).Returns(new CommonResponse { Exception = null, Data = new FinanceYearDto() });
            var result = _iFinanveYear.Object.SaveFinanceYear(financeYearDto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteFinanceYear()
        {
            int id = 1;
            _iFinanveYear.Setup(x => x.DeleteFinanceYear(id)).Returns(new CommonResponse { Exception = null, Data = new FinanceYearDto() });
            var result = _iFinanveYear.Object.DeleteFinanceYear(id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateFinanceYear()
        {
            FinanceYearDto financeYearDto = new FinanceYearDto
            {
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                FinanceYear = "2024-25",
                LockTillDate = DateTime.Now,
                Status = "R"
            };
            _iFinanveYear.Setup(x => x.UpdateFinanceYear(financeYearDto,1)).Returns(new CommonResponse { Exception = null, Data = new FinanceYearDto() });
            var result = _iFinanveYear.Object.UpdateFinanceYear(financeYearDto,1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
