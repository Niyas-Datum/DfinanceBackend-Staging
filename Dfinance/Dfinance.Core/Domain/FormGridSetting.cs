namespace Dfinance.Core.Domain
{
    public partial class FormGridSetting
    {
        public int Id { get; set; }
        public string FormName { get; set; } = null!;
        public string GridName { get; set; } = null!;
        public string ColumnName { get; set; } = null!;
        public string? OriginalCaption { get; set; }
        public string? NewCaption { get; set; }
        public bool? Visible { get; set; }
        public int? PageId { get; set; }
        public int? BranchId { get; set; }
        public string? ArabicCaption { get; set; }
    }
}
