

namespace Dfinance.Core.Views.Common
{
    public class ReadView//retrieving ID,Code and Description
    {
        public int ID { get; set; }
        public string? Code {  get; set; }
        public string? Name { get; set; } = null;
    }
    public class ReadViewDesc//retrieving ID,Code and Description
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }

    public class ReadViewVal//retrieving ID,Code and Value
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Value { get; set; }
    }
    public class ReadViewAlias//retrieving Alias,Name and ID
    {
        public string? Alias { get; set; }
        
        public string? Name { get; set; }
        public int ID { get; set; }
    }
}
