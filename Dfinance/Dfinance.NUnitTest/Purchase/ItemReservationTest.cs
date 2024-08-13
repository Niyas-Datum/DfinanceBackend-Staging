using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.NUnitTest.Purchase
{
    public class ItemReservationTest
    {
        private Mock<IItemReservationService> _itemReserv;

        [SetUp]
        public void Setup()
        {
            _itemReserv = new Mock<IItemReservationService>();
        }
        [Test]
        public void GetLoadData()
        {

            _itemReserv.Setup(x => x.GetLoadData()).Returns(new CommonResponse { Exception = null, Data = new ItemReservationDto() });
            var result = _itemReserv.Object.GetLoadData();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void SaveItemReserv()
        {
            int PageId = 452;
            int voucherId = 139;
            ItemReservationDto ItemReservation = new ItemReservationDto()
            {
                Id = 0,
                VoucherNo="001",
                Date = DateTime.Now,
                Party=new DataModels.Dto.Common.PopUpDto()
                {
                    Id=1536,
                },
                Project=new DataModels.Dto.Common.PopUpDto()
                {
                    Id=68
                },
                Description="Test",
                PartyInfo="Aswathy",
                Warehouse = new DataModels.Dto.Common.DropDownDtoName()
                {
                    Id=62
                },
                Items=new List<InvTransItemReservDto>()
                {
                   new InvTransItemReservDto()
                   {
                       TransactionId=0,
                       ItemId=8,
                       ItemCode="001",
                       ItemName="test",
                       Unit=new UnitPopupDto()
                       {
                           Unit="No",
                       },
                       Qty=2,
                   }
                },
            };
            _itemReserv.Setup(x => x.SaveItemReserv(ItemReservation,PageId,voucherId)).Returns(new CommonResponse { Exception = null, Data = new ItemReservationDto() });
            var result = _itemReserv.Object.SaveItemReserv(ItemReservation, PageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateItemReserv()
        {
            int PageId = 452;
            int voucherId = 139;
            ItemReservationDto ItemReservation = new ItemReservationDto()
            {
                Id = 5198,
                VoucherNo = "001",
                Date = DateTime.Now,
                Party = new DataModels.Dto.Common.PopUpDto()
                {
                    Id = 1536,
                },
                Project = new DataModels.Dto.Common.PopUpDto()
                {
                    Id = 68
                },
                Description = "Test",
                PartyInfo = "Aswathy",
                Warehouse = new DataModels.Dto.Common.DropDownDtoName()
                {
                    Id = 62
                },
                Items = new List<InvTransItemReservDto>()
                {
                   new InvTransItemReservDto()
                   {
                       TransactionId=0,
                       ItemId=8,
                       ItemCode="001",
                       ItemName="test",
                       Unit=new UnitPopupDto()
                       {
                           Unit="No",
                       },
                       Qty=2,
                   }
                },
            };
            _itemReserv.Setup(x => x.UpdateItemReserv(ItemReservation, PageId, voucherId)).Returns(new CommonResponse { Exception = null, Data = new ItemReservationDto() });
            var result = _itemReserv.Object.UpdateItemReserv(ItemReservation, PageId, voucherId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillMaster()
        {
            int pageId = 452;
            var transId = 139;
            _itemReserv.Setup(x => x.FillMaster(pageId,transId)).Returns(new CommonResponse { Exception = null, Data = new ItemReservationDto() });
            var result = _itemReserv.Object.FillMaster(pageId,transId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillById()
        {            
            var transId = 139;
            _itemReserv.Setup(x => x.FillById( transId)).Returns(new CommonResponse { Exception = null, Data = new ItemReservationDto() });
            var result = _itemReserv.Object.FillById(transId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
