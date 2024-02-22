using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Common
{
    public class PopUpDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code {  get; set; }
        public string? Description { get; set; }
    }
    public class AccountNamePopUpDto
    {
        public string? Alias {  get; set; }
        public string? Name { get; set; }
        public int ID { get; set; }
    }
    
}
