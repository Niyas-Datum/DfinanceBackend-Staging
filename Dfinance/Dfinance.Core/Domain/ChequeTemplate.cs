using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class ChequeTemplate
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? DateFormat { get; set; }
        public string? DateSeperator { get; set; }

        public virtual FiMaAccount Account { get; set; } = null!;
        public virtual ICollection<ChequeTemplateField> ChequeTemplateFields { get; set; }
    }
}
