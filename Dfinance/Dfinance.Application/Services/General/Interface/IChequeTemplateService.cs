using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IChequeTemplateService
    {
        CommonResponse FillMaster();
        CommonResponse FillChqTemplate(int Id);
        CommonResponse FillChqTemplateField(int CheqTempId);
        CommonResponse SaveChequeTemplate(CheqTempDto cheqTempDto);
        CommonResponse UpdateCheqTemp(CheqTempDto cheqTempDto);
        CommonResponse DeleteCheqTemp(int CheqTempId);
        CommonResponse BankPopup();
    }
}
