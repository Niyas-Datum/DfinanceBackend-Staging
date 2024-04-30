namespace Dfinance.Core.Domain
{
    public class MaVehicles
    {
        public int Id { get; set; }
        public string RegistrationNo { get; set; } = null!;
        public int VehicleTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public int CreatedBranchId { get; set; }
        public decimal? AcqMeterValue { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public string? Color { get; set; }
        public string? Vinno { get; set; }
        public string? EngineNo { get; set; }
        public int? DepartmentId { get; set; }
        public int? FuelTypeId { get; set; }
        public int? MeterTypeId { get; set; }
        public int? SecondMeterTypeId { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public string? Ownership { get; set; }
        public DateTime? AcquisitionDate { get; set; }
        public string? AcquisitionRefNo { get; set; }
        public int? AcquisitionPartyId { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public string? AcquisitionNote { get; set; }
        public int? InsurancePartyId { get; set; }
        public string? InsuranceRefNo { get; set; }
        public string? InsuranceNote { get; set; }
        public string? LicenceRefNo { get; set; }
        public string? LicenceNote { get; set; }
        public int? CostCenterId { get; set; }

        public virtual MaCompany CreatedBranch { get; set; } = null!;
        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;
        //  public virtual ICollection<TransLoadSchedule> TransLoadSchedules { get; set; }
        //  public virtual ICollection<Vmdriver> Vmdrivers { get; set; }
    }
}
