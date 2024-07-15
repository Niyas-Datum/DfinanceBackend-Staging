using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

namespace Dfinance.Core.Domain
{
    public class ChequeTemplateField
    {
        public int Id { get; set; }
        public int ChequeTemplateId { get; set; }
        public string? FieldId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public string Font { get; set; } = null!;
        public int FontSize { get; set; }
        public string FontStyle { get; set; } = null!;
        public string? Casing { get; set; }
        public bool Visible { get; set; }

        public virtual ChequeTemplate ChequeTemplate { get; set; } = null!;
    }
}
