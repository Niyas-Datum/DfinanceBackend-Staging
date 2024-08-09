namespace Dfinance.Core.Views.Item
{
    public class ItemMappingCommon
    {
        public int ID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? BarCode { get; set; }
        public string? PartNo { get; set; }
    }
    public class ItemMappingView:ItemMappingCommon
    {
       
    }
    public class ItemDetailsView : ItemMappingCommon
    {
        public int ItemID1 {  get; set; }
        public int ItemID2 { get; set; }
    }
    
}
