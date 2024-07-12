using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.Finance
{
    public class AccConfigTest
    {
        private Mock<IAccountConfigurationService> _account;
        [SetUp]
        public void SetUp()
        {
            _account = new Mock<IAccountConfigurationService>();
        }

        [Test]
        public void FillAccConfigTest()
        {
            _account.Setup(x => x.FillAccConfig()).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new AccountConfigView() });
            var result = _account.Object.FillAccConfig();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void SaveAccConfigTest()
        {
            var accConfigDto = new List<AccConfigDto>
             {
            new AccConfigDto
            {
                Keyword = "CONTRACT ACCOUNT",
                Account = new PopUpDto
                {
                    Id = 211,
                    Name = "",
                    Code = "",
                    Description = ""
                }
            }
            };
            _account.Setup(x => x.SaveAccConfig(accConfigDto)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new AccountConfigView() });
            var result = _account.Object.SaveAccConfig(accConfigDto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
