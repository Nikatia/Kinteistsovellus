using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class YearlyMonthlySpendingsByType
    {
        public Nullable<int> Vuosi { get; set; }
        public decimal HintaYhteensa { get; set; }
        public int Tyyppi { get; set; }
    }
}