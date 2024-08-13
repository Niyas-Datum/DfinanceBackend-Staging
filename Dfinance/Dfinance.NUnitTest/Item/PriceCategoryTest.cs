using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.Item
{
    public class PriceCategoryTest
    {
        private Mock<IPriceCategoryService> _priceCat;

        [SetUp]
        public void Setup()
        {
            _priceCat = new Mock<IPriceCategoryService>();
        }
        [Test]
        public void FillMasterTest()
        {

            _priceCat.Setup(x => x.FillMaster()).Returns(new CommonResponse { Exception = null, Data = new PriceCategoryDto() });
            var result = _priceCat.Object.FillMaster();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillPriceCategoryById()
        {
            int id = 26;
            _priceCat.Setup(x => x.FillPriceCategoryById(id)).Returns(new CommonResponse { Exception = null, Data = new PriceCategoryDto() });
            var result = _priceCat.Object.FillPriceCategoryById(id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SavePriceCategory()
        {
            int id = 244;
            PriceCategoryDto priceCat = new PriceCategoryDto()
            { 
                Id=0,
                CategoryName="Test",
                SellingPrice=10,
                Active=true,
                Description="Test",
            };
            _priceCat.Setup(x => x.SavePriceCategory(priceCat, id)).Returns(new CommonResponse { Exception = null, Data = new PriceCategoryDto() });
            var result = _priceCat.Object.SavePriceCategory(priceCat, id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdatePriceCategory()
        {
            int id = 244;
            PriceCategoryDto priceCat = new PriceCategoryDto()
            {
                Id = 0,
                CategoryName = "Test",
                SellingPrice = 10,
                Active = true,
                Description = "Test",
            };
            _priceCat.Setup(x => x.UpdatePriceCategory(priceCat, id)).Returns(new CommonResponse { Exception = null, Data = new PriceCategoryDto() });
            var result = _priceCat.Object.UpdatePriceCategory(priceCat, id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeletePriceCategory()
        {
            int pageid = 244;
            int Id = 0;
            _priceCat.Setup(x => x.DeletePriceCategory(Id,pageid)).Returns(new CommonResponse { Exception = null, Data = new PriceCategoryDto() });
            var result = _priceCat.Object.DeletePriceCategory(Id, pageid);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
