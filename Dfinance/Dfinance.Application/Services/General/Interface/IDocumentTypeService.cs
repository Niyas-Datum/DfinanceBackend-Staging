using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IDocumentTypeService
    {
        CommonResponse FillDocTypeMaster();
        CommonResponse FillById(int id);
        CommonResponse SaveDocType(int pageId, DocumentTypeDto docTypeDto);
        CommonResponse DeleteDocType(int pageId, int Id);
    }
}
