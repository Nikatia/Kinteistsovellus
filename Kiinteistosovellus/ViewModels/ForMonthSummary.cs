using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class ForMonthSummary
    {
        public Nullable<int> MonthOfSpending { get; set; }
        public Nullable<int> YearOfSpending { get; set; }
        public decimal Kokonaishinta { get; set; }
        public string TypeName { get; set; }
    }
}