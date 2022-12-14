//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kiinteistosovellus.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class OtherSpendings
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
        public string ImageUrl { get; set; }
        public virtual Contractors Contractors { get; set; }
        public virtual OtherSpendingTypes OtherSpendingTypes { get; set; }
    }
}
