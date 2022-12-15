using Kiinteistosovellus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class MoveOthSpendings
    {
        public int OtherSpendingsID { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        public int OtherSpendingTypeID { get; set; }

        public Nullable<int> ContractorID { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string PlanToDelete { get; set; }

        public virtual Contractors Contractors { get; set; }
        public virtual OtherSpendingTypes OtherSpendingTypes { get; set; }
    }
}