using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class Vmdriver
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int? DriverId { get; set; }
        public string? Note { get; set; }

        public virtual HREmployee? Driver { get; set; }
        public virtual MaVehicles? Vehicle { get; set; }
    }
}
