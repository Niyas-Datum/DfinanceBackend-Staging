﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class SpFillCostCategoryByIdG
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllocateRevenue { get; set; }
        public bool AllocateNonRevenue { get; set; }
        public bool Active { get; set; }
    }
    public class SpFillCostCategoryG
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
