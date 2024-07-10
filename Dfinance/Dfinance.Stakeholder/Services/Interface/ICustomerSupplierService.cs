using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Stakeholder.Services.Interface
{
    public interface ICustomerSupplierService
    {
        CommonResponse GetCode();

        //customer supplier- general form
        CommonResponse FillSupplier(int locId, int pageId, int voucherId, string criteria);
        CommonResponse FillCategory(); // customer supplier category 
        CommonResponse SaveGen(GeneralDto generalDto,int pageId);
		CommonResponse FillPartyType();
        CommonResponse FillParty();
        CommonResponse FillPartyWithID(int Id,int pageId);
        CommonResponse Delete(int Id,int pageId);
    }
}
