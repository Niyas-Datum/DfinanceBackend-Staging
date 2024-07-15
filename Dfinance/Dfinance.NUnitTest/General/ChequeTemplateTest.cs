using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.General
{
    public class ChequeTemplateTest
    {
        private Mock<IChequeTemplateService> _cheqTemp;
        [SetUp]
        public void Setup()
        {
            _cheqTemp = new Mock<IChequeTemplateService>();
        }
        [Test]
        public void ChequeTemplateSave()
        {
            CheqTempDto cheqDto = new CheqTempDto
            {
                Id = 0,
                Code = "CT001",
                Name = "Sample Cheque Template",
                Bank = new AccountNamePopUpDto
                {
                    Alias = "ABC",
                    Name = "ABC Bank",
                    ID = 1001
                },
                Width = 800,
                Height = 600,
                DateFormat = "dd/MM/yyyy",
                DateSeperator = "/",
                CheqTempFields = new List<CheqTempFields>
        {
            new CheqTempFields
            {
                ChequeTemplateId = 1,
                FieldId = "lblAccountPayee",
                Width = 200,
                Height = 30,
                Left = 100,
                Top = 50,
                Font = "Arial",
                FontSize = 12,
                FontStyle = "Bold",
                Casing = "Upper",
                Visible = true
            },
            new CheqTempFields
            {
                ChequeTemplateId = 1,
                FieldId = "lblPartyName",
                Width = 250,
                Height = 30,
                Left = 100,
                Top = 100,
                Font = "Arial",
                FontSize = 12,
                FontStyle = "Bold",
                Casing = "Upper",
                Visible = true
            }
        }
            };
            _cheqTemp.Setup(x => x.SaveChequeTemplate(cheqDto)).Returns(new CommonResponse { Exception = null, Data = new CheqTempDto() });
            var result = _cheqTemp.Object.SaveChequeTemplate(cheqDto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
        [Test]
        public void ChequeTemplateUpdate()
        {
            CheqTempDto cheqDto = new CheqTempDto
            {
                Id = 5,
                Code = "CT001",
                Name = "Sample Cheque Template",
                Bank = new AccountNamePopUpDto
                {
                    Alias = "ABC",
                    Name = "ABC Bank",
                    ID = 1001
                },
                Width = 800,
                Height = 600,
                DateFormat = "dd/MM/yyyy",
                DateSeperator = "/",
                CheqTempFields = new List<CheqTempFields>
        {
            new CheqTempFields
            {
                ChequeTemplateId = 1,
                FieldId = "lblAccountPayee",
                Width = 200,
                Height = 30,
                Left = 100,
                Top = 50,
                Font = "Arial",
                FontSize = 12,
                FontStyle = "Bold",
                Casing = "Upper",
                Visible = true
            },
            new CheqTempFields
            {
                ChequeTemplateId = 1,
                FieldId = "lblPartyName",
                Width = 250,
                Height = 30,
                Left = 100,
                Top = 100,
                Font = "Arial",
                FontSize = 12,
                FontStyle = "Bold",
                Casing = "Upper",
                Visible = true
            }
        }
            };
            _cheqTemp.Setup(x => x.UpdateCheqTemp(cheqDto)).Returns(new CommonResponse { Exception = null, Data = new CheqTempDto() });
            var result = _cheqTemp.Object.UpdateCheqTemp(cheqDto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);

        }
        [Test]
        public void Delete()
        {
            _cheqTemp.Setup(x => x.DeleteCheqTemp(5)).Returns(new CommonResponse { Exception = null, Data = new CheqTempDto() });
            var result = _cheqTemp.Object.DeleteCheqTemp(5);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillMaster()
        {
            _cheqTemp.Setup(x => x.FillMaster()).Returns(new CommonResponse { Exception = null, Data = new CheqTempDto() });
            var result = _cheqTemp.Object.FillMaster();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillChqTemplate()
        {
            _cheqTemp.Setup(x => x.FillChqTemplate(5)).Returns(new CommonResponse { Exception = null, Data = new CheqTempDto() });
            var result = _cheqTemp.Object.FillChqTemplate(5);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillChqTemplateField()
        {
            _cheqTemp.Setup(x => x.FillChqTemplateField(5)).Returns(new CommonResponse { Exception = null, Data = new CheqTempDto() });
            var result = _cheqTemp.Object.FillChqTemplateField(5);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
