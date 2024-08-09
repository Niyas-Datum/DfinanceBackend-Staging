using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Item.Services.Interface
{
    public interface ISizeMasterService
    {
        CommonResponse FillMaster();
        CommonResponse FillById(int id);
        CommonResponse SaveSizeMaster(SizeMasterDto sizeMasterDto, int pageId); 
        CommonResponse DeleteSizeMaster(int id,int pageId);
    }
}
