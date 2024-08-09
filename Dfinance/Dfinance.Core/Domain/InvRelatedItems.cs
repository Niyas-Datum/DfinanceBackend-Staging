namespace Dfinance.Core.Domain
{
    public class InvRelatedItems
    {
        public int Id { get; set; }
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }

        //public virtual ItemMaster ItemId1Navigation { get; set; } = null!;
       // public virtual ItemMaster ItemId2Navigation { get; set; } = null!;
    }
}
