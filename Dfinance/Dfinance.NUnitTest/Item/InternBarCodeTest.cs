using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.Item
{
    public class InternBarCodeTest
    {
        private Mock<IInternationalBarCodeService> _internbarcode;
        [SetUp]
        public void Setup()
        {
            _internbarcode = new Mock<IInternationalBarCodeService>();
        }
        [Test]
        public void Fill()
        {

            _internbarcode.Setup(x => x.FillInternBarCode()).Returns(new CommonResponse { Exception = null, Data = new IntnBarCodeDto() });
            var result = _internbarcode.Object.FillInternBarCode();
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SaveInternBarCode() 
        {
            IntnBarCodeDto intnBarCodeDto = new IntnBarCodeDto
            {
                Id = 1,
                BarCode = "88569743",
                Active = true,

            };
            var BarCode = new List<IntnBarCodeDto> { intnBarCodeDto };
            _internbarcode.Setup(x => x.SaveUpdateIntBarcCode(BarCode)).Returns(new CommonResponse { Exception = null, Data = new IntnBarCodeDto() });


            var result = _internbarcode.Object.SaveUpdateIntBarcCode(BarCode);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
