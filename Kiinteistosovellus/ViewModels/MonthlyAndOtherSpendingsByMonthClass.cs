using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class MonthlyAndOtherSpendingsByMonthClass
    {
        public Nullable<int> Vuosi { get; set; }
        public decimal Tammikuu { get; set; }
        public decimal Helmikuu { get; set; }
        public decimal Maaliskuu { get; set; }
        public decimal Huhtikuu { get; set; }
        public decimal Toukokuu { get; set; }
        public decimal Kesäkuu { get; set; }
        public decimal Heinäkuu { get; set; }
        public decimal Elokuu { get; set; }
        public decimal Syyskuu { get; set; }
        public decimal Lokakuu { get; set; }
        public decimal Marraskuu { get; set; }
        public decimal Joulukuu { get; set; }
    }
}