using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Inventory.Interface
{
    public interface IQualityTypeService
    {
        CommonResponse QualityinCaretPopup();
        CommonResponse Fill();
        CommonResponse SaveEditQualityType(List<QualityTypeDto> qualityTypeDto);
    }
}
