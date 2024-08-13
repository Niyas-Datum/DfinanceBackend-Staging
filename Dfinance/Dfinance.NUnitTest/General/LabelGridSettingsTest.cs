using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Moq;

namespace Dfinance.NUnitTest.General
{
    public class LabelGridSettingsTest
    {
        private Mock<ILabelAndGridSettings> _labelgrid;
        [SetUp]
        public void Setup()
        {
            _labelgrid = new Mock<ILabelAndGridSettings>();
        }
        [Test]
        public void SaveUpdateLabel()
        {
            LabelDto labelDto = new LabelDto
            {
                 Id= 1,
                FormName = new Formpopup
                {
                    Name = "frmWPS"
                },
                LabelName="lblWPS",
                OriginalCaption="frmWps",
                NewCaption="frmWps",
                ArabicCaption="Arb",
                Visible=true
            };
            var labelDtoList = new List<LabelDto> { labelDto };

            // Setup the mock
            _labelgrid.Setup(x => x.SaveAndUpdateLabel(labelDtoList,"78001"))
                      .Returns(new CommonResponse { Exception = null, Data = new LabelDto() });
            var result = _labelgrid.Object.SaveAndUpdateLabel(labelDtoList, "78001");
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void SaveUpdateGrid()
        {
            GridDto gridDto = new GridDto
            {
                Id = 1,
                FormName = new Formpopup
                {
                    Name = "frmWPS"
                },
                PageId=15,
                Page=new GridPopup
                {
                    Id=0,
                    Name="frmWPS"
                },
                GridName = "lblWPS",
                ColumnName= "frmWps",
                OriginalCaption = "frmWps",
                NewCaption = "frmWps",
                ArabicCaption = "Arb",
                Visible = true
            };
            var gridDtoList = new List<GridDto> { gridDto };

            // Setup the mock
            _labelgrid.Setup(x => x.SaveAndUpdateGrid(gridDtoList, "78001"))
                      .Returns(new CommonResponse { Exception = null, Data = new GridDto() });
            var result = _labelgrid.Object.SaveAndUpdateGrid(gridDtoList, "78001");
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillGridSettings()
        {
            _labelgrid.Setup(x => x.FillGridSettings())
                    .Returns(new CommonResponse { Exception = null, Data = new FormGridView() });
            var result = _labelgrid.Object.FillGridSettings();
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillFormLabelSettings()
        {
            _labelgrid.Setup(x => x.FillFormLabelSettings())
                    .Returns(new CommonResponse { Exception = null, Data = new FormLabelView() });
            var result = _labelgrid.Object.FillFormLabelSettings();
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }





    }
}
