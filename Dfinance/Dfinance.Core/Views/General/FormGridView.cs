using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public partial class FormGridView
    {
        public int ID { get; set; }
        public string FormName { get; set; }
        public int? PageID { get; set; }
        public string? Page { get; set; }
        public string GridName { get; set; }
        public string ColumnName { get; set; }
        public string? OriginalCaption { get; set; }
        public string? NewCaption { get; set; }
        public bool? Visible { get; set; }
        public string? ArabicCaption { get; set; }
    }
    public partial class FormLabelView
    {
        public int ID { get; set; }
        public string FormName { get; set; }
        public string LabelName { get; set; }
        public string? OriginalCaption { get; set; }
        public string? NewCaption { get; set; }
        public bool? Visible { get; set; }
        public int? PageID { get; set; }
        public string? ArabicCaption { get; set; }
        public bool? Enable { get; set; }
    }
}
