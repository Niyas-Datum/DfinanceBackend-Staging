using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain.Roles
{
    public partial class MaPageMenu
    {
        public int Id { get; set; }

        public string MenuText { get; set; } = null!;

        public string MenuValue { get; set; } = null!;

        public string? UrlId { get; set; }

        public string? Url { get; set; }

        public bool? IsFinanceRef { get; set; }

        public int? MenuOrder { get; set; }

        public int? VoucherId { get; set; }

        public bool? Active { get; set; }

        public int? ModuleId { get; set; }

        public int? ParentId { get; set; }

        public bool? IsPage { get; set; }

        public int? MenuLevel { get; set; }

        public string? MenuPermission { get; set; }

        public string? PageTitle { get; set; }

        public string? AssemblyName { get; set; }

        public string? FormName { get; set; }

        public bool? IsMaximized { get; set; }

        public bool? Mdiparent { get; set; }

        /// <summary>
        /// To set the page ID of form, that has to be drilled down or traversed from this main page
        /// </summary>
        public int? RefPageId { get; set; }

        public int? HelpId { get; set; }

        public bool? FrequentlyUsed { get; set; }

        public string? ShortcutKey { get; set; }

        public string? ArabicName { get; set; }

        public string? ProcedureName { get; set; }

        //user permission
        public virtual ICollection<MaUserRight> MaUserRights { get; set; } = new List<MaUserRight>();

    }
}
