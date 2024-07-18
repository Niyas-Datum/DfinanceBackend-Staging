using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface ISubMastersService
    {
        CommonResponse KeyDropDown();
        CommonResponse FillMaster(string Key);
        CommonResponse FillSubMasterById(int? Id);
        CommonResponse SaveSubMasters(SubMasterDto submasterDto);
        CommonResponse UpdateSubMasters(SubMasterDto submasterDto, int PageId);
        CommonResponse DeleteCounter(int Id, int PageId);

    }
}
