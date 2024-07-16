using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Common
{
    public class DropdownDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("value")]
        public string?  Value { get; set; } 

    }
    public class DropDownDtoName
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    public class  DropDownDtoNature
    {
        [JsonPropertyName("key")]
        public string? Key {  get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
    public class DropDownDesc
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
    }

}
