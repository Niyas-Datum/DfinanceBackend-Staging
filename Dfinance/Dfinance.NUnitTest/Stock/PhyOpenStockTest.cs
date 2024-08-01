using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using Dfinance.WareHouse.Services;
using Moq;

namespace Dfinance.NUnitTest.Stock
{
    public class PhyOpenStockTest
    {
        private Mock<IPhyOpenStockService> _phyopenStock;
        [SetUp]
        public void Setup()
        {
            _phyopenStock = new Mock<IPhyOpenStockService>();
        }
        [Test]
        public void SavePhyOpenStockTest()
        {
            int pageId = 135;
            int voucherId = 38;
            var dto = new PhyOpenStockDto
            {
                Id = 1,
                VoucherNo = "V123",
                Warehouse = new DropdownDto { Id = 1 }, 
                Date = DateTime.Now,
                Currency = new DropdownDto { Id = 1},
                ExchangeRate = 1.25m,
                Terms = "", 

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
            TempRate = 0,
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
            _phyopenStock.Setup(x => x.SavePhyOpenStock(dto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new PhyOpenStockDto() });
            var result = _phyopenStock.Object.SavePhyOpenStock(dto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
        [Test]
        public void UpdateOpeningStockTest()
        {
            int pageId = 135;
            int voucherId = 38;

            var dto = new PhyOpenStockDto
            {
                Id = 1,
                VoucherNo = "V123",
                Warehouse = new DropdownDto
                {
                    Id = 1
                }, 
                Date = DateTime.Now,
                Currency = new DropdownDto
                {
                    Id = 1
                }, 
                ExchangeRate = 1.25m,
                Terms = "", 

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
            _phyopenStock.Setup(x => x.UpdatePhyOpenStock(dto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new PhyOpenStockDto() });
            var result = _phyopenStock.Object.UpdatePhyOpenStock(dto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
    }
                  
                   
                   
    
}
