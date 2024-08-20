using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.Item
{
    public class ItemMappingTest
    {
        private Mock<IItemMappingService> _item;

        [SetUp]
        public void Setup()
        {
            _item = new Mock<IItemMappingService>();

        }
        [Test]
        public void UpdateTest()
        {
            var dto = new ItemMappingDto
            {
                 ItemId = 1,
              Items= new List<PopUpDto>
            {
                new PopUpDto { Id = 1, Name = "Item 1" },
                new PopUpDto { Id = 2, Name = "Item 2" }
              }
           
            };
            _item.Setup(x => x.SaveItemMapping(dto, 267)).Returns(new CommonResponse { Exception = null, Data = new SizeMasterDto() });
            var result = _item.Object.SaveItemMapping(dto, 267);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
