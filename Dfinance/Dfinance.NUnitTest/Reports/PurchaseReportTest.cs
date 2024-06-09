using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfinance.NUnitTest.Reports
{
    [TestFixture]
    public class PurchaseReportTest
    {
        private Mock<DFCoreContext> _mockContext;
        private Mock<IAuthService> _mockAuthService;
        private Mock<ILogger<PurchaseService>> _mockLogger;
        private IPurchaseService _purchaseReportService;

       

        private Mock<IPurchaseService> _purchaseMock;

        [SetUp]
        public void Setup()
        {
            _purchaseMock = new Mock<IPurchaseService>();

        }

        [Test]
        public void FillPurchaseReport_ShouldReturnOkResponse_WhenViewByIsFalse()
        {
            // Arrange
            var reportDto = new PurchaseReportDto
            {
                ViewBy = null,
                From = new DateTime(2022, 4, 6),
                To = new DateTime(2024, 4, 6),
                Branch = new DropdownDto { Id = 1 },
                BaseType = new PopUpDto { Id = null },
                VoucherType = new PopUpDto { Id = null },
                customerSupplier = new PopUpDto { Id = null },
                PaymentType = new PopUpDto { Id = null },
                Item = new PopUpDto { Id = null },
                Inventory = false,
                Counter = new PopUpDto { Id = null },
                InvoiceNo = new PopUpDto { Id = null },
                BatchNo = new PopUpDto { Id = null },
                User = new PopUpDto { Id = 118 },
                Area = new PopUpDto { Id = null },
                Staff = new PopUpDto { Id = null }
            };

           //
            var result = _purchaseReportService.GetPurchaseReport(reportDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
