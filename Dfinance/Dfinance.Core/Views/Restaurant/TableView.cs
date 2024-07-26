using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views
{
    public class TableView
    {
        public int Id { get; set; }
        public string? TableName { get; set; }
        public int? TransactionID { get; set; }
        public string? BTime { get; set; }
        public string? TransactionNo { get; set; }
        public string? ChairName { get; set;}
    }
}
