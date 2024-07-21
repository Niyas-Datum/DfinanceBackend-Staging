using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.General;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.NUnitTest.General
{
    public class PageMenuTest
    {
        private Mock<IPageMenuService> _pageMenu;
        [SetUp]
        public void Setup()
        {
            _pageMenu = new Mock<IPageMenuService>();
        }
        [Test]
        public void SavePageMenuTest()
        {
            var pageMenuDto = new PageMenuDto
            {
                Id = 1,
                MenuText = "Home",
                Group = new DropdownDto { Id = 1, Value = "Main Group" },
                AssemblyName = "Dfinance",
                Active = true,
                UrlID = "home",
                IsFinanceRef = false,
                Module = new DropdownDto { Id = 2, Value = "Module 1" },
                MenuPermission = new DropDownDtoNature { Key ="C", Value = "" },
                IsMaximized = true,
                FrequentlyUsed = true,
                ArabicName = " ",
                MenuValue = "Home",
                IsPage = true,
                FormName = "HomePage",
                MenuOrder = 1,
                Url = "/home",
                Voucher = new DropdownDto { Id = 4, Value = "Voucher 1" },
                PageTitle = "Home Page",
                MenuLevel = 1,
                MDIParent = false,
                ShortcutKey = "Ctrl+H",
                ProcedureName = "GetHomePage"
            };
            _pageMenu.Setup(x => x.SavePageMenu(pageMenuDto, null)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new PageMenuDto() });
            var result = _pageMenu.Object.SavePageMenu(pageMenuDto, null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void FillMenuTest()
        {
            bool AllPages = true;
            int? ParentId = null;
            _pageMenu.Setup(x => x.FillMenu(AllPages, ParentId)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new PageMenuDto() });
            var result = _pageMenu.Object.FillMenu(AllPages, ParentId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void DeletePageMenuTest() 
        {
            int pageId = 100;
            _pageMenu.Setup(x => x.DeletePageMenu(pageId)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new PageMenuDto() });
            var result = _pageMenu.Object.DeletePageMenu(pageId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void UpdateActiveTest() 
        {
            List<PageActiveDto> pageActiveDtos = new List<PageActiveDto>
        {
            new PageActiveDto { PageId = 1, Active = true, FrequentlyUsed = false },
            new PageActiveDto { PageId = 2, Active = false, FrequentlyUsed = true }
        };
            _pageMenu.Setup(x => x.UpdateActive(pageActiveDtos)).Returns(new Shared.Domain.CommonResponse { Exception = null, Data = new PageMenuDto() });
            var result = _pageMenu.Object.UpdateActive(pageActiveDtos);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}
