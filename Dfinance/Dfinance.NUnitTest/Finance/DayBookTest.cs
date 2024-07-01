using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Statements.Interface;
using Moq;

namespace Dfinance.NUnitTest.Finance
{
    public class DayBookTest
    {
        private Mock<IDayBookService> _dayBook;
        [SetUp]
        public void Setup()
        {
            _dayBook = new Mock<IDayBookService>();
        }
        [Test]
        public void FillVoucherAndUserTest()
        {
            
            _dayBook.Setup(x => x.FillVoucherAndUser()).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new DropDownDtoName() });
            var result = _dayBook.Object.FillVoucherAndUser();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillDayBookTest()
        {
            var dayBookDto = new DayBookDto
            {
                DateFrom = new DateTime(2023, 1, 1),
                DateUpto = new DateTime(2023, 1, 31),
                VoucherType = new DropdownDto { Id = 1, Value = "Sales" },
                Branch = new DropdownDto { Id = 2, Value = "Main Branch" },
                User = new PopUpDto { Id = 1, Description = "JohnDoe" },
                Detailed = true,
                Posted = false
            };

            _dayBook.Setup(x => x.FillDayBook(dayBookDto,76)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new DayBookView() });
            var result = _dayBook.Object.FillDayBook(dayBookDto, 76);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
