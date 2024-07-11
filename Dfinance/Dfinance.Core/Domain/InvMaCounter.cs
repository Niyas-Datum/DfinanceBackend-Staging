namespace Dfinance.Core.Domain
{
    public partial class InvMaCounter
    {
        public int Id { get; set; }
        public string MachineName { get; set; } = null!;
        public string CounterCode { get; set; } = null!;
        public string CounterName { get; set; } = null!;
        public string? MachineIp { get; set; }
        public bool Active { get; set; }
    }
}
