namespace Dfinance.Core.Views.PagePermission
{

    public class treeview
    {
        public int ID { get; set; }
        public string? MenuText { get; set; }
        public string? MenuValue { get; set; }
        public string? Url { get; set; }
        public int? ParentID { get; set; }
        public int? IsPage { get; set; } = 0;
        public int? VoucherID { get; set; }
        public string? ShortcutKey { get; set; }
        public string? ToolTipText { get; set; }
        public int? IsView { get; set; }
        public int? IsCreate { get; set; }
        public int? IsCancel { get; set; }
        public int? IsApprove { get; set; }
        public int? IsEditApproved { get; set; }
        public int? IsHigherApprove { get; set; }
        public int? IsPrint { get; set; }
        public int? IsEMail { get; set; }
        public int? IsEdit { get; set; }
        public int? IsDelete { get; set; }
        public List<treeview> Submenu { get; set; }
    }
    public class UserPageListView
    {
        public int ID { get; set; }
        public string? MenuText { get; set; }
        public string? MenuValue { get; set; }
        public string? Url { get; set; }
        public int? ParentID { get; set; }
        public int? IsPage { get; set; } = 0;
        public int? VoucherID { get; set; }
        public string? ShortcutKey { get; set; }
        public string? ToolTipText { get; set; }
        public int? IsView { get; set; }
        public int? IsCreate { get; set; }

        public int? IsCancel { get; set; }

        public int? IsApprove { get; set; }
        public int? IsEditApproved { get; set; }
        public int? IsHigherApprove { get; set; }
        public int? IsPrint { get; set; }
        public int? IsEMail { get; set; }
        public int? IsEdit { get; set; }
        public int? IsDelete { get; set; }
    }
        
}
