using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.General
{
    public class SubMasterTest
    {
        private Mock<ISubMastersService> _submaster;
        [SetUp]
        public void Setup()
        {
            _submaster = new Mock<ISubMastersService>();
        }
        [Test]
        public void SaveSubmaster()
        {
            int pageid = 112;
            SubMasterDto subMasterDto = new SubMasterDto
            {
                Id = 0,
                Key = new dropdownkey
                { Value ="Area"},
       
                Code ="are",
                Value ="infopark",
                Description ="infocherthala"
            };
            _submaster.Setup(x => x.SaveSubMasters(subMasterDto,pageid)).Returns(new CommonResponse { Exception = null, Data = new SubMasterDto() });
            var result = _submaster.Object.SaveSubMasters(subMasterDto, pageid);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateSubmaster()
        {
            SubMasterDto subMasterDto = new SubMasterDto
            {
                Id = 1319,
                Key = new dropdownkey
                { Value = "Area" },

                Code = "are1",
                Value = "infopark2",
                Description = "infocherthala3"
            };
            _submaster.Setup(x => x.UpdateSubMasters(subMasterDto,112)).Returns(new CommonResponse { Exception = null, Data = new SubMasterDto() });
            var result = _submaster.Object.UpdateSubMasters(subMasterDto,112);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeleteSubmaster()
        {
            
            _submaster.Setup(x => x.DeleteCounter(1319, 112)).Returns(new CommonResponse { Exception = null, Data = new SubMasterDto() });
            var result = _submaster.Object.DeleteCounter(1319, 112);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void KeyDropdown() 
        {
            _submaster.Setup(x => x.KeyDropDown()).Returns(new CommonResponse { Exception = null, Data = new SubMasterDto() });
            var result = _submaster.Object.KeyDropDown();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillMaster()
        {
            string key = "Area";
            _submaster.Setup(x => x.FillMaster(key)).Returns(new CommonResponse { Exception = null, Data = new SubMasterDto() });
            var result = _submaster.Object.FillMaster(key);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillById()
        {
            int Id = 1319;
            _submaster.Setup(x => x.FillSubMasterById(Id)).Returns(new CommonResponse { Exception = null, Data = new SubMasterDto() });
            var result = _submaster.Object.FillSubMasterById(Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
