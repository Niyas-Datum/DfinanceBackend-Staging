

namespace Dfinance.Core
{
    public partial class FormLabelSetting
    {
        public int Id { get; set; }
        public string FormName { get; set; } = null!;
        public string LabelName { get; set; } = null!;
        public string? OriginalCaption { get; set; }
        public string? NewCaption { get; set; }
        public bool? Visible { get; set; }
        public int? PageId { get; set; }
        public int? BranchId { get; set; }
        public string? ArabicCaption { get; set; }
        public bool? Enable { get; set; }
    }
}
