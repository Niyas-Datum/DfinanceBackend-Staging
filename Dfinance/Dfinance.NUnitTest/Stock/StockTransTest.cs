using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto;
using Dfinance.WareHouse.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.NUnitTest.Stock
{
    public class StockTransTest
    {
        private Mock<IStockTransactionService> _stock;
        [SetUp]
        public void Setup()
        {
            _stock = new Mock<IStockTransactionService>();
        }
        [Test]
        public void SaveStockTest()
        {
            var dto = new StockTransactionDto
            {
                Id = 1,
                VoucherNo = "ST123",
                FromBranch = new DropdownDto { Id = 1 },
                ToBranch = new DropdownDto { Id = 2 },
                FromWarehouse = new DropdownDto { Id = 1 },
                ToWarehouse = new DropdownDto { Id = 2 },
                VoucherDate = DateTime.Now,
                Description = "Stock transfer",
                Terms = "Immediate transfer",
                Reference = "",
                StockItems = new List<StockTransItemDto>
            {
                new StockTransItemDto { ItemId = 1, ItemName = "Item 1", Qty = 10, Rate = 25.5M, Amount = 255 }
            },
                references = new List<ReferenceDto>
                {
                    new ReferenceDto { Id = 1, ReferenceNo = "REF-001" }
            }
            };
            _stock.Setup(x => x.SaveStockTrans(dto, 86, 229)).Returns(new CommonResponse { Exception = null, Data = new StockTransactionDto() });
            var result = _stock.Object.SaveStockTrans(dto, 86, 229);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateStockTest()
        {
            var dto = new StockTransactionDto
            {
                Id = 1,
                VoucherNo = "ST123",
                FromBranch = new DropdownDto { Id = 1 },
                ToBranch = new DropdownDto { Id = 2 },
                FromWarehouse = new DropdownDto { Id = 1 },
                ToWarehouse = new DropdownDto { Id = 2 },
                VoucherDate = DateTime.Now,
                Description = "Stock transfer",
                Terms = "Immediate transfer",
                Reference = "",
                StockItems = new List<StockTransItemDto>
            {
                new StockTransItemDto { ItemId = 1, ItemName = "Item 1", Qty = 10, Rate = 25.5M, Amount = 255 }
            },
                references = new List<ReferenceDto>
                {
                    new ReferenceDto { Id = 1, ReferenceNo = "REF-001" }
            }
            };
            _stock.Setup(x => x.UpdateStockTrans(dto, 86, 229)).Returns(new CommonResponse { Exception = null, Data = new StockTransactionDto() });
            var result = _stock.Object.UpdateStockTrans(dto, 86, 229);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
