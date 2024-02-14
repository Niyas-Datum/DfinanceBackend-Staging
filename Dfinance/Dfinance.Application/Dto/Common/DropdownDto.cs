using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Dto.Common
{
    public class DropdownDto
    {
        public int? Id { get; set; }
        public string?  Value { get; set; } 

    }
    public class DropDownDtoName
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class  DropDownDtoNature
    {
        public string Key {  get; set; }
        public string Value { get; set; }
    }
    public class DropDownDesc
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
    }

}
