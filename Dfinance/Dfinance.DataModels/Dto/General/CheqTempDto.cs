using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.General
{
    public class CheqTempDto
    {
        public int Id { get; set; }
        [NullValidation(ErrorMessage = "Code is Mandatory!!")]
        public string Code { get; set; } 
        public string? Name { get; set; }
        public AccountNamePopUpDto? Bank { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? DateFormat { get; set; }
        public string? DateSeperator { get; set; }
        public List<CheqTempFields> CheqTempFields { get; set; }
    }
    public class CheqTempFields
    {
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
    }
}
