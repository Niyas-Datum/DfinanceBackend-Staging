﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views
{
    public class ItemOptionsListView
    {
        public int ItemId { get; set; }
        public List<ItemOptionsView>? ItemOptions {  get; set; }
    }
}
