using Dfinance.Core.Domain;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.Item
{
    public class SizeMasterTest
    {
        private Mock<ISizeMasterService> _sizeMaster;

        [SetUp]
        public void Setup()
        {
            _sizeMaster = new Mock<ISizeMasterService>();

        }
        [Test]
        public void FillSizeMasterTest()
        {

            _sizeMaster.Setup(x => x.FillMaster()).Returns(new CommonResponse { Exception = null, Data = new InvSizeMaster() });
            var result = _sizeMaster.Object.FillMaster();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillByIdTest()
        {

            _sizeMaster.Setup(x => x.FillById(5)).Returns(new CommonResponse { Exception = null, Data = new InvSizeMaster() });
            var result = _sizeMaster.Object.FillById(5);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SaveTest()
        {
            var dto = new SizeMasterDto
            {
                Id = 0,
                Code = "S001",
                Name = "Small",
                Description = "Small size",
                Active = false
            };
            _sizeMaster.Setup(x => x.SaveSizeMaster(dto, 267)).Returns(new CommonResponse { Exception = null, Data = new SizeMasterDto() });
            var result = _sizeMaster.Object.SaveSizeMaster(dto, 267);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteTest()
        {
            _sizeMaster.Setup(x => x.DeleteSizeMaster(45, 267)).Returns(new CommonResponse { Exception = null, Data = new SizeMasterDto() });
            var result = _sizeMaster.Object.DeleteSizeMaster(45, 267);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
