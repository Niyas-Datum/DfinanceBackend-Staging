using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class InvSizeMaster
    {
        public InvSizeMaster()
        {
            InvTransItems = new HashSet<InvTransItems>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool? Active { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<InvTransItems> InvTransItems { get; set; }
    }
}
