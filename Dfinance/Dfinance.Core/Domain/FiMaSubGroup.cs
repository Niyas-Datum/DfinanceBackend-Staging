using Dfinance.Core;

namespace Dfinance.Core.Domain
{
    public partial class FiMaSubGroup
    {
        public FiMaSubGroup()
        {
            Lists = new HashSet<FiMaAccountGroup>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int? OrderNo { get; set; }
        public int? DivisionNo { get; set; }
        public string? GroupType { get; set; }
        public string? MajorGroup { get; set; }

        public virtual ICollection<FiMaAccountGroup> Lists { get; set; }
    }
}
