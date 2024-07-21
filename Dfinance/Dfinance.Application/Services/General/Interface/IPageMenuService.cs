using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IPageMenuService
    {
        CommonResponse FillMenu(bool AllPages, int? parentId = null);
        CommonResponse FillGroupAndModules();
        CommonResponse SavePageMenu(PageMenuDto pageMenuDto, int? parentId = null);
        CommonResponse DeletePageMenu(int Id);
        CommonResponse UpdateActive(List<PageActiveDto> pageActiveDto);
    }
}
