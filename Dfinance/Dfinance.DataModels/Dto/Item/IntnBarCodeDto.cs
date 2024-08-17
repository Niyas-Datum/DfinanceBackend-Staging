using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Item
{
    public class IntnBarCodeDto
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public bool Active { get; set; }


    }
}
