using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Common
{
    /*User,*/
    public class DropDownViewName//For retrieving Id and Name
    {
        public int ID { get; set; }
        public string? Name { get; set; }
    }

    public class DropDownViewValue//For retrieving Id and Value
    {
        public int ID { get; set; }
        public string? Value { get; set; }
    }
    public class DropDownViewDesc
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
