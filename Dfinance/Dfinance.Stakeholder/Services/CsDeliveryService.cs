using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Domain;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Stakeholder.Services
{
    /// <summary>
    /// General- customer supllier -  Delivery
    /// </summary>
    public class CsDeliveryService : ICsDeliveryService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public CsDeliveryService(DFCoreContext context, IAuthService authService)
        {
            this._context = context;
            this._authService = authService;

        }

        public bool Save(DeliveryDetailsDto deliveryDetailsDto, int partyId)
        {
            try
            {
                var deliveryDat = _context.DeliveryDetails
                                                    .Where(dd => dd.PartyId == partyId && dd.Id == deliveryDetailsDto.DelId)
                                                    .Count();
                //[1 2 3 4 5]

                if (deliveryDat ==0)
                {
                    string criteria2 = "InsertDeliveryDetails";
                    SqlParameter newIdDeliv = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw("EXEC PartyMasterSP @Criteria={0},@DDPartyID={1},@LocationName={2},@ProjectName={3},@DDContactPerson={4},@ContactNo={5},@DDAddress ={6},@NewID= {7} OUTPUT ",
                        criteria2,
                        partyId,
                        deliveryDetailsDto.LocationName,
                        deliveryDetailsDto.ProjectName,
                        deliveryDetailsDto.ContactPerson,
                        deliveryDetailsDto.ContactNo,
                        deliveryDetailsDto.Address,
                        newIdDeliv
                    );
                    int newDeliveryId = (int)newIdDeliv.Value;
                    return true;
                }

                else
                {


                    string criteria2 = "UpdateDeliveryDetails";
                 
                        _context.Database.ExecuteSqlRaw("EXEC PartyMasterSP @Criteria={0},@DDPartyID={1},@LocationName={2},@ProjectName={3},@DDContactPerson={4},@ContactNo={5},@DDAddress ={6},@ID= {7} ",
                            criteria2,
                            partyId,
                            deliveryDetailsDto.LocationName,
                            deliveryDetailsDto.ProjectName,
                            deliveryDetailsDto.ContactPerson,
                            deliveryDetailsDto.ContactNo,
                            deliveryDetailsDto.Address,
                            deliveryDetailsDto.DelId
                        );
                
                }
                    return true;
            }

            catch (Exception ex)
            {
                    return false;
            }
        }
    }
}
