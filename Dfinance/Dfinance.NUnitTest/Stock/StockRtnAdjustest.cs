using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using Dfinance.WareHouse.Services;
using Moq;

namespace Dfinance.NUnitTest.Stock
{
    public class StockRtnAdjustest
    {
        private Mock<IStockRtnAdjustService> _StockRtnAdjust;
        [SetUp]
        public void Setup()
        {
            _StockRtnAdjust = new Mock<IStockRtnAdjustService>();
        }
        [Test]
        public void SaveStockRtnAdjustTest()
        {
            int pageId = 237;
            int voucherId = 90;
            var dto = new StockRtnAdjustDto
            {
                Id = 0,
                VoucherNo = "0005",
                Warehouse = new DropdownDto { Id = 1 }, // Assuming Name is needed for DropdownDto
                Date = DateTime.Now,
                Currency = new DropdownDto { Id = 1 }, // Assuming Name is needed for DropdownDto
                ExchangeRate = 1.25m,
                Terms = "", // Empty string is acceptable, but ensure this matches expected format
                References = new List<ReferenceStockDto>
                {
                    new ReferenceStockDto
                    {
                        Sel=false,
                        AddItem=false,
                        VoucherId=90,
                        VNo="001",
                        VDate=DateTime.Now,
                        ReferenceNo="a",
                        AccountId=0,
                        AccountName="s",
                        Amount=0,
                        PartyInvDate=DateTime.Now,
                        PartyInvNo="a",
                        Id=0,
                        VoucherType="a",
                        MobileNo="a",
                    }
                },
                Description ="",
                Items = new List<InvTransItemDto>
    {
        new InvTransItemDto
        {
            TransactionId = 1,
            ItemId = 5, // Example ItemId
            ItemCode = "ITEM001", // Example ItemCode
            ItemName = "Sample Item", // Example ItemName
            BatchNo = "BATCH001",
            Unit = new UnitPopupDto
            {
                Unit = "No",
                BasicUnit = "Unit",
                Factor = 1
            },
            Qty = 10,
            FocQty = 0,
            BasicQty = 10, // Adjust BasicQty if necessary
            Additional = 0,
            Rate = 100,
            OtherRate = 0,
            Margin = 0,
            RateDisc = 0,
            GrossAmt = 1000, // Should match Qty * Rate
            Discount = 0,
            DiscountPerc = 0,
            Amount = 1000, // Should match GrossAmt - Discount
            TaxValue = 0,
            TaxPerc = 0,
            PrintedMRP = 0,
            PtsRate = 0,
            PtrRate = 0,
            Pcs = 0,
            StockItemId = 0,
            Total = 1000, // Should match Amount
            ExpiryDate = DateTime.Now.AddMonths(6), // Example future expiry date
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
            FinishDate = DateTime.Now.AddMonths(1), // Example future finish date
            UpdateDate = DateTime.Now,
            ReplaceQty = 0,
            PrintedRate = 0,
            Hsn = "HSN123",
            AvgCost = 0,
            IsReturn = false, // Example value for IsReturn
            ManufactureDate = DateTime.Now.AddYears(-1), // Example past manufacture date
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
            },
            TempRate=0
        }
    }
            };
            _StockRtnAdjust.Setup(s => s.SaveStockRtnAdjust(dto, voucherId, pageId)).Returns(new CommonResponse { Exception = null, Data = new StockRtnAdjustDto() });
            var result = _StockRtnAdjust.Object.SaveStockRtnAdjust(dto, voucherId, pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
        [Test]
        public void UpdateStockRtnAdjustTest()
        {
            int pageId = 135;
            int voucherId = 38;
            var dto = new StockRtnAdjustDto
            {
                Id = 1,
                VoucherNo = "V123",
                Warehouse = new DropdownDto { Id = 1 }, // Assuming Name is needed for DropdownDto
                Date = DateTime.Now,
                Currency = new DropdownDto { Id = 1 }, // Assuming Name is needed for DropdownDto
                ExchangeRate = 1.25m,
                Terms = "", // Empty string is acceptable, but ensure this matches expected format

                Items = new List<InvTransItemDto>
{
    new InvTransItemDto
    {
        TransactionId = 0,
        ItemId = 5, // Example ItemId
        ItemCode = "ITEM001", // Example ItemCode
        ItemName = "Sample Item", // Example ItemName
        BatchNo = "BATCH001",
        Unit = new UnitPopupDto
        {
            Unit = "No",
            BasicUnit = "Unit",
    Factor = 1
},
Qty = 10,
FocQty = 0,
BasicQty = 10, // Adjust BasicQty if necessary
Additional = 0,
Rate = 100,
OtherRate = 0,
Margin = 0,
RateDisc = 0,
GrossAmt = 1000, // Should match Qty * Rate
Discount = 0,
DiscountPerc = 0,
Amount = 1000, // Should match GrossAmt - Discount
TaxValue = 0,
TaxPerc = 0,
PrintedMRP = 0,
PtsRate = 0,
PtrRate = 0,
Pcs = 0,
StockItemId = 0,
Total = 1000, // Should match Amount
ExpiryDate = DateTime.Now.AddMonths(6), // Example future expiry date
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
FinishDate = DateTime.Now.AddMonths(1), // Example future finish date
UpdateDate = DateTime.Now,
ReplaceQty = 0,
PrintedRate = 0,
Hsn = "HSN123",
AvgCost = 0,
IsReturn = false, // Example value for IsReturn
ManufactureDate = DateTime.Now.AddYears(-1), // Example past manufacture date
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
},
  
            };
            _StockRtnAdjust.Setup(s => s.UpdateStockRtnAdjust(dto, voucherId, pageId)).Returns(new CommonResponse { Exception = null, Data = new StockRtnAdjustDto() });
            var result = _StockRtnAdjust.Object.UpdateStockRtnAdjust(dto, voucherId, pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
[Test]
    public void DeleteStockTransactionTest()
    {
        int transId = 31;
        int pageId = 231;
        _StockRtnAdjust.Setup(x => x.DeleteTransactions(transId, pageId,"Deleted")).Returns(new CommonResponse { Exception = null, Data = new StockRtnAdjustDto() });
        var result = _StockRtnAdjust.Object.DeleteTransactions(transId, pageId, "Deleted");
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Data, Is.Not.Null);

    }
        [Test]
        public void CancelStockTransactionTest()
        {
            int transId = 31;
            int pageId = 231;
            _StockRtnAdjust.Setup(x => x.CancelTransaction(transId, pageId, "Deleted")).Returns(new CommonResponse { Exception = null, Data = new StockRtnAdjustDto() });
            var result = _StockRtnAdjust.Object.CancelTransaction(transId, pageId, "Deleted");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
    }

    


}
