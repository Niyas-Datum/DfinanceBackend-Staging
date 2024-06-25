﻿using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using Dfinance.WareHouse.Services.Interface;
using Moq;

namespace Dfinance.NUnitTest.Purchase
{
    public class MaterialRqTest
    {
        private Mock<IMaterialRequestService> _materialReq;

        [SetUp]
        public void Setup()
        {
            _materialReq = new Mock<IMaterialRequestService>();
        }
        [Test]
        public void GetDataTest()
        {
            // Arrange
            int pageId = 516;
            int voucherId = 156;
            _materialReq.Setup(x => x.GetData(pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _materialReq.Object.GetData(pageId, voucherId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillTransItemsTest()
        {
            int partyId = 304, pageId = 516, locId = 62, voucherId = 156;
            _materialReq.Setup(x => x.FillTransItems(partyId, pageId, locId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new TransItemsView() });
            var result = _materialReq.Object.FillTransItems(partyId, pageId, locId, voucherId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillFromWHTest()
        {
            int branchId = 1;
            _materialReq.Setup(x => x.FillFromWarehouse(branchId)).Returns(new CommonResponse { Exception = null, Data = new DropdownDto() });
            var result = _materialReq.Object.FillFromWarehouse(branchId);
        }
        [Test]
        public void FillMaster()
        {
            int pageId = 516;
            int transId = 500;
            int voucherId = 156;
            _materialReq.Setup(x => x.FillMaster(pageId, transId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new Fillvoucherview() });
            var result = _materialReq.Object.FillMaster(pageId, transId, voucherId);
        }
        [Test]
        public void FillByIdTest()
        {
            int TransId = 10;
            int PageId = 516;
            _materialReq.Setup(x => x.FillById(TransId, PageId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _materialReq.Object.FillById(TransId, PageId);
        }
        [Test]
        public void SaveMatReqTest()
        {
            int voucherId = 156;
            int pageId = 516;

            var materialDto = new MaterialTransferDto
            {
                TransactionId = 10,
                VoucherNo = "0004",
                VoucherDate = DateTime.Now,
                Description = "desc",
                BranchAccount = new PopUpDto
                {
                    Id = 1515,
                    Name = "",
                    Code = "",
                    Description = ""
                },
                Account = new PopUpDto
                {
                    Id = 1515,
                    Name = "",
                    Code = "",
                    Description = ""
                },
                Reference = null,
                materialTransAddDto = new MaterialTransAddDto
                {
                    Terms = "terms",
                    MainBranch = new DropdownDto
                    {
                        Id = 1,
                        Value = null
                    },
                    SubBranch = new DropdownDto
                    {
                        Id = 1,
                        Value = null
                    },
                    FromWarehouse = new DropdownDto
                    {
                        Id = 62,
                        Value = "string"
                    },
                    ToWarehouse = new DropdownDto
                    {
                        Id = 62,
                        Value = "string"
                    }
                },
                Items = new List<InvTransItemDto>
                {
                    new InvTransItemDto
                    {
                        TransactionId = 0,
                        ItemId = 5, // Example ItemId
                        ItemCode = "string",
                        ItemName = "string",
                        BatchNo = "string",
                        Unit = new UnitPopupDto
                        {
                            Unit = "No",
                            BasicUnit = "string",
                            Factor = 0
                        },
                        Qty = 10,
                        FocQty = 0,
                        BasicQty = 0,
                        Additional = 0,
                        Rate = 100,
                        OtherRate = 0,
                        Margin = 0,
                        RateDisc = 0,
                        GrossAmt = 0,
                        Discount = 0,
                        DiscountPerc = 0,
                        Amount = 0,
                        TaxValue = 0,
                        TaxPerc = 0,
                        PrintedMRP = 0,
                        PtsRate = 0,
                        PtrRate = 0,
                        Pcs = 0,
                        StockItemId = 0,
                        Total = 1000,
                        ExpiryDate = DateTime.Now,
                        Description = "string",
                        LengthFt = 0,
                        LengthIn = 0,
                        LengthCm = 0,
                        GirthFt = 0,
                        GirthIn = 0,
                        GirthCm = 0,
                        ThicknessFt = 0,
                        ThicknessIn = 0,
                        ThicknessCm = 0,
                        Remarks = "string",
                        TaxTypeId = 0,
                        TaxAccountId = 0,
                        CostAccountId = 0,
                        BrandId = 0,
                        Profit = 0,
                        RepairsRequired = "string",
                        FinishDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        ReplaceQty = 0,
                        PrintedRate = 0,
                        Hsn = "string",
                        AvgCost = 0,
                        IsReturn = true,
                        ManufactureDate = DateTime.Now,
                        PriceCategory = new PopUpDto
            {
                Id=0,
                Name="",
                Code="",
                Description=""
            },
                        UniqueItems = new List<InvUniqueItemDto>
                        {
                            new InvUniqueItemDto
                            {
                                UniqueNumber = "string"
                            }
                        }
                    }
                },
                References = new List<ReferenceDto>
                {
                    new ReferenceDto
                    {
                        Sel=true,
                        AddItem=true,
                        VoucherId=156,
                        VNo="0001",
                        VDate=DateTime.Now,
                        ReferenceNo="0001",
                        AccountId=1359,
                        AccountName="nithya",
                        Amount=500,
                        PartyInvNo="0001",
                        PartyInvDate=DateTime.Now,
                        Id=200,
                        VoucherType="Card",
                        MobileNo="12345"
                    }
                }
            };
            _materialReq.Setup(x => x.SaveMaterialReq(materialDto, voucherId, pageId)).Returns(new CommonResponse { Exception = null, Data = new MaterialTransferDto() });
            var result = _materialReq.Object.SaveMaterialReq(materialDto, voucherId, pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void UpdateMatReqTest()
        {
            int voucherId = 156;
            int pageId = 516;

            var materialDto = new MaterialTransferDto
            {
                TransactionId = 10,
                VoucherNo = "0004",
                VoucherDate = DateTime.Now,
                Description = "desc",
                BranchAccount=new PopUpDto
                {
                    Id=1515,
                    Name="",
                    Code="",
                    Description=""
                },
                Account=new PopUpDto
                {
                    Id = 1515,
                    Name = "",
                    Code = "",
                    Description = ""
                },
                Reference=null,
                materialTransAddDto = new MaterialTransAddDto
                {
                    Terms = "terms",                   
                    MainBranch = new DropdownDto
                    {
                        Id = 1,
                        Value = null
                    },
                    SubBranch = new DropdownDto
                    {
                        Id = 1,
                        Value = null
                    },
                    FromWarehouse = new DropdownDto
                    {
                        Id = 62,
                        Value = "string"
                    },
                    ToWarehouse = new DropdownDto
                    {
                        Id = 62,
                        Value = "string"
                    }  
                },
                Items = new List<InvTransItemDto>
                {
                    new InvTransItemDto
                    {
                        TransactionId = 0,
                        ItemId = 5, // Example ItemId
                        ItemCode = "string",
                        ItemName = "string",
                        BatchNo = "string",
                        Unit = new UnitPopupDto
                        {
                            Unit = "No",
                            BasicUnit = "string",
                            Factor = 0
                        },
                        Qty = 10,
                        FocQty = 0,
                        BasicQty = 0,
                        Additional = 0,
                        Rate = 100,
                        OtherRate = 0,
                        Margin = 0,
                        RateDisc = 0,
                        GrossAmt = 0,
                        Discount = 0,
                        DiscountPerc = 0,
                        Amount = 0,
                        TaxValue = 0,
                        TaxPerc = 0,
                        PrintedMRP = 0,
                        PtsRate = 0,
                        PtrRate = 0,
                        Pcs = 0,
                        StockItemId = 0,
                        Total = 1000,
                        ExpiryDate = DateTime.Now,
                        Description = "string",
                        LengthFt = 0,
                        LengthIn = 0,
                        LengthCm = 0,
                        GirthFt = 0,
                        GirthIn = 0,
                        GirthCm = 0,
                        ThicknessFt = 0,
                        ThicknessIn = 0,
                        ThicknessCm = 0,
                        Remarks = "string",
                        TaxTypeId = 0,
                        TaxAccountId = 0,
                        CostAccountId = 0,
                        BrandId = 0,
                        Profit = 0,
                        RepairsRequired = "string",
                        FinishDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        ReplaceQty = 0,
                        PrintedRate = 0,
                        Hsn = "string",
                        AvgCost = 0,
                        IsReturn = true,
                        ManufactureDate = DateTime.Now,
                       PriceCategory = new PopUpDto
            {
                Id=0,
                Name="",
                Code="",
                Description=""
            },
                        UniqueItems = new List<InvUniqueItemDto>
                        {
                            new InvUniqueItemDto
                            {
                                UniqueNumber = "string"
                            }
                        }
                    }
                },
                References = new List<ReferenceDto>
                {
                    new ReferenceDto
                    {
                        Sel=true,
                        AddItem=true,
                        VoucherId=156,
                        VNo="0001",
                        VDate=DateTime.Now,
                        ReferenceNo="0001",
                        AccountId=1359,
                        AccountName="nithya",
                        Amount=500,
                        PartyInvNo="0001",
                        PartyInvDate=DateTime.Now,
                        Id=200,
                        VoucherType="Card",
                        MobileNo="12345"
                    }
                }
            };
            _materialReq.Setup(x => x.UpdateMaterialReq(materialDto, voucherId, pageId)).Returns(new CommonResponse { Exception = null, Data = new MaterialTransferDto() });
            var result = _materialReq.Object.UpdateMaterialReq(materialDto, voucherId, pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteMaterialReq()
        {
            int transId = 31;
            int pageId = 231;
            _materialReq.Setup(x => x.DeleteMaterialReq(transId, pageId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _materialReq.Object.DeleteMaterialReq(transId, pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
       
        [Test]
        public void FindQtyTest()
        {
            int itemId = 6;
            int locId = 62;
            int qty = 10;
            int? transId = 10;
            _materialReq.Setup(x => x.FindQuantity(itemId, locId, qty, transId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _materialReq.Object.FindQuantity(itemId, locId, qty, transId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void GetLatestVoucherNoTest()
        {
            int branchId = 1;
            int userId = 118;
            _materialReq.Setup(x => x.GetLatestVoucherDate()).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _materialReq.Object.GetLatestVoucherDate();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}