

namespace Dfinance.Core.Views.Common
{
    public class ReadView//retrieving ID,Code and Description
    {
        public int ID { get; set; }
        public string Code {  get; set; }
        public string? Name { get; set; } = null;
    }
    public class ReadViewDesc//retrieving ID,Code and Description
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
    }
}
