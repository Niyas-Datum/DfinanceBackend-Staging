using Dfinance.DataModels.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface ICostCentreService
    {
        CommonResponse SaveCostCentre(CostCentreDto costCentreDto);

        CommonResponse FillCostCentre();

        CommonResponse FillCostCentreById(int Id);

        CommonResponse UpdateCostCentre(CostCentreDto costCentreDto, int Id);

        CommonResponse DeleteCostCentre(int Id);

        CommonResponse FillCostCentreDropDown();

        CommonResponse FillPopUp(string Description);

        //CommonResponse FillConsultancy()
    }
}
