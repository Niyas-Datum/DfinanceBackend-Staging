using Dfinance.Core.Domain;
using Dfinance.Core.Views.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views
{
   
    public class ItemUnitsListView
    {
       public ProductVew Items { get; set; }
        public List<ItemUnitRestView>? ItemUnits { get; set; }
        public List<ItemOptionsView>? ItemOptions { get; set; }
    }
}
