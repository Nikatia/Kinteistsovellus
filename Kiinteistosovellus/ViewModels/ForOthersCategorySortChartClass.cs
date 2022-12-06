using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class ForOthersCategorySortChartClass
    {
        public string Category { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> SpendingYear { get; set; }
    }
}