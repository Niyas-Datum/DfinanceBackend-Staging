using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.Core.Domain
{
    public class LogInfo
    {
        public long ID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime LogInTime { get; set; }
        public string SessionID { get; set; }
        public DateTime? LogOutTime { get; set; }
        public char? LogOutMode { get; set; }

        // Navigation property for the foreign key relationship with MaEmployees
        public MaEmployee MaEmployee { get; set; }
    }
}
