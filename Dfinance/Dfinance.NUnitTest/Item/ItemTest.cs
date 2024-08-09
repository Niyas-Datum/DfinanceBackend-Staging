using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Views.Inventory;
using Dfinance.Core.Views.Item;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Item.Services.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Dfinance.Shared.Routes.v1.FinRoute;


namespace Dfinance.NUnitTest.Item
{
    [TestFixture]
    public class ItemTest
    {
        private Mock<IItemMasterService> _itemServiceMock;

        [SetUp]
        public void Setup()
        {
            _itemServiceMock = new Mock<IItemMasterService>();

        }


        [Test]
        public void FillItemMasterTest()
        {
            //arrange
            int[]? catId = { 54 };
            int[]? brandId = { 1060 };
            string search = null;
            int pageNo = 1;
            int limit = 10;
            int pageId = 55;
            // Act
            _itemServiceMock.Setup(x => x.FillItemMaster(catId, brandId, search, pageNo, limit)).Returns(new CommonResponse { Exception = null, Data = new ItemMaster() });
            var result = _itemServiceMock.Object.FillItemMaster(catId, brandId, search, pageNo, limit);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillByIDTest()
        {
            int itemId = 9;
            int branchId = 1;
            int pageId = 55;
            _itemServiceMock.Setup(x => x.FillItemByID(pageId, itemId, branchId)).Returns(new CommonResponse { Exception = null, Data = new ItemMaster() });
            var result = _itemServiceMock.Object.FillItemByID(pageId, itemId, branchId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillTransItemsTest()
        {

            _itemServiceMock.Setup(x => x.FillTransItems(null, null, null, null, null, null, false, false, false, false, false, false, null, null, null)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionsDto() });
            var result = _itemServiceMock.Object.FillTransItems(null, null, null, null, null, null, false, false, false, false, false, false, null, null, null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }



        [Test]
        public void SaveItemTest()
        {
            // Arrange
            int pageId = 55;
            var newItem = new ItemMasterDto
            {
                ItemCode = "ABC123",
                ItemName = "Test Item",
                ArabicName = "Arabic Test Item",
                Unit = new UnitPopupDto
                {
                    Unit = "No",
                    BasicUnit = "No"
                },
                BarCode = "123456789",
                Category = new CatPopupDto
                {
                    ID = 54
                },
                IsUniqueItem = true,
                StockItem = true,
                CostPrice = 10.0m,
                SellingPrice = 15.0m,
                MRP = 20.0m,
                Margin = 5.0m,
                MarginValue = 2.0m,
                TaxType = new DropDownDtoName
                {
                    Id = 13
                },
                IsExpiry = true,
                ExpiryPeriod = 30,
                IsFinishedGood = true,
                IsRawMaterial = false,
                Location = "Location Name",
                ItemDisc = 2.0m,
                HSN = "HSN Code",
                Parent = new ParentItemPopupDto
                {
                    ID = 5
                },
                ModelNo = "Model Number",
                ROL = 5.0m,
                PartNo = "Part Number",
                ROQ = 10.0m,
                Manufacturer = "Manufacturer Name",
                Weight = 1.5m,
                ShipMark = "Ship Mark",
                PaintMark = "Paint Mark",
                SellingUnit = new UnitPopupDto
                {
                    Unit = "No"
                },
                OEMNo = "OEM Number",
                PurchaseUnit = new UnitPopupDto
                {
                    Unit = "No"
                },
                IsGroup = true,
                Active = true,
                InvAccount = new DropDownDtoName
                {
                    Id = 1
                },
                SalesAccount = new DropDownDtoName
                {
                    Id = 1
                },
                CostAccount = new DropDownDtoName
                {
                    Id = 1
                },
                PurchaseAccount = new DropDownDtoName
                {
                    Id = 1
                },
                Remarks = "Remarks for the item",
                ItemUnit = new List<ItemUnitsDto>
    {
        new ItemUnitsDto
        {
            UnitID = 1,
            Unit = new UnitPopupDto
            {
                Unit = "No"
            },
            BasicUnit = "No",
            Factor = 1.0m,
            PurchaseRate = 10.0m,
            SellingPrice = 15.0m,
            MRP = 20.0m,
            WholeSalePrice = 18.0m,
            RetailPrice = 22.0m,
            WholeSalePrice2 = 16.0m,
            RetailPrice2 = 25.0m,
            LowestRate = 8.0m,
            BarCode = "123456789",
            Active = true,
            Status = 1
        }
    },
                Branch = new List<DropDownDtoName>
    {
        new DropDownDtoName { Id = 1 },
        new DropDownDtoName { Id = 2 }
    },
                ImageFile = "imageFileName.jpg"
            };


            // Act              
            _itemServiceMock.Setup(x => x.SaveItemMaster(newItem, pageId)).Returns(new CommonResponse { Exception = null, Data = new ItemMaster() });
            var result = _itemServiceMock.Object.SaveItemMaster(newItem, pageId);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void UpdateItemTest()
        {
            // Arrange
            int itemId = 5;
            List<int> branch = new List<int>() { 1, 2 };

            var newItem = new ItemMasterDto
            {
                ItemCode = "ABC123",
                ItemName = "Test Item",
                ArabicName = "Arabic Test Item",
                Unit = new UnitPopupDto
                {
                    Unit = "No",
                    BasicUnit = "No"
                },
                BarCode = "123456789",
                Category = new CatPopupDto
                {
                    ID = 54
                },
                IsUniqueItem = true,
                StockItem = true,
                CostPrice = 10.0m,
                SellingPrice = 15.0m,
                MRP = 20.0m,
                Margin = 5.0m,
                MarginValue = 2.0m,
                TaxType = new DropDownDtoName
                {
                    Id = 13
                },
                IsExpiry = true,
                ExpiryPeriod = 30,
                IsFinishedGood = true,
                IsRawMaterial = false,
                Location = "Location Name",
                ItemDisc = 2.0m,
                HSN = "HSN Code",
                Parent = new ParentItemPopupDto
                {
                    ID = 5
                },
                ModelNo = "Model Number",
                ROL = 5.0m,
                PartNo = "Part Number",
                ROQ = 10.0m,
                Manufacturer = "Manufacturer Name",
                Weight = 1.5m,
                ShipMark = "Ship Mark",
                PaintMark = "Paint Mark",
                SellingUnit = new UnitPopupDto
                {
                    Unit = "No"
                },
                OEMNo = "OEM Number",
                PurchaseUnit = new UnitPopupDto
                {
                    Unit = "No"
                },
                IsGroup = true,
                Active = true,
                InvAccount = new DropDownDtoName
                {
                    Id = 1
                },
                SalesAccount = new DropDownDtoName
                {
                    Id = 1
                },
                CostAccount = new DropDownDtoName
                {
                    Id = 1
                },
                PurchaseAccount = new DropDownDtoName
                {
                    Id = 1
                },
                Remarks = "Remarks for the item",
                ItemUnit = new List<ItemUnitsDto>
    {
        new ItemUnitsDto
        {
            UnitID = 1,
            Unit = new UnitPopupDto
            {
                Unit = "No"
            },
            BasicUnit = "No",
            Factor = 1.0m,
            PurchaseRate = 10.0m,
            SellingPrice = 15.0m,
            MRP = 20.0m,
            WholeSalePrice = 18.0m,
            RetailPrice = 22.0m,
            WholeSalePrice2 = 16.0m,
            RetailPrice2 = 25.0m,
            LowestRate = 8.0m,
            BarCode = "123456789",
            Active = true,
            Status = 1
        }
    },
                Branch = new List<DropDownDtoName>
    {
        new DropDownDtoName { Id = 1 },
        new DropDownDtoName { Id = 2 }
    },
                ImageFile = "imageFileName.jpg"
            };
            int pageId = 55;

            _itemServiceMock.Setup(x => x.UpdateItemMaster(newItem, itemId, pageId)).Returns(new CommonResponse { Exception = null, Data = new ItemMaster() });
            var result = _itemServiceMock.Object.UpdateItemMaster(newItem, itemId, pageId);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }


        [Test]
        public void DeleteItemTest()
        {
            // Arrange
            int itemId = 0;
            int pageId = 55;
            _itemServiceMock.Setup(x => x.DeleteItem(itemId, pageId)).Returns(new CommonResponse { Exception = null, Data = new ItemMaster() });

            // Act
            var result = _itemServiceMock.Object.DeleteItem(itemId, pageId);

            // Assert 
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void GenCodeTest()
        {
            _itemServiceMock.Setup(x => x.GetNextItemCode()).Returns(new CommonResponse { Exception = null, Data = new ItemNextCode() });

            // Act
            var result = _itemServiceMock.Object.GetNextItemCode();

            // Assert 
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void GenBarCodeTest()
        {
            _itemServiceMock.Setup(x => x.GenerateBarCode()).Returns(new CommonResponse { Exception = null, Data = new BarcodeView() });

            // Act
            var result = _itemServiceMock.Object.GenerateBarCode();

            // Assert 
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void ParentPopTest()
        {
            _itemServiceMock.Setup(x => x.ParentItemPopup()).Returns(new CommonResponse { Exception = null, Data = new ParentItemPoupView() });

            // Act
            var result = _itemServiceMock.Object.ParentItemPopup();

            // Assert 
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }

}