using Dfinance.Core.Domain;
using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using Moq;


namespace Testing
{
    [TestFixture]
    public class ItemTest
    {              
            private Mock<IItemMasterService> _itemServiceMock;

            [SetUp]
            public void Setup()
            {
                _itemServiceMock=new Mock<IItemMasterService>();

            }
            [Test]
            public void FillItemTest()
            {
                // Arrange
                int itemId = 100;
                _itemServiceMock.Setup(x => x.FillItemMaster(itemId)).Returns(new CommonResponse { Exception = null,Data=new SpFillItemMaster() });

                // Act
                var result = _itemServiceMock.Object.FillItemMaster(itemId);

                // Assert 
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsValid, Is.True);
                Assert.That(result.Data,Is.Not.Null);              
            }
           [Test]
            public void SaveItemTest()
            {
                // Arrange
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
                    PurchaseRate = 10.0m,
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
                    Branch = new DropDownDtoName
                    {
                        Id = 1 
                    },
                    ImageFile = "imageFileName.jpg" 
                };

                
                // Act              
               _itemServiceMock.Setup(x => x.SaveItemMaster(newItem)).Returns(new CommonResponse { Exception = null, Data=new ItemMaster() });
                var result = _itemServiceMock.Object.SaveItemMaster(newItem);
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
                    PurchaseRate = 10.0m,
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
                    Branch = new DropDownDtoName
                    {
                        Id = 1
                    },
                    ImageFile = "imageFileName.jpg"
                };
                _itemServiceMock.Setup(x => x.UpdateItemMaster(newItem,itemId)).Returns(new CommonResponse { Exception = null, Data = new ItemMaster() });
                var result = _itemServiceMock.Object.UpdateItemMaster(newItem, itemId);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

          
            [Test] 
            public void DeleteItemTest()
            {
            // Arrange
                 int itemId=0;
                _itemServiceMock.Setup(x => x.DeleteItem(itemId)).Returns(new CommonResponse { Exception = null,Data=new ItemMaster() });

                // Act
                var result = _itemServiceMock.Object.DeleteItem(itemId);

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