namespace Dfinance.Core.Domain
{
    public partial class FiMaAccountGroup
    {
        public FiMaAccountGroup()
        {
            Ids = new HashSet<FiMaSubGroup>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<FiMaSubGroup> Ids { get; set; }
    }
}


