using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class DeliveryDetails
    {
        public int Id { get; set; }

        public int? PartyId { get; set; }

        public string? LocationName { get; set; }

        public string? ProjectName { get; set; }

        public string? ContactPerson { get; set; }

        public string? ContactNo { get; set; }
        public string? Address { get; set; }

        public virtual Parties? Party { get; set; }
       
        public virtual ICollection <MaCustomerDetails> MaCustomerDetails { get; set; }

}
}
