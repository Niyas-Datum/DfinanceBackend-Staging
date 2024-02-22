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
        
        CommonResponse FillCategory(); // customer supplier category 
        CommonResponse SaveGen(GeneralDto generalDto);
		CommonResponse FillPartyType();
    }
}
