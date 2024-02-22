using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Stakeholder.Services.Interface
{
    public interface ICustomerService
    {
        CommonResponse FillCommodity();
        CommonResponse FillPriceCategory();
        CommonResponse FillCustomerCategories();
        bool SaveCustomDetails(CustomerDetailsDto customerDetailsDto, decimal? CreditPeriod, int PartyId);
    }
}
