using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class ForMonthlySpendingsChartsClass
    {
        public string MonthYearDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int SpendingTypeID { get; set; }
    }
}