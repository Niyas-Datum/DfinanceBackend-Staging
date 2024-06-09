using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Reports;
using Dfinance.Purchase.Reports.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfinance.NUnitTest.Reports
{
    [TestFixture]
    public class PurchaseReportTest
    {
        private Mock<DFCoreContext> _mockContext;
        private Mock<IAuthService> _mockAuthService;
        private Mock<ILogger<PurchaseReportService>> _mockLogger;
        private IPurchaseReportService _purchaseReportService;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<DFCoreContext>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<PurchaseReportService>>();
            _purchaseReportService = new PurchaseReportService(_mockContext.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Test]
        public void FillPurchaseReport_ShouldReturnOkResponse_WhenViewByIsFalse()
        {
            // Arrange
            var reportDto = new PurchaseReportDto
            {
                ViewBy = false,
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
            var result = _purchaseReportService.FillPurchaseReport(reportDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
