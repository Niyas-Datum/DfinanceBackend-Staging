using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.General
{
    public class LabelDto
    {
        public int Id { get; set; } 
        public Formpopup FormName { get; set; }
        public string LabelName { get; set; }
        public string? OriginalCaption { get; set; }
        public string? NewCaption { get; set; }
        //public int? PageId { get; set; }
        public string? ArabicCaption { get; set; }
        public bool? Visible { get; set; }
        //public bool? Enable { get; set; }
    }
    public class GridDto
    {
        public int Id { get; set; }
        public Formpopup FormName { get; set; }
        public int? PageId { get; set; }
        public GridPopup? Page {  get; set; }
        public string GridName { get; set; }
        public string ColumnName { get; set; }
        public string? OriginalCaption { get; set; }
        public string? NewCaption { get; set; }
        public string? ArabicCaption { get; set; }
        public bool? Visible { get; set; }

    }

}
