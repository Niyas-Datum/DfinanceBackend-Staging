using Dfinance.Application.Dto.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.General.Services.Interface
{
    public interface IDesignationsService
    {
        CommonResponse FillAllDesignation();
        CommonResponse FillDesignationById(int Id);
        CommonResponse SaveDesignations(DesignationsDto designationsdto);
        CommonResponse UpdateDesignation(DesignationsDto designationsdto,int Id);
        CommonResponse DeleteDesignation(int Id);
    }
}
