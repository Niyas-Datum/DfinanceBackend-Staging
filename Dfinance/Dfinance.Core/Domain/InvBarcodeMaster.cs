using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class InvBarcodeMaster
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = null!;
        public bool? Active { get; set; }
    }
}
