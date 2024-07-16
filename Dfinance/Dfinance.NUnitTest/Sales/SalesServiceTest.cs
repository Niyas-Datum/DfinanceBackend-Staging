using Dfinance.Application;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.Sales
{
    public class SalesServiceTest
    {
        private Mock<ISalesInvoiceService> _salesInvoice;
        [SetUp]
        public void Setup()
        {
            _salesInvoice = new Mock<ISalesInvoiceService>();
        }
        [Test]
        public void SalesInvoice_Save()
        {
            InventoryTransactionDto salesDto = new InventoryTransactionDto
            {
                Id = null,
                VoucherNo = "0007",
                Date = DateTime.Now,
                References = null,
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
               // Approve = true,
               // CloseVoucher = true,
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
                    ID = 0
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
               // AccountId = 1047,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
               // AccountName = "string",
                Description = "saddc",
                Amount = 10,
              //  TransType = "string",
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
                    ID = 0
                },
              //  AccountName = "string",
                Description = "string",
                Amount = 1010,
              //  TransType = "string",
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

            _salesInvoice.Setup(x => x.SaveSales(salesDto, 149, 23)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });

            //Act
            var result = _salesInvoice.Object.SaveSales(salesDto, 149, 23);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SalesInvoice_Update()
        {
            InventoryTransactionDto salesDto = new InventoryTransactionDto
            {
                Id = 16,
                VoucherNo = "0007",
                Date = DateTime.Now,
                References = null,
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
               // Approve = true,
               // CloseVoucher = true,
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
                    ID = 0
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
              //  AccountId = 1047,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
              //  AccountName = "string",
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
             //   AccountId = 61,
                AccountCode = new AccountNamePopUpDto
                {
                    Alias = "string",
                    Name = "string",
                    ID = 0
                },
              //  AccountName = "string",
                Description = "string",
                Amount = 1010,
             //   TransType = "string",
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

            _salesInvoice.Setup(x => x.UpdateSales(salesDto, 149, 23)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });

            //Act
            var result = _salesInvoice.Object.UpdateSales(salesDto, 149, 23);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteSalesTest()
        {
            int transId = 16;
            _salesInvoice.Setup(x => x.DeleteSales(transId,149)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _salesInvoice.Object.DeleteSales(transId, 149);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void CancelSalesTest()
        {
            int transId = 16;
            _salesInvoice.Setup(x => x.CancelSales(transId,149,"Canceled")).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _salesInvoice.Object.CancelSales(transId, 149, "Canceled");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillSaleByIDTest()
        {
            int transId = 16;
            _salesInvoice.Setup(x => x.FillSalesById(transId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _salesInvoice.Object.FillSalesById(transId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillSalesTest()
        {
            int pageId = 149;
            bool post = true;
            _salesInvoice.Setup(x => x.FillSales(pageId, post)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _salesInvoice.Object.FillSales(pageId, post);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillTransItemsTest()
        {
            int pageId = 149;
            int partyId = 309;
            int logId = 238;
            int voucherId = 23;
            _salesInvoice.Setup(x => x.FillTransItems(partyId, pageId, logId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _salesInvoice.Object.FillTransItems(partyId, pageId, logId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void GetDataTest()

        {
            int pageId = 149;
            int voucherId = 23;
            _salesInvoice.Setup(x => x.GetData(pageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new InventoryTransactionDto() });
            var result = _salesInvoice.Object.GetData(pageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
