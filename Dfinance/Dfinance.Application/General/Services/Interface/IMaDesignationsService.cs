using Dfinance.Application.Dto.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.General.Services.Interface
{
    public interface IMaDesignationsService
    {
        CommonResponse GetAllDesignation();
        CommonResponse GetAllDesignationById(int Id);
        CommonResponse AddDesignations(MaDesignationsDto designationsdto);
        CommonResponse UpdateDesignation(MaDesignationsDto designationsdto,int Id);
        CommonResponse DeleteDesignation(int Id);
    }
}
