using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Dfinance.NUnitTest.Purchase
{
    public class PurchaseTest
    {
       
        private Mock<IPurchaseService> _purchaseMock;
       
        [SetUp]
        public void Setup()
        {
            _purchaseMock = new Mock<IPurchaseService>();
           
        }
        [Test]
        public void SavePurchaseTest()
        {
            int voucherId = 17;
            int pageId = 15;
            InventoryTransactionDto invTransDto = new InventoryTransactionDto
            {
                Id = null,
                VoucherNo = "0013",
                Date = DateTime.Now,
                References = null,
                Party = new PopUpDto
                {
                    Id = 309,
                },
                Currency = new DropdownDto
                {
                    Id = 1,
                },
                ExchangeRate = 0,
                Project = null,
                Description = "string",
                //Approve = true,
                //CloseVoucher = true,
                GrossAmountEdit = true,
                FiTransactionAdditional = new InvTransactionAdditionalDto
                {
                    TransactionId = 31,
                    Warehouse = new DropdownDto
                    {
                        Id = 62,
                        Value = "string"
                    },
                    PartyInvoiceNo = "string",
                    PartyDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    OrderNo = "string",
                    PartyNameandAddress = "string",
                    ExpiryDate = DateTime.Now,
                    TransPortationType = new DropdownDto
                    {
                        Id = 0,
                        Value = "string"
                    },
                    CreditPeriod = 0,
                    SalesMan = new PopUpDto
                    {
                        Id = 0,
                        Name = "string",
                        Code = "string",
                        Description = "string"
                    },
                    SalesArea = new DropdownDto
                    {
                        Id = 36,
                        Value = "string"
                    },
                    StaffIncentives = 0,
                    MobileNo = "string",
                    VehicleNo = new PopUpDto
                    {
                        Id = 0,
                        Name = "string",
                        Code = "string",
                        Description = "string"
                    },
                    Attention = "string",
                    DespatchNo = "string",
                    DespatchDate = DateTime.Now,
                    DeliveryDate = DateTime.Now,
                    DeliveryNote = "string",
                    PartyName = "string",
                    AddressLine1 = "string",
                    AddressLine2 = "string",
                    DelivaryLocation = new PopUpDto
                    {
                        Id = 0,
                        Name = "string",
                        Code = "string",
                        Description = "string"
                    },
                    TermsOfDelivery = "string",
                    PayType = new DropdownDto
                    {
                        Id = 1,
                        Value = "string"
                    },
                    Approve = true,
                    CloseVoucher = true
                },
                Items = new List<InvTransItemDto>
    {
        new InvTransItemDto
        {
            TransactionId = 0,           
            ItemId = 5, // Assuming ItemId as 5 for example
            ItemCode = "string", // Change this according to your data
            ItemName = "string", // Change this according to your data
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
                TransactionEntries = new InvTransactionEntriesDto
                {
                   // Terms = "string",
                    TotalDisc = 0,
                    Amt = 0,
                    Roundoff = 0,
                    NetAmount = 0,
                    GrandTotal = 1010,
                    PayType = new DropdownDto
                    {
                        Id = 1,
                        Value = "string"
                    },
                    DueDate = DateTime.Now,
                    TotalPaid = 1010,
                    Balance = 0,
                    Tax = new List<InvAccountDetailsDto>
        {
            new InvAccountDetailsDto
            {
                //AccountId = 1381,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 1381
                },
                //AccountName = "string",
                Description = "taxdes",
                Amount = 30,
                //TransType = "string",
                PayableAccount = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = null
                }
            }
        },
                    AddCharges = new List<InvAccountDetailsDto>
        {
            new InvAccountDetailsDto
            {
               // AccountId = 1047,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 1047
                },
               // AccountName = "string",
                Description = "saddc",
                Amount = 10,
                //TransType = "string",
                PayableAccount = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = null
                }
            }
        },
                    Cash = new List<InvAccountDetailsDto>
        {
            new InvAccountDetailsDto
            {
               // AccountId = 61,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 61
                },
                ///AccountName = "string",
                Description = "string",
                Amount = 1010,
               // TransType = "string",
                PayableAccount = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                }
            }
        }
                }
            };

            _purchaseMock.Setup(x => x.SavePurchase(invTransDto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseMock.Object.SavePurchase(invTransDto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void UpdatePurchaseTest()
        {
            int voucherId = 17;
            int pageId = 15;
            InventoryTransactionDto invTransDto = new InventoryTransactionDto
            {
                Id = null,
                VoucherNo = "0013",
                Date = DateTime.Now,
                References = null,
                Party = new PopUpDto
                {
                    Id = 309,
                },
                Currency = new DropdownDto
                {
                    Id = 1,
                },
                ExchangeRate = 0,
                Project = null,
                Description = "string",
                //Approve = true,
                //CloseVoucher = true,
                GrossAmountEdit = true,
                FiTransactionAdditional = new InvTransactionAdditionalDto
                {
                    TransactionId = 31,
                    Warehouse = new DropdownDto
                    {
                        Id = 62,
                        Value = "string"
                    },
                    PartyInvoiceNo = "string",
                    PartyDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    OrderNo = "string",
                    PartyNameandAddress = "string",
                    ExpiryDate = DateTime.Now,
                    TransPortationType = new DropdownDto
                    {
                        Id = 0,
                        Value = "string"
                    },
                    CreditPeriod = 0,
                    SalesMan = new PopUpDto
                    {
                        Id = 0,
                        Name = "string",
                        Code = "string",
                        Description = "string"
                    },
                    SalesArea = new DropdownDto
                    {
                        Id = 36,
                        Value = "string"
                    },
                    StaffIncentives = 0,
                    MobileNo = "string",
                    VehicleNo = new PopUpDto
                    {
                        Id = 0,
                        Name = "string",
                        Code = "string",
                        Description = "string"
                    },
                    Attention = "string",
                    DespatchNo = "string",
                    DespatchDate = DateTime.Now,
                    DeliveryDate = DateTime.Now,
                    DeliveryNote = "string",
                    PartyName = "string",
                    AddressLine1 = "string",
                    AddressLine2 = "string",
                    DelivaryLocation = new PopUpDto
                    {
                        Id = 0,
                        Name = "string",
                        Code = "string",
                        Description = "string"
                    },
                    TermsOfDelivery = "string",
                    PayType = new DropdownDto
                    {
                        Id = 1,
                        Value = "string"
                    },
                    Approve = true,
                    CloseVoucher = true
                },
                Items = new List<InvTransItemDto>
    {
        new InvTransItemDto
        {
            TransactionId = 0,          

            ItemId = 5, // Assuming ItemId as 5 for example
            ItemCode = "string", // Change this according to your data
            ItemName = "string", // Change this according to your data
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
                TransactionEntries = new InvTransactionEntriesDto
                {
                  //  Terms = "string",
                    TotalDisc = 0,
                    Amt = 0,
                    Roundoff = 0,
                    NetAmount = 0,
                    GrandTotal = 1010,
                    PayType = new DropdownDto
                    {
                        Id = 1,
                        Value = "string"
                    },
                    DueDate = DateTime.Now,
                    TotalPaid = 1010,
                    Balance = 0,
                    Tax = new List<InvAccountDetailsDto>
        {
            new InvAccountDetailsDto
            {
                //AccountId = 1381,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 1381
                },
               // AccountName = "string",
                Description = "taxdes",
                Amount = 30,
               // TransType = "string",
                PayableAccount = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = null
                }
            }
        },
                    AddCharges = new List<InvAccountDetailsDto>
        {
            new InvAccountDetailsDto
            {
                //AccountId = 1047,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 1047
                },
               // AccountName = "string",
                Description = "saddc",
                Amount = 10,
               // TransType = "string",
                PayableAccount = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = null
                }
            }
        },
                    Cash = new List<InvAccountDetailsDto>
        {
            new InvAccountDetailsDto
            {
               // AccountId = 61,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 61
                },
               // AccountName = "string",
                Description = "string",
                Amount = 1010,
               // TransType = "string",
                PayableAccount = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                }
            }
        }
                }
            };

            _purchaseMock.Setup(x => x.UpdatePurchase(invTransDto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseMock.Object.UpdatePurchase(invTransDto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void DeletePurchaseTest()
        {
            int transId = 31;
            int pageId = 231;
            _purchaseMock.Setup(x => x.DeletePurchase(transId, pageId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseMock.Object.DeletePurchase(transId, pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }

        [Test]
        public void GetDataTest()
        {
            // Arrange
            int pageId = 15;
            int voucherId = 17;
            _purchaseMock.Setup(x => x.GetData(pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseMock.Object.GetData(pageId, voucherId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillTransItemsTest()
        {
            int partyId = 304, pageId = 15, locId = 62, voucherId = 17;
            _purchaseMock.Setup(x => x.FillTransItems(partyId,pageId,locId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new TransItemsView() });
            var result = _purchaseMock.Object.FillTransItems(partyId, pageId, locId, voucherId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillPurchase()
        {
            int pageId = 15;
            bool post = true;
            _purchaseMock.Setup(x => x.FillPurchase( pageId,post)).Returns(new CommonResponse { Exception = null, Data = new Fillvoucherview() });
            var result = _purchaseMock.Object.FillPurchase(pageId, post);
        }

        [Test]
        public void FillPurchasebyIdTest()
        {
            int TransId = 10;
            int PageId = 15;
            _purchaseMock.Setup(x => x.FillPurchaseById(TransId, PageId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseMock.Object.FillPurchaseById(TransId, PageId);
        }
    }
}