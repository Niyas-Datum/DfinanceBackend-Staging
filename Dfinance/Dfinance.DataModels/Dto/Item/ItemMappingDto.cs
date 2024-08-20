using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Item
{
    public class ItemMappingDto
    {            
        public int ItemId { get; set; }
        public List<PopUpDto> Items { get; set; }

    }
}
