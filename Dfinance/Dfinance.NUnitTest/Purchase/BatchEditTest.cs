using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.NUnitTest.Purchase
{
    public class BatchEditTest
    {
        private Mock<IBatchEditService> _batchEdit;

        [SetUp]
        public void Setup()
        {
            _batchEdit = new Mock<IBatchEditService>();

        }
        [Test]
        public void GetLoadData()
        {
           
            _batchEdit.Setup(x => x.GetLoadData()).Returns(new CommonResponse { Exception = null, Data = new BatchEditDto() });
            var result = _batchEdit.Object.GetLoadData();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void BatchDetailsForUpdate()
        {
            BatchEditDto batchEdit = new BatchEditDto()
            {
                BatchNo = new DataModels.Dto.Common.PopUpDto
                {
                    Id = 1,
                    Name ="1001"
                },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ItemId=new DataModels.Dto.Common.PopUpDto
                {
                    Id=null,
                },
                PartyId = null,
                ExpiryDate = DateTime.Now,

            };
            _batchEdit.Setup(x => x.BatchDetailsForUpdate(batchEdit)).Returns(new CommonResponse { Exception = null, Data = new BatchEditDto() });
            var result = _batchEdit.Object.BatchDetailsForUpdate(batchEdit);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateBatchNo()
        {
            int pageId = 100;
            string vNo = "Pu001";
            string batchNo = "1001";
            string newBatchNo = "100111";
            var expiry= DateTime.Now;
            _batchEdit.Setup(x => x.UpdateBatchNo(pageId,vNo,batchNo,newBatchNo,expiry)).Returns(new CommonResponse { Exception = null, Data = new BatchEditDto() });
            var result = _batchEdit.Object.UpdateBatchNo(pageId, vNo, batchNo, newBatchNo, expiry);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
