namespace Dfinance.Core.Views.Inventory
{
    public class WareHouseView
    {
        public Warehousebyid? warehousebyid { get; set; }

        public  List<WarehouseBranchView>? warehousebranch{get;set;}

    }
    public class Warehousebyid
    {


        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int LocationTypeID { get; set; }
        public string LocationType { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public decimal? ClearingPerCFT { get; set; }
        public decimal? GroundRentPerCFT { get; set; }
        public decimal? LottingPerPiece { get; set; }
        public decimal? LorryHirePerCFT { get; set; }
    }
    public class WarehouseBranchView
    {
        public int? LocationID { get; set; }
        public int? BranchID { get; set; }
        public bool? IsDefault { get; set; }
        public bool? Active { get; set; }
    }
    public class Warehousebranchfill
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
    }
}
