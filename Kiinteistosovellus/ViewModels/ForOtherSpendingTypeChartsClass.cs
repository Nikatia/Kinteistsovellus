﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class ForOtherSpendingTypeChartsClass
    {
        public System.DateTime DateBegin { get; set; }
        public Nullable<decimal> DailySpendings { get; set; }
        public int OtherSpendingTypeId { get; set; }
    }
}