using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Stakeholder.Services.Interface
{
    /// <summary>
    /// Customer Suppllier  -  delivery Module 
    /// </summary>
    public interface ICsDeliveryService
    {
        bool Save(DeliveryDetailsDto deliveryDetailsDto, int partyId);
    }
}
