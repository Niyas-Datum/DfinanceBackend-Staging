namespace Dfinance.Core.Domain
{
    public partial class MaSettings
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public int ModuleId { get; set; }
        public bool SystemSetting { get; set; }

    }
}
