using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Purchase.Services.Interface;
using Moq;
using Dfinance.Shared.Domain;

namespace Dfinance.NUnitTest.Purchase
{
    public class PurchaseEnquiryTest
    {
        private Mock<IPurchaseEnquiryService> _purchaseEnquiryMock;
        [SetUp]
        public void Setup()
        {
            _purchaseEnquiryMock = new Mock<IPurchaseEnquiryService>();

        }
        [Test]
        public void SavePurchaseEnqTest()
        {
            int voucherId = 105;
            int pageId = 268;
            InventoryTransactionDto invTransDto = new InventoryTransactionDto
            {
                Id = null,
                VoucherNo = "0041",
                Date = DateTime.Now,
                Reference = null,
                Party = new PopUpDto
                {
                    Id = 1536,
                },
                Currency = new DropdownDto
                {
                    Id = 1,
                },
                ExchangeRate = 0,
                Project = null,
                Description = "string",
                Approve = true,
                CloseVoucher = true,
                GrossAmountEdit = true,
                FiTransactionAdditional = new InvTransactionAdditionalDto
                {
                    TransactionId = 34,
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
            
            ItemId = 5, 
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
            PriceCategory = 0,
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
                    Terms = "string",
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

            _purchaseEnquiryMock.Setup(x => x.SavePurchaseEnquiry(invTransDto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseEnquiryMock.Object.SavePurchaseEnquiry(invTransDto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdatePurchaseEnqTest()
        {
            int voucherId = 18;
            int pageId = 231;
            InventoryTransactionDto invTransDto = new InventoryTransactionDto
            {
                Id = 34,
                VoucherNo = "0040",
                Date = DateTime.Now,
                Reference = null,
                Party = new PopUpDto
                {
                    Id = 1536,
                },
                Currency = new DropdownDto
                {
                    Id = 1,
                },
                ExchangeRate = 0,
                Project = null,
                Description = "string",
                Approve = true,
                CloseVoucher = true,
                GrossAmountEdit = true,
                FiTransactionAdditional = new InvTransactionAdditionalDto
                {
                    TransactionId = 34,
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
           
            ItemId = 5, 
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
            PriceCategory = 0,
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
                    Terms = "string",
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
                    ID = 0
                },
              //  AccountName = "string",
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
              //  AccountId = 1047,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
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
              //  AccountId = 61,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
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

            _purchaseEnquiryMock.Setup(x => x.UpdatePurchaseEnquiry(invTransDto, pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseEnquiryMock.Object.UpdatePurchaseEnquiry(invTransDto, pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void DeletePurchaseEnqTest()
        {
            int transId = 34;
            _purchaseEnquiryMock.Setup(x => x.DeletePurchaseEnq(transId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _purchaseEnquiryMock.Object.DeletePurchaseEnq(transId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
    }
}
