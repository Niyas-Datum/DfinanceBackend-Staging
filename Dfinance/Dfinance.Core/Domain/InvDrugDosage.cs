using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class InvDrugDosage
    {
        public int Id { get; set; }
        public string? Dosage { get; set; }
        public string? Remarks { get; set; }
        public bool? Active { get; set; }
    }
}
