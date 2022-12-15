using Kiinteistosovellus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class MoveMonthSpendings
    {
        public int MonthlySpendingID { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateEnd { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        public int SpendingTypeID { get; set; }

        public Nullable<decimal> AmountOfUnits { get; set; }

        [DataType(DataType.Currency)]
        public Nullable<decimal> PricePerUnit { get; set; }

        [DataType(DataType.Currency)]
        public Nullable<decimal> TransferPayment { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        [DataType(DataType.Currency)]
        public decimal FullPrice { get; set; }
        public string PlanToDelete { get; set; }

        public Nullable<int> ContractorID { get; set; }

        public virtual Contractors Contractors { get; set; }
        public virtual MonthlySpendingTypes MonthlySpendingTypes { get; set; }
    }
}