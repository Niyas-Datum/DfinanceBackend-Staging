using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using Dfinance.Stock.Interface;
using Moq;

namespace Dfinance.NUnitTest.Stock
{
    public class PhysicalStockTest
    {
        private Mock<IPhysicalStockService> _phystock;

        [SetUp]
        public void SetUp()
        {
            _phystock=new Mock<IPhysicalStockService>();
        }
        [Test]
        public void SavePhysicalStock()
        {
            // Arrange
            PhysicalStockDto physicalStockDto = new PhysicalStockDto
            {
                Id = 0,
                VoucherNo = "0004",
                Warehouse = new DropdownDto
                {
                    Id = 63,
                    Value = "warehouse1",
                },
                Date = DateTime.Now,
                Currency = new DropdownDto
                {
                    Id = 1,
                    Value = "sar"
                },
                ExchangeRate = 1,
                Terms = "checkterms",
                Items = new List<InvTransItemDto>
            {
                new InvTransItemDto
                {
                    TransactionId = 0,
                    ItemId = 5,
                    ItemCode = "ITEM001",
                    ItemName = "Sample Item",
                    BatchNo = "BATCH001",
                    Unit = new UnitPopupDto
                    {
                        Unit = "No",
                        BasicUnit = "Unit",
                        Factor = 1
                    },
                    Qty = 10,
                    FocQty = 0,
                    BasicQty = 10,
                    Additional = 0,
                    Rate = 100,
                    OtherRate = 0,
                    Margin = 0,
                    RateDisc = 0,
                    GrossAmt = 1000,
                    Discount = 0,
                    DiscountPerc = 0,
                    Amount = 1000,
                    TaxValue = 0,
                    TaxPerc = 0,
                    PrintedMRP = 0,
                    PtsRate = 0,
                    PtrRate = 0,
                    Pcs = 0,
                    StockItemId = 0,
                    Total = 1000,
                    ExpiryDate = DateTime.Now.AddMonths(6),
                    Description = "Sample Description",
                    LengthFt = 0,
                    LengthIn = 0,
                    LengthCm = 0,
                    GirthFt = 0,
                    GirthIn = 0,
                    GirthCm = 0,
                    ThicknessFt = 0,
                    ThicknessIn = 0,
                    ThicknessCm = 0,
                    Remarks = "Sample Remarks",
                    TaxTypeId = 0,
                    TaxAccountId = 0,
                    CostAccountId = 0,
                    BrandId = 0,
                    Profit = 0,
                    RepairsRequired = "No",
                    FinishDate = DateTime.Now.AddMonths(1),
                    UpdateDate = DateTime.Now,
                    ReplaceQty = 0,
                    PrintedRate = 0,
                    Hsn = "HSN123",
                    AvgCost = 0,
                    IsReturn = false,
                    ManufactureDate = DateTime.Now.AddYears(-1),
                    PriceCategory = new PopUpDto
                    {
                        Id = 0,
                        Name = "Standard",
                        Code = "STD",
                        Description = "Standard price category"
                    },
                    UniqueItems = new List<InvUniqueItemDto>
                    {
                        new InvUniqueItemDto
                        {
                            UniqueNumber = "UNIQUE001"
                        }
                    }
                }
            }
            };

            int pageId = 236;
            int voucherId = 16;

            // Setup mock to return a valid response
            _phystock.Setup(x => x.SavePhysicalStock(physicalStockDto, pageId, voucherId))
                     .Returns(new CommonResponse { Exception = null, Data = new PhysicalStockDto() });

            // Act
            var result = _phystock.Object.SavePhysicalStock(physicalStockDto, pageId, voucherId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void Updatephysicalstock()
        {
            PhysicalStockDto physicalStockDto = new PhysicalStockDto
            {
                Id = 6391,
                VoucherNo = "0004",
                Warehouse = new DropdownDto
                {
                    Id = 63,
                    Value = "warehouse1",
                },
                Date = DateTime.Now,
                Currency = new DropdownDto
                {
                    Id = 1,
                    Value = "sar"
                },
                ExchangeRate = 1,
                Terms = "checkterms",
                Items = new List<InvTransItemDto>
                {
                    new InvTransItemDto
                    {
            TransactionId = 0,
            ItemId = 5,
            ItemCode = "ITEM001",
            ItemName = "Sample Item",
            BatchNo = "BATCH001",
            Unit = new UnitPopupDto
            {
                Unit = "No",
                BasicUnit = "Unit",
                Factor = 1
            },
            Qty = 10,
            FocQty = 0,
            BasicQty = 10,
            Additional = 0,
            Rate = 100,
            OtherRate = 0,
            Margin = 0,
            RateDisc = 0,
            GrossAmt = 1000,
            Discount = 0,
            DiscountPerc = 0,
            Amount = 1000,
            TaxValue = 0,
            TaxPerc = 0,
            PrintedMRP = 0,
            PtsRate = 0,
            PtrRate = 0,
            Pcs = 0,
            StockItemId = 0,
            Total = 1000,
            ExpiryDate = DateTime.Now.AddMonths(6),
            Description = "Sample Description",
            LengthFt = 0,
            LengthIn = 0,
            LengthCm = 0,
            GirthFt = 0,
            GirthIn = 0,
            GirthCm = 0,
            ThicknessFt = 0,
            ThicknessIn = 0,
            ThicknessCm = 0,
            Remarks = "Sample Remarks",
            TaxTypeId = 0,
            TaxAccountId = 0,
            CostAccountId = 0,
            BrandId = 0,
            Profit = 0,
            RepairsRequired = "No",
            FinishDate = DateTime.Now.AddMonths(1),
            UpdateDate = DateTime.Now,
            ReplaceQty = 0,
            PrintedRate = 0,
            Hsn = "HSN123",
            AvgCost = 0,
            IsReturn = false,
            ManufactureDate = DateTime.Now.AddYears(-1),
            PriceCategory = new PopUpDto
            {
                Id = 0,
                Name = "Standard",
                Code = "STD",
                Description = "Standard price category"
            },
            UniqueItems = new List<InvUniqueItemDto>
            {
                new InvUniqueItemDto
                {
                    UniqueNumber = "UNIQUE001"
                }
            }
        }
    }

            };
            int pageId = 236;
            int voucherId = 16;
            _phystock.Setup(x => x.UpdatePhysicalStock(physicalStockDto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new PhysicalStockDto() });
            var result = _phystock.Object.UpdatePhysicalStock(physicalStockDto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);


        }
        [Test]
        public void Deletephysicalstock()
        {
            int transId = 6397;
            int pageId = 236;
            string Msg = "del";
            _phystock.Setup(x => x.DeletePhystock(transId, pageId,Msg)).Returns(new CommonResponse { Exception = null, Data = new PhysicalStockDto() });
            var result = _phystock.Object.DeletePhystock(transId, pageId, Msg);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void Cancelphysicalstock()
        {
            int transId = 3164;
            string reason = "cancel";
            _phystock.Setup(x => x.CancelPhysicalStock(transId, reason)).Returns(new CommonResponse { Exception = null, Data = new PhysicalStockDto() });
            var result = _phystock.Object.CancelPhysicalStock(transId, reason);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
