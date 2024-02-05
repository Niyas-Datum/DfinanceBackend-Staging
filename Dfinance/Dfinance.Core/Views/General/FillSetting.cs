namespace Dfinance.Core.Views.General
{
    public class FillSetting
    {
        public int? Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }

    }
    public class FillSettingById
    {
        public int? Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public bool? SystemSetting { get; set; }
    }
   
}
